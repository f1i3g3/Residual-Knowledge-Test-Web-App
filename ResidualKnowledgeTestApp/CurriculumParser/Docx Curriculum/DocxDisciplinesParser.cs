using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CurriculumParser
{
    /// <summary>
    /// Парсер информации о дисциплинах
    /// </summary>
    class DocxDisciplinesParser
    {
        private readonly Body body;
        private readonly Programme programme;
        private readonly List<Competence> competences;
        private readonly List<ElectivesBlock> electivesBlocks;
        private readonly Dictionary<string, Discipline> disciplines;

        /// <summary>
        /// Созадет экземляр класса <name>DocxDisciplinesParser</name>
        /// </summary>
        /// <param name="body">Тело документа</param>
        /// <param name="competences">Компетенции</param>
        /// <param name="programme">Программа</param>
        /// <param name="electivesBlocks">Элективные блоки</param>
        public DocxDisciplinesParser(Body body, List<Competence> competences, Programme programme, List<ElectivesBlock> electivesBlocks)
        {
            this.body = body;
            this.programme = programme;
            this.competences = competences;
            this.electivesBlocks = electivesBlocks;
            disciplines = new Dictionary<string, Discipline>();
        }

        /// <summary>
        /// Парсер дисциплин
        /// </summary>
        /// <returns></returns>
        public List<Discipline> Parse()
        {
            try
            {
                return ParseDisciplines();
            }
            catch (Exception e)
            {
                throw new CurriculumParsingException("Ошибка парсинга дисциплин .docx", e);
            }
        }

        private List<Discipline> ParseDisciplines()
        {
            DisciplineType type = 0;
            var electivesBlockNumber = 1;
            var semester = 1;
            ElectivesBlock electivesBlock = null;

            var prevIntensity = 0;
            var prevMonitoringType = "";
            List<Competence> prevCompetences = null;

            Specialization specialization = null;

            var table = body.Elements<Table>().Skip(3).First();
            var rows = table.Elements<TableRow>().Skip(2);
            foreach (var row in rows)
            {
                if (row.Descendants<TableCell>().Count() == 1)
                {
                    electivesBlockNumber = 1;
                    specialization = null;

                    var text = row.InnerText;
                    if (TryParseDisciplineType(text, out DisciplineType newType))
                    {
                        type = newType;
                    }
                    else if (text.Contains("Семестр"))
                    {
                        semester = ParseSemesterNumber(text);
                    }
                    else if (type == DisciplineType.Elective && !text.Contains("Не предусмотрено") && !text.Contains("год"))
                    {
                        specialization = GetSpecialization(text);
                    }
                    continue;
                }

                var nameCell = row.Elements<TableCell>().ElementAt(3);
                var (regNumber, rusName, engName, realization, trajectory) = ParseDisciplineNameCodeRealization(nameCell);
                if (regNumber == null) continue; // для чистой математики

                if (!disciplines.ContainsKey(regNumber))
                {
                    var discipline = new Discipline(regNumber, rusName, engName, type);
                    disciplines.Add(regNumber, discipline);
                }

                var competencesCell = row.Elements<TableCell>().ElementAt(2);
                if (!string.IsNullOrEmpty(competencesCell?.InnerText))
                {
                    if (type == DisciplineType.Elective)
                    {
                        electivesBlock = new ElectivesBlock(semester, electivesBlockNumber, specialization);
                        electivesBlocks.Add(electivesBlock);
                        ++electivesBlockNumber;
                        if (specialization != null)
                        {
                            specialization.ElectivesBlocks.Add(electivesBlock);
                        }
                    }
                    prevCompetences = ParseDisciplineCompetences(competencesCell);
                }

                var intensityCell = row.Elements<TableCell>().ElementAt(1);
                if (!string.IsNullOrEmpty(intensityCell?.InnerText))
                {
                    prevIntensity = int.Parse(intensityCell.InnerText);
                }

                var monitoringTypeCell = row.Elements<TableCell>().ElementAt(4);
                if (!string.IsNullOrEmpty(monitoringTypeCell?.InnerText))
                {
                    prevMonitoringType = monitoringTypeCell.InnerText;
                }

                var workHours = ParseDisciplineWorkHours(row);

                var implementation = new DisciplineImplementation(disciplines[regNumber], semester, prevIntensity,
                    realization, trajectory, prevMonitoringType, workHours, prevCompetences);
                disciplines[regNumber].Implementations.Add(implementation);

                if (type == DisciplineType.Elective)
                {
                    electivesBlock.Disciplines.Add((disciplines[regNumber], implementation));
                    disciplines[regNumber].ElectivesBlocks.Add(electivesBlock);
                }
            }

            return disciplines.Select(d => d.Value).ToList();
        }

        private int ParseSemesterNumber(string text)
            => int.Parse(text.Trim()[text.Length - 1].ToString());

        private bool TryParseDisciplineType(string text, out DisciplineType type)
        {
            switch (text.Trim())
            {
                case "Базовая часть периода обучения":
                    type = DisciplineType.Base;
                    return true;
                case "Вариативная часть периода обучения":
                    type = DisciplineType.Elective;
                    return true;
                case "Факультативные занятия":
                    type = DisciplineType.Facultative;
                    return true;
            }
            type = default;
            return false;
        }

        private Specialization GetSpecialization(string numberAndNameOfElective)
        {
            var regexMatch = Regex.Match(numberAndNameOfElective, @"(\d+[.]?\d*[.]?)(.+)");
            if (!regexMatch.Success)
            {
                return null;
            }
            var number = regexMatch.Groups[1].Value;
            var name = regexMatch.Groups[2].Value.Trim();

            var specializations = programme.Specializations;
            if (specializations.Find(s => s.Name == name) == null)
            {
                specializations.Add(new Specialization(number, name, programme));
            }
            return specializations.Where(s => s.Name == name).First();
        }

        private (string RegNumber, string Name, string EngName, string RealizationType, string Trajectory)
            ParseDisciplineNameCodeRealization(TableCell cell)
        {
            var name = "";
            var engName = "";
            var seenBreak = false;
            var runs = cell.Descendants<Run>();
            foreach (var run in runs)
            {
                foreach (var element in run.ChildElements)
                {
                    if (element.LocalName == "br")
                    {
                        seenBreak = true;
                        continue;
                    }
                    if (seenBreak)
                    {
                        engName += element.InnerText;
                        continue;
                    }
                    name += element.InnerText;
                }
            }

            var regexMatch = Regex.Match(name, @"\[(\d+)\]\s+([^(]+)\(?([^)]+)?\)?,?\s?(.+)?");
            if (regexMatch.Success)
            {
                var code = regexMatch.Groups[1].Value;
                var rusName = regexMatch.Groups[2].Value.Trim();
                var realizationType = "" == regexMatch.Groups[3].Value ? null : regexMatch.Groups[3].Value.Trim();
                var trajectory = "" == regexMatch.Groups[4].Value ? null : regexMatch.Groups[4].Value.Trim();
                var match = Regex.Match(engName, @"[^(]+");
                if (match.Success)
                {
                    engName = match.Value.Trim();
                }
                return (code, rusName, engName, realizationType, trajectory);
            }
            return default;
        }

        private List<Competence> ParseDisciplineCompetences(TableCell cell)
        {
            var codes = cell.InnerText.Replace(" ", "").Split(',');
            return competences.Where(c => codes.Contains(c.Code)).ToList();
        }

        private string ParseDisciplineWorkHours(TableRow row)
        {
            var workHours = "";
            var cells = row.Descendants<TableCell>().Skip(5).ToList();
            for (var i = 0; i < cells.Count(); ++i)
            {
                workHours += cells[i].InnerText + ' ';
            }
            return workHours.Trim();
        }
    }
}