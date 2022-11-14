using System.Security.Principal;

namespace ResidualKnowledgeApp.Shared
{
    public class User : IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string RightAnswersEmail { get; set; }

        public int ProjectId { get; set; }

        public Role Role { get; set; } = Role.User; // TODO
    }

    public enum Role
    {
        User,
        Adin
    }
}
