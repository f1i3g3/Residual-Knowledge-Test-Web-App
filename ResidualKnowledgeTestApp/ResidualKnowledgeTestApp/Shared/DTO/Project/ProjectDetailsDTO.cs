using System;
using System.Collections.Generic;

namespace ResidualKnowledgeTestApp.Shared.DTO
{
    public class ProjectDetailsDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastEditionTime { get; set; }

        public Stage Stage { get; set; }

        public CurriculumDTO Curriculum { get; set; }

        public List<CheckingDisciplineDetailsDTO> CheckingDisciplines { get; set; } 
    }
}
