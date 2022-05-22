using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POST_AOF_AWS_IMAGENES.Models;
using POST_AOF_AWS_IMAGENES.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace POST_AOF_AWS_IMAGENES.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ServiceS3 service;

        public HomeController(ILogger<HomeController> logger, ServiceS3 service)
        {
            _logger = logger;
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<string> imagenes = await this.service.GetFilesAsync();
            foreach(string imagen in imagenes)
            {

            }
            return View(imagenes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
