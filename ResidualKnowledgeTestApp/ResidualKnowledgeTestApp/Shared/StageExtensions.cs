namespace ResidualKnowledgeTestApp.Shared
{
    public static class StageExtensions
    {
        public static string ToStringRepresentation(this Stage s)
        {
            return s switch
            {
                Stage.DisciplinesChoosing => "Выбор дисциплин",
                Stage.CompetencesChoosing => "Выбор компетенций",
                Stage.FilesUploading => "Загрузка файлов",
                _ => "",
            };
        }
    }
}
