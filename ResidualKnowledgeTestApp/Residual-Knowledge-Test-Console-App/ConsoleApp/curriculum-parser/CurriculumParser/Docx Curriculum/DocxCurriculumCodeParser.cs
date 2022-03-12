using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит методы парсинга кода учебного плана
    /// </summary>
    static class DocxCurriculumCodeParser
    {
        /// <summary>
        /// Парсит код учебного плана
        /// </summary>
        /// <param name="body">Тело документа</param>
        /// <returns>Код учебного плана</returns>
        public static string Parse(Body body)
        {
            try 
            {
                var table = body.Elements<Table>().ElementAt(1);
                var row = table.Elements<TableRow>().Last();
                var match = Regex.Match(row.InnerText.Trim(), @"\d{2}/\d{4}/\d{1}");
                return match.Success ? match.Value : null;
            }
            catch (Exception e)
            {
                throw new CurriculumParsingException("Ошибка парсинга кода учебного плана .docx" , e);
            }
        }
    }
}
