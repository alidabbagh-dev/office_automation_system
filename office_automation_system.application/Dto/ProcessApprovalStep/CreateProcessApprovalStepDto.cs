using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.application.Dto.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.ProcessApprovalStep
{
    public class CreateProcessApprovalStepDto
    {
        public string? StepTitle { get; set; }
        public Guid? OwnerId { get; set; }
        public CreateApplicationUserDto? Owner { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? AdministrativeProcessId { get; set; }
        public CreateAdministrativeProcessDto? AdministrativeProcess { get; set; }
        public int? Order { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
