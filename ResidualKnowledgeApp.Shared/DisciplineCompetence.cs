namespace ResidualKnowledgeApp.Shared
{
	public class DisciplineCompetence
	{
		public int DisciplineId { get; set; }
		public Discipline Discipline { get; set; }

		public int CompetenceId { get; set; }
		public Competence Competence { get; set; }
	}
}
