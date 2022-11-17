using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResidualKnowledgeApp.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UploadController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ILogger<UploadController> _logger;	
		public UploadController(IWebHostEnvironment environment, ILogger<UploadController> logger)
		{
			_environment = environment;
			_logger = logger;
		}

		/// <summary>
		/// Загрузка файла
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task Post()
		{
			if (HttpContext.Request.Form.Files.Any())
			{
				var file = HttpContext.Request.Form.Files.FirstOrDefault();
				var filePath = file.Name.Replace(" ", "\\");

				var directory = Path.Combine(_environment.ContentRootPath, filePath);
				_logger.LogInformation($"Adding file {file.FileName} to directory {directory}...");
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				var path = Path.Combine(directory, file.FileName);
				using var stream = new FileStream(path, FileMode.Create);
				await file.CopyToAsync(stream);

				_logger.LogInformation("Success!");
			}
		}
	}
}
