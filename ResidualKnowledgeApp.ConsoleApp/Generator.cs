global using ResidualKnowledgeApp.ContingentParser;
global using ResidualKnowledgeApp.CurriculumParser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResidualKnowledgeApp.ConsoleApp.InputFilesParser;


namespace ResidualKnowledgeApp.ConsoleApp
{
	public class Generator
	{
		public static string Generate(DocxCurriculum curriculum, Contingent contingent, List<CheckingDiscipline> checkingDisciplines, List<MarkCriterion> competenceCriterions = null, List<StudentAnswer> studentAnswers = null, List<MidCerificationAssesmentResult> midCertificationResults = null, User user = null)
		{
			try
			{
				var groups = contingent
				.Where(s => s.CurriculumCode == curriculum.CurriculumCode.Replace("/", "\\")) 
				.Select(s => s.GroupInContingent)
				.Distinct()
				.ToList();

				var disciplines = curriculum.Disciplines.Select(d => d.Implementations[0].Discipline).ToList();

				var discipline = disciplines.First(d => d.Code == "009503");
				var competences = discipline.Implementations[0].Competences.Take(1).ToList();

				var listOfMarks = new List<MarkCriterion>(); // listOfMarkCreations from server?

				user = new User("Кузнецов", "Дмитрий", "Владимирович"); // TODO: Authorization
				var config = new MsFormsParserConfiguration(4, 8, 3, user); // Server

				var userChoice = new UserChoice(user, curriculum, contingent, checkingDisciplines);
				competenceCriterions = new List<MarkCriterion>
				{
					new MarkCriterion(90, 100, 'A', 5),
					new MarkCriterion(80, 89, 'B', 4),
					new MarkCriterion(70, 79, 'C', 4),
					new MarkCriterion(60, 69, 'D', 3),
					new MarkCriterion(50, 59, 'E', 3),
					new MarkCriterion(0, 49, 'F', 2)
				}; // Server

				midCertificationResults = new List<MidCerificationAssesmentResult>();
				studentAnswers = new List<StudentAnswer>();
				foreach (var d in checkingDisciplines)
				{
					var parser = new ResidualKnowledgeDataParser(d, userChoice.Students);
					var result = parser.Parse();
					midCertificationResults.AddRange(result.MidCerificationResults);
					studentAnswers.AddRange(result.StudentAnswers);
					d.Questions.AddRange(result.Questions);
				}

				/*
				var mcrParser = new MidCertificationResultsParser(d); // нужен ли?
				mcrParser.TryParseMidCertificationResults(out midCertificationResult);  // парсер отдельного файла?
				*/

				var spreadsheetGenerator = new GoogleSpreadsheetGenerator(userChoice, groups, competenceCriterions, studentAnswers, midCertificationResults);
				spreadsheetGenerator.Generate(); // exception point

				var spreadsheet = new GoogleSpreadsheetParser.GoogleSpreadsheetParser(userChoice.CheckingDisciplines);
				var fileGenerators = new List<IFileGenerator>
				{
				  new SheetGenerator(spreadsheet, userChoice),
				  new ProtocolGenerator(spreadsheet, userChoice),
				  new ResidualKnowledgeFundGenrator(userChoice)
				}; // TODO: return files

				Parallel.ForEach(fileGenerators, g => g.Generate());

				return spreadsheetGenerator.spreadsheetId;
			}
			catch
			{
				return null;
			}
		}
	
		internal static void Sample()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory + "/../../../user_files";

			string[] args = new string[]
			{
				Path.Combine(path, "curriculum.docx"),
				Path.Combine(path, "contingent.xlsx"),
				Path.Combine(path, "msformsAlg.xlsx"),
				Path.Combine(path, "msformsCompArc.xlsx"),
				Path.Combine(path, "alg.txt"),
				Path.Combine(path, "compArc.txt"),
			};

			// TODO: Update parser to iText 7
			var curriculum = new DocxCurriculum(args[0]);
			var contingent = new Contingent(args[1]);
			var MsFormsAlgebra = args[2];
			var msFormsCompArchitecture = args[3];
			var QandAalg = args[4];
			var QandAcompArc = args[5];
			var midCerificationResultsAlg = ""; // args[6];
			var midCerificationResultsCompArc = ""; // args[7];

			Console.WriteLine("Files opended!");

			var groups = contingent
				.Where(s => s.CurriculumCode == curriculum.CurriculumCode.Replace("/", "\\"))
				.Select(s => s.GroupInContingent)
				.Distinct()
				.ToList();

			var disciplines = curriculum.Disciplines.Take(2).Select(d => d.Implementations[0].Discipline).ToList();

			var algebra = disciplines.First(d => d.Code == "009503");
			var algebraCompetences = algebra.Implementations[0].Competences.Take(1).ToList();
			var algebraScale = new List<MarkCriterion>
			{
				new MarkCriterion(12, 15, 'A', 5),
				new MarkCriterion(10, 11, 'B', 4),
				new MarkCriterion(8, 9, 'C', 4),
				new MarkCriterion(6, 7, 'D', 3),
				new MarkCriterion(5, 5, 'E', 3),
				new MarkCriterion(0, 4, 'F', 2)
			};

