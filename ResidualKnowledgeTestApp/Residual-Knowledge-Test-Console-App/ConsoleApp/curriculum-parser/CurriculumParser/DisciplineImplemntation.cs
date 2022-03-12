using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит информацию о реализации дисциплины
    /// </summary>
    public class DisciplineImplementation
    {
        /// <summary>
        /// Создает экзкмпляр класса <name>DisciplineImplementation</name>
        /// </summary>
        /// <param name="discipline">Дисциплина, которой принадлежит реализация</param>
        /// <param name="semester">Семестр, в котором реализуется данная дисциплина</param>
        /// <param name="laborIntensity">Трудоемкость в зачетных единицах</param>
        /// <param name="realization">Тип проведения/реализации</param>
        /// <param name="trajectory">Траектория</param>
        /// <param name="monitoringTypes">Типы аттестации</param>
        /// <param name="workHours">Часы работы с разбиением по типу</param>
        /// <param name="competences">Формируемые компетенции</param>
        public DisciplineImplementation(Discipline discipline, int semester, int laborIntensity, 
            string realization, string trajectory, string monitoringTypes, 
            string workHours, List<Competence> competences)
        {
            Discipline = discipline;
            Semester = semester;
            LaborIntensity = laborIntensity;
            Realization = realization;
            Trajectory = trajectory;
            MonitoringTypes = monitoringTypes;
            WorkHours = workHours;
            Competences = competences;
        }

        /// <summary>
        /// Дисциплина, которой принадлежит данная реализация
        /// </summary>
        public Discipline Discipline { get; private set; }

        /// <summary>
        /// Семестр, которому принадлежит данная реализация дисциплины
        /// </summary>
        public int Semester { get; private set; }

        /// <summary>
        /// Трудоемкость в зачетных единицах
        /// </summary>
        public int LaborIntensity { get; private set; }

        /// <summary>
        /// Тип проведения/реализации
        /// </summary>
        public string Realization { get; private set; }

        /// <summary>
        /// Траектория
        /// </summary>
        public string Trajectory { get; private set; }

        /// <summary>
        /// Типы аттестации
        /// </summary>
        public string MonitoringTypes { get; private set; }

        /// <summary>
        /// Часы работы с разбиением по типу
        /// </summary>
        public string WorkHours { get; private set; }

        /// <summary>
        /// Формируемые компетенции
        /// </summary>
        public List<Competence> Competences { get; private set; }
    }
}
