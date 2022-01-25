using System.Collections.Generic;

namespace ResidualKnowledgeTestApp.Shared.ViewModels
{
    public class CreateCheckingDisciplineVM
    {
        public int Id { get; set; }

        public int QuestionsCount { get; set; }

        public string MsFormsPath { get; set; }

        public string TxtTestFormPath { get; set; }

        public string MidCerificationResultsPath { get; set; }

        public int DisciplineId { get; set; }

        public int ProjectId { get; set; }

        public List<Competence> CheckingCompetences { get; set; }
    }
}