using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using ResidualKnowledgeTestApp.Shared.DTO;
using ResidualKnowledgeTestApp.Shared.ViewModels;

namespace ResidualKnowledgeTestApp.Client.Services
{
	public class ProjectsService : IProjectsService
	{
		private readonly HttpClient _httpClient;
		public ProjectsService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public string SheetLink { get; set; }

		public List<ProjectOverviewDTO> Projects { get; set; }

		public UserChoice UserChoice { get; private set; } = new UserChoice();

		public ProjectDetailsDTO Project { get; set; }

		public CurriculumDTO Curriculum { get; private set; }

		public List<CheckingDisciplineDetailsDTO> CheckingDisciplines { get; private set; } = new List<CheckingDisciplineDetailsDTO>();

		public List<DisciplineDTO> DisciplinesForSelection { get; private set; } = new List<DisciplineDTO>();

		public HashSet<CompetenceWithDisciplineDTO> CompetencesForSelection { get; private set; } = new HashSet<CompetenceWithDisciplineDTO>();

		public HashSet<CompetenceWithDisciplineDTO> SelectedCompetences { get; private set; } = new HashSet<CompetenceWithDisciplineDTO>();

		public event Action OnChange;

		public async Task<ProjectDetailsDTO> CreateProject(CreateProjectVM project)
		{
			var response = await _httpClient.PostAsJsonAsync($"api/projects", project);
			var created = await response.Content.ReadFromJsonAsync<ProjectDetailsDTO>();

			return created;
		}

		public async Task LoadProject(int projectId)
		{
			Project = await _httpClient.GetFromJsonAsync<ProjectDetailsDTO>($"api/projects/{projectId}");
			CheckingDisciplines = Project.CheckingDisciplines;
			DisciplinesForSelection = Project.Curriculum.Disciplines;
			var competencesForSelection = await _httpClient.GetFromJsonAsync<List<CompetenceWithDisciplineDTO>>($"api/projects/competences/selection/{Project.Id}");
			CompetencesForSelection = competencesForSelection.ToHashSet();

			var selectedCompetences = CheckingDisciplines.SelectMany(cd => cd.CheckingCompetences);
			SelectedCompetences = CompetencesForSelection
				.Where(cfs => null != selectedCompetences.FirstOrDefault(sc => sc.Id == cfs.Id && sc.Discipline.Id == cfs.Discipline.Id))
				.ToHashSet();

			UserChoice = new UserChoice
			{
				CurriculumSelected = Project.Curriculum != null,
				DisciplinesSelected = CheckingDisciplines != null && CheckingDisciplines.Count > 0,
				CompetencesSelected = SelectedCompetences.Count > 0,
			};

			if (Project.Stage == Stage.FilesUploading)
			{
				UserChoice.DisciplinesChanged = true;
				UserChoice.CompetencesShouldBeUpdated = false;
				UserChoice.FilesShouldBeUpdated = true;
			}
			else if (Project.Stage == Stage.CompetencesChoosing)
			{
				UserChoice.DisciplinesChanged = true;
				UserChoice.CompetencesShouldBeUpdated = true;
				UserChoice.FilesShouldBeUpdated = true;
			}

			SheetLink = Project.SheetLink is null ? "" : Project.SheetLink;

			OnChange.Invoke();
		}

		public async Task<CurriculumDTO> UploadCurriculumAsync(Curriculum curriculum, HttpContent content) // стоит сделать createCurriculumVM и запихать в него контент
		{
			await _httpClient.PostAsync("api/upload", content);

			var response = await _httpClient.PostAsJsonAsync($"api/curriculum", curriculum);
			Curriculum = await response.Content.ReadFromJsonAsync<CurriculumDTO>();

			await GetDisciplinesForSelectionAsync(Curriculum.Id);

			UserChoice.CurriculumSelected = true;

			await LoadProject(curriculum.ProjectId);

			OnChange.Invoke();
			return Curriculum;
		}

		private async Task<List<DisciplineDTO>> GetDisciplinesForSelectionAsync(int curriculumId)
		{
			DisciplinesForSelection = await _httpClient.GetFromJsonAsync<List<DisciplineDTO>>($"api/curriculum/disciplines/{curriculumId}");
			OnChange.Invoke();
			return DisciplinesForSelection;
		}

		public async Task SetCheckingDisciplines(IEnumerable<DisciplineDTO> selectedDisciplines)
		{
			var response = await _httpClient.PutAsJsonAsync($"api/projects/disciplines/set/{Project.Id}", selectedDisciplines.ToList());
			CheckingDisciplines = await response.Content.ReadFromJsonAsync<List<CheckingDisciplineDetailsDTO>>();

			UserChoice.DisciplinesSelected = true;
			UserChoice.DisciplinesChanged = true;

			await GetCompetencesForSelection();
			OnChange.Invoke();
		}

