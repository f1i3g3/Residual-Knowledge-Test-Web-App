using System.Linq;
using ResidualKnowledgeTestApp.Shared.DTO;

namespace ResidualKnowledgeTestApp.Client.Services
{
    public class UserChoice
    {
        private bool disciplinesChanged = false;
        private bool competencesShouldBeUpdated = false;
        private bool filesShouldBeUpdated = false;
        private bool sheetLinkChanged = false;

        public UserChoice()
        {
        }

        public UserChoice(ProjectDetailsDTO project)
        {
            CurriculumSelected = project.Curriculum != null;
            DisciplinesSelected = project.CheckingDisciplines != null && project.CheckingDisciplines.Count > 0;
            CompetencesSelected = project.CheckingDisciplines.SelectMany(cd => cd.CheckingCompetences).Any();
            SheetLinkGenerated = project.SheetLink != null;

            FilesUploaded = false;

            DisciplinesChanged = disciplinesChanged;
            CompetencesShouldBeUpdated = competencesShouldBeUpdated;
            FilesShouldBeUpdated = filesShouldBeUpdated;
        }

        public bool CurriculumSelected { get; set; } = false;

        public bool DisciplinesSelected { get; set; } = false;

        public bool CompetencesSelected { get; set; } = false;

        public bool FilesUploaded { get; set; } = false;

        public bool SheetLinkGenerated { get; set; } = false;

        public bool DisciplinesChanged
        {
            get => disciplinesChanged;
            set
            {
                if (value)
                {
                    CompetencesShouldBeUpdated = true;
                    FilesShouldBeUpdated = true;
                }
                disciplinesChanged = value;
            }
        }

        public bool CompetencesShouldBeUpdated
        {
            get => competencesShouldBeUpdated;
            set
            {
                if (!value && !FilesShouldBeUpdated)
                {
                    DisciplinesChanged = false;
                }
                competencesShouldBeUpdated = value;
            }
        }

        public bool FilesShouldBeUpdated
        {
            get => filesShouldBeUpdated;
            set
            {
                if (!value && !CompetencesShouldBeUpdated)
                {
                    DisciplinesChanged = false;
                }
                filesShouldBeUpdated = value;
            }
        }

        public bool SheetLinkChanged
        {
            get => sheetLinkChanged;
            set
            {
                if (value)
                {
                    SheetLinkChanged = true;
                }
                sheetLinkChanged = value;
            }
        }
    }
}
