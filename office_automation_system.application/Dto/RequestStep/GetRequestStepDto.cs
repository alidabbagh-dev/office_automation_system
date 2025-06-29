using office_automation_system.application.Dto.ApplicationUser;
using office_automation_system.application.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.RequestStep
{
    public class GetRequestStepDto
    {
        public Guid Id { get; set; }
        public Guid? RequestId { get; set; }
        public GetRequestDto? Request { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? RoleId { get; set; }
        public GetApplicationUserDto? Owner { get; set; }
        public int? Order { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCurrentStep { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
