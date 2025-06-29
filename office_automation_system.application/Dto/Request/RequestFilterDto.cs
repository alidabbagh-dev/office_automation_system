using office_automation_system.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.Request
{
    public class RequestFilterDto
    {
        public Guid? ProcessId { get; set; }
        public Guid? UserId { get; set; }
        public RequestStatus? Status { get; set; }
    }
}
