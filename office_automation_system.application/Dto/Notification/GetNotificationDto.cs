using office_automation_system.application.Dto.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Dto.Notification
{
    public class GetNotificationDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public GetApplicationUserDto? User { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsSeen { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
