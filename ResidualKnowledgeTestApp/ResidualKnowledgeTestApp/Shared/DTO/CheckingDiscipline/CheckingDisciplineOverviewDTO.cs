using System.Collections.Generic;

namespace ResidualKnowledgeTestApp.Shared.DTO
{
    public class CheckingDisciplineOverviewDTO
    {
        public DisciplineDTO Discipline { get; set; }

        public List<CompetenceDTO> CheckingCompetences { get; set; }
    }
}
