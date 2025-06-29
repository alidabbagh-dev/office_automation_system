using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.Services.FileManager
{
    public interface IFileManagerGenericService
    {
    
        Task<string> UploadFileAsync(IFormFile file, string path);
    }

}
