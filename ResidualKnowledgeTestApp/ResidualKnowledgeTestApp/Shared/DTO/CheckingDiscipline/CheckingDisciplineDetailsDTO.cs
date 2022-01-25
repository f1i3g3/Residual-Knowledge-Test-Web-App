using System.Collections.Generic;

namespace ResidualKnowledgeTestApp.Shared.DTO
{
    public class CheckingDisciplineDetailsDTO
    {
        public int Id { get; set; }

        public int QuestionsCount { get; set; }

        public string MsFormsPath { get; set; }

        public string TxtTestFormPath { get; set; }

        public string MidCerificationResultsPath { get; set; }

        public string RightAnswersAuthorEmail { get; set; }

        public DisciplineDTO Discipline { get; set; }

        public List<CompetenceWithDisciplineDTO> CheckingCompetences { get; set; }

        public List<MarkCriterion> MarkCriteria { get; set; }

        //public User User { get; set; }
    }
}
