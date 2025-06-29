using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.RequestStep
{
    public class RequestStepFilterDto
    {
        public Guid? RequestId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? RoleId { get; set; }
        public string? Title { get; set; }
    }
}
