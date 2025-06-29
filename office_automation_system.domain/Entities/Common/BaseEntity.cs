using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.domain.Entities.Common
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool? IsDeleted { get; set; } = false; 
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