			var compArchitecture = disciplines.First(d => d.Code != "009503");
			var compArchitectureCompetences = compArchitecture.Implementations[0].Competences.Skip(1).Take(1).ToList();
			var compArchitectureScale = new List<MarkCriterion>
			{
				new MarkCriterion(12, 14, 'A', 5),
				new MarkCriterion(10, 11, 'B', 4),
				new MarkCriterion(8, 9, 'C', 4),
				new MarkCriterion(6, 7, 'D', 3),
				new MarkCriterion(5, 5, 'E', 3),
				new MarkCriterion(0, 4, 'F', 2)
			};

			var user = new User("ФИО", "Автора", "Ответов");
			var config = new MsFormsParserConfiguration(4, 8, 3, user);

			var checkingDisciplines = new List<CheckingDiscipline>
			{
				new CheckingDiscipline(algebra, algebraCompetences, user, algebraScale, config: config, questionsCount: 7,
					msFormsPath: MsFormsAlgebra, txtTestFormPath: QandAalg, midCerificationResultsPath: midCerificationResultsAlg),

				new CheckingDiscipline(compArchitecture, compArchitectureCompetences, user, compArchitectureScale, config: config,
					questionsCount: 7, msFormsPath: msFormsCompArchitecture, txtTestFormPath: QandAcompArc,
					midCerificationResultsPath: midCerificationResultsCompArc)
			};

			var userChoice = new UserChoice(user, curriculum, contingent, checkingDisciplines);
			var competenceCriterion = new List<MarkCriterion>
			{
				new MarkCriterion(90, 100, 'A', 5),
				new MarkCriterion(80, 89, 'B', 4),
				new MarkCriterion(70, 79, 'C', 4),
				new MarkCriterion(60, 69, 'D', 3),
				new MarkCriterion(50, 59, 'E', 3),
				new MarkCriterion(0, 49, 'F', 2)
			};

			var midCertificationResult = new List<MidCerificationAssesmentResult>();
			var studentAnswers = new List<StudentAnswer>();
			foreach (var d in checkingDisciplines)
			{
				var parser = new ResidualKnowledgeDataParser(d, userChoice.Students);
				var result = parser.Parse();
				midCertificationResult.AddRange(result.MidCerificationResults);
				studentAnswers.AddRange(result.StudentAnswers);
				d.Questions.AddRange(result.Questions);
			}

			midCertificationResult.AddRange(new List<MidCerificationAssesmentResult>
			{
				new MidCerificationAssesmentResult("Анон 1", "Алгебра", "5"),
				new MidCerificationAssesmentResult("Анон 2", "Алгебра", "2"),
				new MidCerificationAssesmentResult("Анон 4", "Алгебра", "3"),
				new MidCerificationAssesmentResult("Анон 5", "Алгебра", "4"),
				new MidCerificationAssesmentResult("Анон 6", "Алгебра", "5"),
				new MidCerificationAssesmentResult("Анон 7", "Алгебра", "4"),
				new MidCerificationAssesmentResult("Анон 8", "Алгебра", "5"),
				new MidCerificationAssesmentResult("Анон 9", "Алгебра", "2"),
				new MidCerificationAssesmentResult("Анон 10", "Алгебра", "5"),
				new MidCerificationAssesmentResult("Анон 11", "Алгебра", "5"),
				new MidCerificationAssesmentResult("Анон 12", "Алгебра", "5"),

				new MidCerificationAssesmentResult("Анон 1", "Архитектура ЭВМ и операционные системы", "4"),
				new MidCerificationAssesmentResult("Анон 3", "Архитектура ЭВМ и операционные системы", "5"),
				new MidCerificationAssesmentResult("Анон 2", "Архитектура ЭВМ и операционные системы", "4"),
				new MidCerificationAssesmentResult("Анон 4", "Архитектура ЭВМ и операционные системы", "5"),
				new MidCerificationAssesmentResult("Анон 5", "Архитектура ЭВМ и операционные системы", "4"),
				new MidCerificationAssesmentResult("Анон 6", "Архитектура ЭВМ и операционные системы", "3"),
				new MidCerificationAssesmentResult("Анон 7", "Архитектура ЭВМ и операционные системы", "4"),
				new MidCerificationAssesmentResult("Анон 8", "Архитектура ЭВМ и операционные системы", "5"),
				new MidCerificationAssesmentResult("Анон 9", "Архитектура ЭВМ и операционные системы", "3"),
				new MidCerificationAssesmentResult("Анон 10", "Архитектура ЭВМ и операционные системы", "4"),
				new MidCerificationAssesmentResult("Анон 12", "Архитектура ЭВМ и операционные системы", "5")
			});

			var spreadsheetGenerator = new GoogleSpreadsheetGenerator(userChoice, groups, competenceCriterion, studentAnswers, midCertificationResult);
			spreadsheetGenerator.Generate();

			// Console.WriteLine(spreadsheetGenerator.spreadsheetId);
			return;

			/*
            var spreadsheet = new GoogleSpreadsheetParser.GoogleSpreadsheetParser(userChoice.CheckingDisciplines);
            var fileGenerators = new List<IFileGenerator> 
            { 
                new SheetGenerator(spreadsheet, userChoice), 
                new ProtocolGenerator(spreadsheet, userChoice), 
                new ResidualKnowledgeFundGenrator(userChoice) 
            };

            Parallel.ForEach(fileGenerators, g => g.Generate());
            */
		}
	}
}
