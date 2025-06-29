using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.application.Dto.ApplicationUser;
using office_automation_system.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.Request
{
    public class CreateRequestDto
    {
        public Guid? ProcessId { get; set; }
        public CreateAdministrativeProcessDto? Process { get; set; }
        public Guid? UserId { get; set; }
        public CreateApplicationUserDto? User { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
