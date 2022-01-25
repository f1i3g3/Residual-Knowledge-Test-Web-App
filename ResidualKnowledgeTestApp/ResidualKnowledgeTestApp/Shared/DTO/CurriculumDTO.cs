using System.Collections.Generic;

namespace ResidualKnowledgeTestApp.Shared.DTO
{
    public class CurriculumDTO
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string ProgrammeName { get; set; }

        public string ProgrammeCode { get; set; }

        public string LevelOfEducation { get; set; } // enum

        public string FileName { get; set; }

        public List<DisciplineDTO> Disciplines { get; set; }

        public List<CompetenceDTO> Competences { get; set; }
    }
}