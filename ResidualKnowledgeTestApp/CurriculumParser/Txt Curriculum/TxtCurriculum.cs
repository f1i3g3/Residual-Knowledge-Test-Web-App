using System;
using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит информацию об учебном плане в формате txt
    /// </summary>
    public class TxtCurriculum : ICurriculumWithElectiveBlocks
    {
        /// <summary>
        /// Код учебного плана
        /// </summary>
        public string CurriculumCode { get; private set; }

        /// <summary>
        /// Программа, которую реализует данный учебный план
        /// </summary>
        public Programme Programme { get; private set; }

        /// <summary>
        /// Дисциплины
        /// </summary>
        public List<Discipline> Disciplines { get; private set; }

        /// <summary>
        /// Элективные блоки
        /// </summary>
        public List<ElectivesBlock> ElectiveBlocks { get; private set; }

        /// <summary>
        /// Создает экземпляр класса <name>TxtCurriculum</name>
        /// </summary>
        /// <param name="fileName">Файл на основе которого создается учебный план</param>
        public TxtCurriculum(string fileName)
        {
            ElectiveBlocks = new List<ElectivesBlock>();
            try
            {
                var txtCurriculumParser = new TxtCurriculumParser(fileName);
                (CurriculumCode, Programme, Disciplines, ElectiveBlocks) = txtCurriculumParser.Parse();
            }
            catch (Exception e)
            {
                throw new CurriculumParsingException("Ошибка парсинга учебного плана в .txt", e);
            }
        }
    }
}
