using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит информацию о дисциплине
    /// </summary>
    public class Discipline
    {
        /// <summary>
        /// Создает экземляр класса <name>Discipline</name>
        /// </summary>
        /// <param name="code">Уникальный код дисциплины</param>
        /// <param name="russianName">Русское наименование дисциплины</param>
        /// <param name="englishName">Английское наименование дисциплины</param>
        /// <param name="type">Тип дисциплины</param>
        public Discipline(string code, string russianName, string englishName, DisciplineType type)
        {
            Code = code;
            RussianName = russianName;
            EnglishName = englishName;
            Type = type;

            ElectivesBlocks = new List<ElectivesBlock>();
            Implementations = new List<DisciplineImplementation>();
        }

        /// <summary>
        /// Уникальный код дисциплины
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Русское наименование дисциплины
        /// </summary>
        public string RussianName { get; private set; }

        /// <summary>
        /// Английское наименование дисциплины
        /// </summary>
        public string EnglishName { get; private set; }

        /// <summary>
        /// Тип дисциплины(базовая, элективная, факультативная)
        /// </summary>
        public DisciplineType Type { get; private set; }

        /// <summary>
        /// Элективные блоки, в которых реализуется данная дисциплина
        /// </summary>
        public List<ElectivesBlock> ElectivesBlocks { get; private set; }

        /// <summary>
        /// Все реализации дисциплины
        /// </summary>
        public List<DisciplineImplementation> Implementations { get; private set; }
    }
}