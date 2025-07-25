using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.RequestStepFile
{
    public class RequestStepFileFilterDto
    {
        public string? Title { get; set; }
        public string? FileUrl { get; set; }
        public Guid? RequestStepId { get; set; }
    }
}
