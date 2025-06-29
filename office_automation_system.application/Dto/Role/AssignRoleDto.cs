using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.Role
{
    public class AssignRoleDto
    {
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
    }


}
