namespace ResidualKnowledgeApp.Shared
{
	public class MarkCriterion : IEntity
	{
		public int Id { get; set; }

		public int MinScore { get; set; }

		public int MaxScore { get; set; }

		public char ECTSMark { get; set; }

		public int FivePointScaleMark { get; set; }
	}
}
