using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUploadFiles.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase {

        private readonly IHostEnvironment _environment;

        public ImagesController(IHostEnvironment environment) {
            _environment = environment;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] IFormFile image) {
            if (image == null || image.Length == 0) {
                return BadRequest("Upload a file");

            } else {
                string fileName = image.FileName;
                string extension = Path.GetExtension(fileName);

                string[] allowedExtensions = { ".jpg" , ".png" , ".bmp" };

                if (!allowedExtensions.Contains(extension)) {
                    return BadRequest("File is not a valid image");

                } else { //Aquí se crea el archivo de imagen en nuestro wwwroot
                    string newFileName = $"{Guid.NewGuid()}{extension}";
                    string filePath = Path.Combine(_environment.ContentRootPath , "wwwroot" , "Images" , newFileName);

                    using(var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
                        await image.CopyToAsync(fileStream);
                    }

                    return Ok($"Images/{newFileName}");

                }
            }
        }

    }
}
