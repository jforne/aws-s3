using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POST_AOF_AWS_IMAGENES.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace POST_AOF_AWS_IMAGENES.Controllers
{
    public class PokemonController : Controller
    {
        private ServiceS3 service;

        public PokemonController(ServiceS3 service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<string> imagenes = await this.service.GetFilesAsync();
            return View(imagenes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile img)
        {
            List<string> imagenes = await this.service.GetFilesAsync();
        
            string fileName = img.FileName;
            using (Stream stream = img.OpenReadStream())
            {
                await this.service.UploadFileAsync(stream, fileName);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
