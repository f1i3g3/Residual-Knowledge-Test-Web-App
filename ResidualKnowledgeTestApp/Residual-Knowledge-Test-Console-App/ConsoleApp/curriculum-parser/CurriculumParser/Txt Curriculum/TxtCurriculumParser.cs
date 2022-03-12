using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CurriculumParser
{
    /// <summary>
    /// Парсер учебного плана в формате txt
    /// </summary>
    class TxtCurriculumParser
    {
        private StreamReader reader;
        private readonly string fileName;

        /// <summary>
        /// Создает экземпляр класса <name>TxtCurriculumParser</name>
        /// </summary>
        /// <param name="fileName">Имя файла с расширением .txt, в котором содержится учебный план</param>
        public TxtCurriculumParser(string fileName)
        {
            this.fileName = fileName;
        }

        private DisciplineType ParseDisciplineType(string disciplineType)
        {
            return (disciplineType.Trim()) switch
            {
                "б" => DisciplineType.Base,
                "э" => DisciplineType.Elective,
                "ф" => DisciplineType.Facultative,
                _ => default,
            };
        }

        private string GetNextLine() => reader.ReadLine()?.Trim();

        /// <summary>
        /// Парсит информацию об учебного плане
        /// </summary>
        /// <returns>Код учебного плана, программу, дисциплины, элективные блоки</returns>
        public (string Code, Programme Programme, List<Discipline> Disciplines, List<ElectivesBlock> ElectiveBlocks) Parse()
        {
            var disciplines = new Dictionary<string, Discipline>();
            var electiveBlocks = new List<ElectivesBlock>();

            using (reader = new StreamReader(fileName, Encoding.UTF8))
            {
                var code = GetNextLine();
                var programmeCode = GetNextLine();
                var programmeRussianName = GetNextLine();
                var programmeEnglishName = GetNextLine();
                var levelOfEducation = GetNextLine();
                var programme = new Programme(programmeCode, programmeRussianName,
                    programmeEnglishName, levelOfEducation, new List<Specialization>());

                var specializationName = GetNextLine();
                var specializations = new HashSet<Specialization>();
                while (specializationName != "")
                {
                    specializations.Add(new Specialization("", specializationName, programme));
                    specializationName = GetNextLine();
                }

                while (!reader.EndOfStream)
                {
                    var line = GetNextLine();
                    while (!reader.EndOfStream && !string.IsNullOrEmpty(line.Trim()))
                    {
                        specializationName = line;
                        var specialization = specializations.SingleOrDefault(s => s.Name == specializationName);

                        line = GetNextLine();
                        var splitted = line.Split(' ', 2);
                        var semester = splitted[0];
                        var number = splitted[1];

                        var block = new ElectivesBlock(int.Parse(semester), int.Parse(number), specialization);
                        electiveBlocks.Add(block);

                        if (specialization != null)
                        {
                            programme.Specializations.Add(specialization);
                            specialization.ElectivesBlocks.Add(block);
                        }

                        line = GetNextLine();
                        while (!reader.EndOfStream && !string.IsNullOrEmpty(line))
                        {
                            var disciplineCode = line;
                            var disciplineRussianName = GetNextLine();
                            var disciplineType = GetNextLine();
                            Discipline discipline;
                            if (!disciplines.ContainsKey(disciplineCode))
                            {
                                discipline = new Discipline(disciplineCode, disciplineRussianName, "", ParseDisciplineType(disciplineType));
                                disciplines.Add(disciplineCode, discipline);
                            }
                            discipline = disciplines[disciplineCode];
                            discipline.ElectivesBlocks.Add(block);
                            block.Disciplines.Add((discipline, null));
                            line = GetNextLine();
                        }
                        line = GetNextLine();
                    }
                }
                return (code, programme, disciplines.Values.ToList(), electiveBlocks);
            }
        }
    }
}
