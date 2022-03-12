namespace CurriculumParser
{
    /// <summary>
    /// Содержит информацию о компетенции
    /// </summary>
    public class Competence
    {
        /// <summary>
        /// Создает экземпляр класса <name>Competence</name>
        /// </summary>
        /// <param name="code">Код компетенции</param>
        /// <param name="description">Описание компетенции</param>
        public Competence(string code, string description)
        {
            Code = code;
            Description = description;
        }

        /// <summary>
        /// Код компетенции
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Описание компетенции
        /// </summary>
        public string Description { get; private set; }
    }
}