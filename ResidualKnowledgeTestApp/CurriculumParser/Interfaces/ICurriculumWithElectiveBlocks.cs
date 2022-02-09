using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Интерфейс, которые должны реализовывать учебные планы, 
    /// чтобы была возможность использовать их в генераторе приказов 
    /// на распределение студентов по дисциплинам по выбору
    /// </summary>
    public interface ICurriculumWithElectiveBlocks
    {
        /// <summary>
        /// Код учебного плана
        /// </summary>
        string CurriculumCode { get; }

        /// <summary>
        /// Программа, которую реализует данный учебный план
        /// </summary>
        Programme Programme { get; }

        /// <summary>
        /// Дисциплины
        /// </summary>
        List<Discipline> Disciplines { get; }

        /// <summary>
        /// Элективные блоки
        /// </summary>
        List<ElectivesBlock> ElectiveBlocks { get; }
    }
}