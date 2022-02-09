using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CurriculumParser
{
    /// <summary>
    /// Парсер информации о программе, которую реализует учебный плн
    /// </summary>
    static class DocxProgrammeParser
    {
        /// <summary>
        /// Парсит информацию о программе, которую реализует учебный план
        /// </summary>
        /// <param name="body">Тело документа</param>
        /// <returns>Объект реализующий сущность <name>Programme</name></returns>
        public static Programme Parse(Body body)
        {
            try
            {
                var (russianName, englishName) = ParseName(body);
                var code = ParseCode(body);
                var levelOfEducation = ParseLevelOdEducation(body);
                var specializations = new List<Specialization>();

                return new Programme(code, russianName, englishName,
                    levelOfEducation, specializations);
            }
            catch (Exception e)
            {
                throw new CurriculumParsingException("Ошибка парсинга информации о программе, которой принадлежит учебный план", e);
            }
        }

        private static string ParseCode(Body body)
        {
            var table = body.Elements<Table>().First();
            var row = table.Elements<TableRow>().ElementAt(1);
            var cell = row.Elements<TableCell>().ElementAt(1);
            var match = Regex.Match(cell.InnerText, @"\d{2}\.\d{2}\.\d{2}");

            return match.Success ? match.Value : null;
        }

        private static string ParseLevelOdEducation(Body body)
        {
            var table = body.Elements<Table>().First();
            var row = table.Elements<TableRow>().First();
            var cell = row.Elements<TableCell>().ElementAt(1);

            return cell.InnerText;
        }

        private static (string RussianName, string EnglishName) ParseName(Body body)
        {
            var englishName = "";
            var russianName = "";

            var firstTable = body.Elements<Table>().First();
            var paragraphs = firstTable.ElementsBefore().OfType<Paragraph>();
            var nameParagraph = paragraphs.Last();

            var runs = nameParagraph.Descendants<Run>();
            var seenBreak = false;
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
                        englishName += element.InnerText;
                        continue;
                    }
                    russianName += element.InnerText;
                }
            }

            return (russianName, englishName);
        }
    }
}
