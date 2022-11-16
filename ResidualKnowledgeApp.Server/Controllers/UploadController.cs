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
		private readonly IWebHostEnvironment environment;
		public UploadController(IWebHostEnvironment environment)
		{
			this.environment = environment;
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

				var directory = Path.Combine(environment.ContentRootPath, file?.Name);
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				var path = Path.Combine(directory, file.FileName);
				using var stream = new FileStream(path, FileMode.Create);
				await file.CopyToAsync(stream);
			}
		}
	}
}
