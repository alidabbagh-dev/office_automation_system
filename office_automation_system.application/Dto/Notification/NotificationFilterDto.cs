using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.Notification
{
    public class NotificationFilterDto
    {
        public Guid? UserId { get; set; }
        public string? Title {  get; set; }

    }
}
