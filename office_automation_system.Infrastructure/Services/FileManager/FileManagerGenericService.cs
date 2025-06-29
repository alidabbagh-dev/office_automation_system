using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using office_automation_system.application.Contracts.Services.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Services.FileManager
{
    public class FileManagerGenericService : IFileManagerGenericService
    {
        private readonly IHostEnvironment _hostEnvironment;

        public FileManagerGenericService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string path)
        {
            if (file == null || file?.Length == 0)
                throw new ArgumentException("Invalid file");

            // Create the full path
            var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, path);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file?.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path (or full path if needed)
            return Path.Combine(path, uniqueFileName).Replace("\\", "/");
        }
    }
}
