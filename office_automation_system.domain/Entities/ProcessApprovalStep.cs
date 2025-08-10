using Microsoft.AspNetCore.Identity;
using office_automation_system.domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.domain.Entities
{
    public class ProcessApprovalStep : BaseEntity
    {
        public string? StepTitle { get; set; }
        public Guid? OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }
        public Guid? RoleId { get; set; }
        public IdentityRole<Guid>? Role { get; set; }
        public Guid? AdministrativeProcessId { get; set; }
        public AdministrativeProcess? AdministrativeProcess { get; set; }
        public int? Order {  get; set; }

    }
}
