using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит информацию о программе обучения
    /// </summary>
    public class Programme
    {
        /// <summary>
        /// Создает экзкмпляр класса <name>Programme</name>
        /// </summary>
        /// <param name="code">Код программы обучения</param>
        /// <param name="russianName">Русское наименование программы</param>
        /// <param name="englishName">Английское наименование программы</param>
        /// <param name="levelOfEducation">Уровень обучения(бакалавриат/магистратура...)</param>
        /// <param name="specializations">Специализации, которые реализует данная учебная программа</param>
        public Programme(string code, string russianName,
            string englishName, string levelOfEducation,
            List<Specialization> specializations)
        {
            Code = code;
            RussianName = russianName;
            EnglishName = englishName;
            LevelOfEducation = levelOfEducation;
            Specializations = specializations;
        }

        /// <summary>
        /// Код программы обучения
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Русское наименование программы
        /// </summary>
        public string RussianName { get; private set; }

        /// <summary>
        /// Английское наименование программы
        /// </summary>
        public string EnglishName { get; private set; }

        /// <summary>
        /// Уровень обучения(бакалавриат/магистратура...)
        /// </summary>
        public string LevelOfEducation { get; private set; }

        /// <summary>
        /// Специализации, которые реализует данная учебная программа
        /// </summary>
        public List<Specialization> Specializations { get; set; }
    }
}
