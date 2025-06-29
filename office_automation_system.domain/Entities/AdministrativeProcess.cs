using office_automation_system.domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.domain.Entities
{
    public class AdministrativeProcess : BaseEntity
    {
        
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
