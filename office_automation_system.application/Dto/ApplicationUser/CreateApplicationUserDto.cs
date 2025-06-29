using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.ApplicationUser
{
    public class CreateApplicationUserDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Guid? RoleId { get; set; } // Only one role
        public string? UserCode { get; set; }
    }
}
