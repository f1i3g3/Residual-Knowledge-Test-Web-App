namespace ResidualKnowledgeApp.Shared
{
	public class Discipline : IEntity
	{
		public int Id { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public int Semester { get; set; }

		public List<Competence> Competences { get; set; } = new List<Competence>();

		public List<DisciplineCompetence> DisciplineCompetences { get; set; } = new List<DisciplineCompetence>();

		public int? CurriculumId { get; set; }

		public Curriculum Curriculum { get; set; }
	}
}