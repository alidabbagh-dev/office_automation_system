using office_automation_system.application.Dto.ApplicationUser;
using office_automation_system.application.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.RequestStep
{
    public class EditRequestStepDto
    {
        
       
        public string? Description { get; set; }
        public bool? IsCurrentStep { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
