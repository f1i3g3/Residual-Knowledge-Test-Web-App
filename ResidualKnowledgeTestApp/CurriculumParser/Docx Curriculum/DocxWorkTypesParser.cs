using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurriculumParser
{
    /// <summary>
    /// Парсер типов работ
    /// </summary>
    class DocxWorkTypesParser
    {
        /// <summary>
        /// Парсер типов работ
        /// </summary>
        /// <param name="body">Тело документа</param>
        /// <returns>Типы работ</returns>
        public static List<WorkType> Parse(Body body)
        {
            try
            {
                return ParseWorkTypes(body);
            }
            catch (Exception e)
            {
                throw new CurriculumParsingException("Ошибка парсинга типа работы учащегося", e);
            }
        }

        private static List<WorkType> ParseWorkTypes(Body body)
        {
            var table = body.Elements<Table>().Skip(3).First();

            var subTypesCells = table.Elements<TableRow>().ElementAt(1).Elements<TableCell>();
            var subTypesNames = GetWorkTypeNames(subTypesCells);
            var subTypes = new WorkType[14];
            var auditoryWorkSubTypes = new List<WorkType>();
            var independentWorkSubTypes = new List<WorkType>();
            for (var i = 0; i < subTypes.Length; ++i)
            {
                subTypes[i] = new WorkType(subTypesNames.ElementAt(i), null);
                if (i < 9)
                {
                    auditoryWorkSubTypes.Add(subTypes[i]);
                    continue;
                }
                independentWorkSubTypes.Add(subTypes[i]);
            }

            var typesCells = table.Elements<TableRow>().ElementAt(0).Elements<TableCell>().Skip(5);
            var typesNames = GetWorkTypeNames(typesCells);
            var types = new WorkType[3];
            for (var i = 0; i < 3; ++i)
            {
                var internalTypes = i switch
                {
                    0 => auditoryWorkSubTypes,
                    1 => independentWorkSubTypes,
                    _ => null
                };
                types[i] = new WorkType(typesNames.ElementAt(i), internalTypes);
            }

            return types.ToList();
        }

        private static IEnumerable<string> GetWorkTypeNames(IEnumerable<TableCell> cells)
        {
            var names = new List<string>();
            foreach (var cell in cells)
            {
                var name = cell.InnerText;
                if (!string.IsNullOrEmpty(name))
                {
                    names.Add(name);
                }
            }
            return names;
        }
    }
}