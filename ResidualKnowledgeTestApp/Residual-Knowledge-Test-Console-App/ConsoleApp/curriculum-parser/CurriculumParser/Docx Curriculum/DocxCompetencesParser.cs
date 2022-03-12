using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит методы парсинга компетенций, формируемых всеми дисциплинами учебного плана
    /// </summary>
    static class DocxCompetencesParser
    {
        /// <summary>
        /// Парсит информацию о формируемых у студента компетенциях
        /// </summary>
        /// <param name="body">Тело документа</param>
        /// <returns>Список компетенций</returns>
        public static List<Competence> Parse(Body body)
        {
            try
            {
                var competences = new List<Competence>();
                var table = body.Elements<Table>().Skip(2).First();                
                var (codeCellIndex, descriptionCellIndex) = AnalyzeCompetenceTableHeader(table.Elements<TableRow>().ElementAt(0));                
                foreach (var row in table.Elements<TableRow>().Skip(1))
                {
                    var code = row.Elements<TableCell>().ElementAt(codeCellIndex).InnerText;
                    var descriptionRuns = row.Elements<TableCell>().ElementAt(descriptionCellIndex).Descendants<Run>();
                    var description = "";
                    foreach (var run in descriptionRuns)
                    {
                        foreach (var element in run.ChildElements)
                        {
                            if (element is PageBreakBefore || element is LastRenderedPageBreak || element is Break)
                            {
                                description += " ";
                                continue;
                            }
                            description += element.InnerText;
                        }
                    }
                    description = description.Replace("  ", " ");
                    competences.Add(new Competence(code, description));
                }
                return competences;
            }
            catch (Exception e)
            {
                throw new CurriculumParsingException("Ошибка парсинга компетенций .docx", e);
            }
        }

        private static (int codeCellIndex, int descriptionCellIndex) AnalyzeCompetenceTableHeader(TableRow row)
        {
            var codeCell = row.Elements<TableCell>().Single(c => c.InnerText.ToLower().Contains("код"));
            var descriptionCell = row.Elements<TableCell>().Single(c => c.InnerText.ToLower().Contains("описание компетенции") 
                || c.InnerText.ToLower().Contains("наименование компетенции"));
            
            var cells = row.Elements<TableCell>().ToList().ToList();
            var codeCellIndex = cells.IndexOf(codeCell);
            var descriptionCellIndex = cells.IndexOf(descriptionCell);

            return (codeCellIndex, descriptionCellIndex);
        }
    }
}
