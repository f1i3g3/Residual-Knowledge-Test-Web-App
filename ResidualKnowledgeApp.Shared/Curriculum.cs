namespace ResidualKnowledgeApp.Shared
{
	public class Curriculum : IEntity
	{
		public int Id { get; set; }

		public string Code { get; set; }

		public string ProgrammeName { get; set; }

		public string ProgrammeCode { get; set; }

		public string LevelOfEducation { get; set; }

		public string FileName { get; set; }

		public int ProjectId { get; set; }

		public List<Discipline> Disciplines { get; set; }

		public List<Competence> Competences { get; set; }
	}
}