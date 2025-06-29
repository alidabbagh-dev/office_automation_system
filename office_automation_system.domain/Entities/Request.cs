using office_automation_system.domain.Entities.Common;
using office_automation_system.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.domain.Entities
{
    public class Request : BaseEntity
    {
        public Guid? ProcessId { get; set; }
        public AdministrativeProcess? Process { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public RequestStatus? Status { get; set; }
    }
}
