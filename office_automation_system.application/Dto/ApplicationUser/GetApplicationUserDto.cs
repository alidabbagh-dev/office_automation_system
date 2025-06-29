using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.ApplicationUser
{
    public class GetApplicationUserDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public Guid? RoleId { get; set; }
        public bool? IsLocked { get; set; }
        public string? UserCode { get; set; }
    }
}
