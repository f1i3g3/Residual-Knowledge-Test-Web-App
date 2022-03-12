using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит информацию об электином блоке
    /// </summary>
    public class ElectivesBlock
    {
        /// <summary>
        /// Создает экзкмпляр класса <name>ElectivesBlock</name>
        /// </summary>
        /// <param name="semester">Семестр, в котором дисциплины блока доступны для выбора</param>
        /// <param name="number">Номер элективного блока, считая с 1 с начала семестра</param>
        /// <param name="specialization">Специализация,в которой реализуются дисциплины данного элективного блока</param>
        public ElectivesBlock(int semester, int number, Specialization specialization)
        {
            Semester = semester;
            Number = number;
            Specialization = specialization;
            Disciplines = new List<(Discipline, DisciplineImplementation)>();
        }

        /// <summary>
        /// Семестр, в котором дисциплины блока доступны для выбора
        /// </summary>
        public int Semester { get; private set; }

        /// <summary>
        /// Номер элективного блока, считая с 1 с начала семестра
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Специализация,в которой реализуются дисциплины данного элективного блока
        /// </summary>
        public Specialization Specialization { get; private set; }

        /// <summary>
        /// Дисциплина и ее реализация, содержащаяся в данном элективном блоке
        /// </summary>
        public List<(Discipline Discipline, DisciplineImplementation Implementation)> Disciplines { get; private set; }
    }
}
