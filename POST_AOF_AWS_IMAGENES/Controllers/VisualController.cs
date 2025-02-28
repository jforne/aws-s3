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
    public class VisualController : Controller
    {
        private ServiceS3 service;

        public VisualController(ServiceS3 service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index(string path = "")
        {
            List<string> elementos = await this.service.GetFoldersAndFilesAsync(path);
            ViewData["CurrentPath"] = path;
            return View(elementos);
        }

        public IActionResult Create(string path = "")
        {
            ViewData["CurrentPath"] = path;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile img, string path = "")
        {
            if (img != null)
            {
                string fileName = img.FileName;
                using (Stream stream = img.OpenReadStream())
                {
                    await this.service.UploadFileAsync(stream, fileName, path);
                }
            }

            // Redirige a la página principal (Home.Index) después de subir la imagen
            return RedirectToAction("Index", "Home");
        }
    }
}
