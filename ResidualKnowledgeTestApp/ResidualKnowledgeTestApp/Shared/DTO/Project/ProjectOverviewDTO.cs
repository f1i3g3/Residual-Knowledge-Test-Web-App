using System;

namespace ResidualKnowledgeTestApp.Shared.DTO
{
    public class ProjectOverviewDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProgrammeName { get; set; }

        public string ProgrammeCode { get; set; }

        public string LevelOfEducation { get; set; }

        public int Course { get; set; }

        public Stage Stage { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastEditionTime { get; set; }
    }
}
