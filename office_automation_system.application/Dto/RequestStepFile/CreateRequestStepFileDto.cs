using Microsoft.AspNetCore.Http;
using office_automation_system.application.Dto.RequestStep;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.RequestStepFile
{
    public class CreateRequestStepFileDto
    {
        public string? Title { get; set; }
        public string? FileUrl { get; set; }//this field is not sending from user we assign value to fileurl in service
        
        public IFormFile? File { get; set; }
        public Guid? RequestStepId { get; set; }
        public CreateRequestStepDto? RequestStep { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
    }
}
