using Microsoft.AspNetCore.Http;
using office_automation_system.application.Dto.RequestStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.RequestStepFile
{
    public class GetRequestStepFileDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? FileUrl { get; set; }
        public Guid? RequestStepId { get; set; }
        public GetRequestStepDto? RequestStep { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
