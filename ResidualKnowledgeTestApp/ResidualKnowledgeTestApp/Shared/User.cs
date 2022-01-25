namespace ResidualKnowledgeTestApp.Shared
{
    public class User : IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string RightAnswersEmail { get; set; }

        public int ProjectId { get; set; }
    }
}
