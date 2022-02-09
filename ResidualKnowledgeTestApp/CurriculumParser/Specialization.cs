using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит информацию о специализации
    /// </summary>
    public class Specialization
    {
        /// <summary>
        /// Создает экзкмпляр класса <name>Specialization</name>
        /// </summary>
        /// <param name="number">Номер специализации в учебном плане (0, если отсутсвует)</param>
        /// <param name="name">Наименование специализации</param>
        /// <param name="programme">Программа, реализующая специализацию</param>
        public Specialization(string number, string name, Programme programme)
        {
            Number = number;
            Name = name;
            Programme = programme;
            ElectivesBlocks = new List<ElectivesBlock>();
        }

        /// <summary>
        /// Номер специализации в учебном плане (0, если отсутсвует)
        /// </summary>
        public string Number { get; private set; }

        /// <summary>
        /// Наименование специализации
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Программа, реализующая специализацию
        /// </summary>
        public Programme Programme { get; private set; }

        /// <summary>
        /// Элективные блоки, принадлежащие специализации
        /// </summary>
        public List<ElectivesBlock> ElectivesBlocks { get; private set; }
    }
}