		private async Task<HashSet<CompetenceWithDisciplineDTO>> GetCompetencesForSelection()
		{
			var competencesForSelection = await _httpClient.GetFromJsonAsync<List<CompetenceWithDisciplineDTO>>($"api/projects/competences/selection/{Project.Id}");
			CompetencesForSelection = competencesForSelection.ToHashSet();
			SelectedCompetences = CompetencesForSelection
				.Where(cfs => null != SelectedCompetences.FirstOrDefault(sc => sc.Id == cfs.Id && sc.Discipline.Id == cfs.Discipline.Id))
				.ToHashSet();

			OnChange.Invoke();
			return CompetencesForSelection;
		}

		public async Task SetCheckingCompetences(IEnumerable<CompetenceWithDisciplineDTO> selectedCompetences)
		{
			var response = await _httpClient.PutAsJsonAsync($"api/projects/competences/set/{Project.Id}", selectedCompetences);
			SelectedCompetences = selectedCompetences.ToHashSet();

			UserChoice.CompetencesShouldBeUpdated = false;
			UserChoice.CompetencesSelected = true;

			OnChange.Invoke();
		}

		public async Task<List<ProjectOverviewDTO>> DeleteProject(int id)
		{
			var response = await _httpClient.DeleteAsync($"api/projects/{id}");
			Projects = await response.Content.ReadFromJsonAsync<List<ProjectOverviewDTO>>();
			OnChange.Invoke();
			return Projects;
		}

		public async Task<List<ProjectOverviewDTO>> GetProjectsAsync()
		{
			Projects = await _httpClient.GetFromJsonAsync<List<ProjectOverviewDTO>>("api/projects");
			return Projects;
		}

		public async Task<List<ProjectOverviewDTO>> UpdateProject(Project project, int id)
		{
			//var response = await _httpClient.PutAsJsonAsync($"api/projects/{id}", project);
			//Projects = await response.Content.ReadFromJsonAsync<List<Project>>(); // ...
			//OnChange.Invoke();
			return Projects;
		}

		public async Task UpdateCheckingDisciplineFiles(int id, CheckingDisciplineDetailsDTO updated)
		{
			await _httpClient.PutAsJsonAsync($"api/checkingdisciplines/{id}",
				new CheckingDiscipline
				{
					MidCerificationResultsPath = updated.MidCerificationResultsPath,
					MsFormsPath = updated.MsFormsPath,
					TxtTestFormPath = updated.TxtTestFormPath,
					QuestionsCount = updated.QuestionsCount
				});
		}

		public async Task GetCheckingDisciplines()
		{
			CheckingDisciplines = await _httpClient.GetFromJsonAsync<List<CheckingDisciplineDetailsDTO>>($"api/projects/disciplines/{Project.Id}");
			OnChange.Invoke();
		}

		public async Task UploadFileAsync(HttpContent content)
		{
			await _httpClient.PostAsync("api/upload", content);
		}

		public async Task SaveMarkCriteria(int checkingDisciplineId, List<MarkCriterion> markCriteria)
		{
			await _httpClient.PutAsJsonAsync($"api/checkingdisciplines/criteria/{checkingDisciplineId}", markCriteria);
		}

		public async Task GetSheetLink(int projectId)
		{
			var link = await _httpClient.GetStringAsync($"api/projects/{projectId}/sheetlink");

			if (link is not null)
			{
				SheetLink = link; 
			}
			else
			{
				SheetLink = "";
				throw new Exception();
			}
		}
	}
}

//var res = await _httpClient.GetAsync($"api/projects");
//if (res.IsSuccessStatusCode)
//{
//    Projects = await res.Content.ReadFromJsonAsync<List<ProjectOverviewDTO>>();
//}
//else
//{
//    var msg = await res.Content.ReadAsStringAsync();
//    Console.WriteLine(msg);
//    throw new Exception(msg);
//}

//var res = await _httpClient.GetAsync($"api/curriculum/disciplines/{id}");
//if (res.IsSuccessStatusCode)
//{
//    Disciplines = await res.Content.ReadFromJsonAsync<List<Discipline>>();
//}
//else
//{
//    var msg = await res.Content.ReadAsStringAsync();
//    Console.WriteLine(msg);
//    throw new Exception(msg);
//}

//public async Task<List<Curriculum>> GetCurriculums()
//{
//    throw new NotImplementedException();
//}
