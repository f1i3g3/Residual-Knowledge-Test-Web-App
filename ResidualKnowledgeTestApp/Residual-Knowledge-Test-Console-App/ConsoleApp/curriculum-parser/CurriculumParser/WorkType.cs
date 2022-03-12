using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Тип работы учащегося
    /// </summary>
    /// <remarks>самостоятельная, в присутсвии преподавателя...</remarks>
    public class WorkType
    {
        /// <summary>
        /// Название типа работы
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Подтипы
        /// </summary>
        public List<WorkType> SubTypes { get; private set; }

        /// <summary>
        /// Создает экзкмпляр класса <name>Programme</name>
        /// </summary>
        /// <param name="name">Название типа работы</param>
        /// <param name="subTypes">Подтипы</param>
        public WorkType(string name, List<WorkType> subTypes)
        {
            Name = name;
            SubTypes = subTypes;
        }
    }
}
