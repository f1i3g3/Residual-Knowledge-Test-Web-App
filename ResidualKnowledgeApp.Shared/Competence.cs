namespace ResidualKnowledgeApp.Shared
{
	public class Competence : IEntity
	{
		public int Id { get; set; }

		public string Code { get; set; }

		public string Description { get; set; }

		public int? CurriculumId { get; set; }

		public List<Discipline> Disciplines { get; set; }

		public List<DisciplineCompetence> DisciplineCompetences { get; set; } = new List<DisciplineCompetence>();

		public List<CheckingDiscipline> CheckingDisciplines { get; set; }

		public List<UserSelection> UserSelection { get; set; } = new List<UserSelection>();
	}
}
