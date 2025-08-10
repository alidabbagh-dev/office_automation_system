using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using office_automation_system.domain.Entities;
using office_automation_system.Infrastructure.Data.configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace office_automation_system.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<AdministrativeProcess> AdministrativeProcesses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProcessApprovalStep> ProcessApprovalSteps { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestStep> RequestSteps { get; set; }
        public DbSet<RequestStepFile> RequestStepFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AdministrativeConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());
            builder.ApplyConfiguration(new  ProcessApprovalStepConfiguration());
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new RequestConfiguration());
            builder.ApplyConfiguration(new RequestStepConfiguration());
            builder.ApplyConfiguration(new RequestStepFileConfiguration());
        }

    }


}
