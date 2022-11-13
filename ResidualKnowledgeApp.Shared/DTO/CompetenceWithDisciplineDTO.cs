namespace ResidualKnowledgeApp.Shared.DTO
{
    public class CompetenceWithDisciplineDTO
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public DisciplineDTO Discipline { get; set; }
    }
}
