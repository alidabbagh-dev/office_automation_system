using Microsoft.AspNetCore.Identity;
using office_automation_system.domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.domain.Entities
{
    public class RequestStep : BaseEntity
    {
        
        public Guid? RequestId { get; set; }
        public Request? Request { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? RoleId { get; set; }
        public IdentityRole<Guid>? Role { get; set; }
        public ApplicationUser? Owner { get; set; }
        public int? Order {  get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCurrentStep { get; set; }
        
    }
}
