using System.Collections.Generic;

namespace ResidualKnowledgeTestApp.Shared.DTO
{
    public class DisciplineDTO
    {
        public int Id { get; set; }

        public string DisciplineCode { get; set; }

        public string DisciplineName { get; set; }

        public int Semester { get; set; }

        public List<CompetenceDTO> Competences { get; set; }
    }
}
