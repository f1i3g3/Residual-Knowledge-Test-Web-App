namespace ResidualKnowledgeApp.Shared
{
    public class UserSelection
    {
        public int CheckingDisciplineId { get; set; }
        public CheckingDiscipline CheckingDiscipline { get; set; }

        public int CheckingCompetenceId { get; set; }
        public Competence CheckingCompetence { get; set; }
    }
}
