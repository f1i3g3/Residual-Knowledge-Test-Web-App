namespace ResidualKnowledgeApp.Shared
{
	public class CheckingDiscipline : IEntity
	{
		public int Id { get; set; }

		public int QuestionsCount { get; set; }

		public string MsFormsPath { get; set; }

		public string TxtTestFormPath { get; set; }

		public string MidCerificationResultsPath { get; set; }

		public string RightAnswersAuthorEmail { get; set; }

		public int DisciplineId { get; set; }

		public Discipline Discipline { get; set; }

		public int ProjectId { get; set; }

		public Project Project { get; set; }

		public List<Competence> CheckingCompetences { get; set; } = new List<Competence>();

		public List<UserSelection> UserSelection { get; set; } = new List<UserSelection>();

		public List<MarkCriterion> MarkCriteria { get; set; } = new List<MarkCriterion>();
	}
}
