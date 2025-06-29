using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using office_automation_system.domain.Entities;
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

            // A process can have many approval steps (1:N)
            // Each approval step belongs to one AdministrativeProcess
            builder.Entity<AdministrativeProcess>()
                .HasMany<ProcessApprovalStep>()
                .WithOne(p => p.AdministrativeProcess) // navigation to parent
                .HasForeignKey(p => p.AdministrativeProcessId)
                .OnDelete(DeleteBehavior.Restrict); // do not delete steps when process is deleted

            // A process can have many requests (1:N)
            builder.Entity<AdministrativeProcess>()
                .HasMany<Request>()
                .WithOne(r => r.Process)
                .HasForeignKey(r => r.ProcessId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete of requests when process is deleted

            // Each ProcessApprovalStep is assigned to one user (Owner)
            // But we don’t define the navigation property on ApplicationUser (so we use WithMany())
            builder.Entity<ProcessApprovalStep>()
                .HasOne(p => p.Owner)
                .WithMany() // no collection of steps defined in ApplicationUser
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // do not delete step if user is deleted

            // Each Request is created by one user
            // Again, no navigation defined in ApplicationUser, so we use WithMany()
            builder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // keep request even if user is deleted

            // Each RequestStep belongs to one Request (1:N)
            builder.Entity<RequestStep>()
                .HasOne(rs => rs.Request)
                .WithMany()
                .HasForeignKey(rs => rs.RequestId)
                .OnDelete(DeleteBehavior.Restrict); // prevent deletion of steps if request is deleted

            // Each RequestStep is handled by one user (Owner)
            builder.Entity<RequestStep>()
                .HasOne(rs => rs.Owner)
                .WithMany()
                .HasForeignKey(rs => rs.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // don't delete step if user is deleted

            // Each RequestStepFile belongs to one RequestStep
            builder.Entity<RequestStepFile>()
                .HasOne(f => f.RequestStep)
                .WithMany()
                .HasForeignKey(f => f.RequestStepId)
                .OnDelete(DeleteBehavior.Restrict); // keep file even if step is deleted

            // Each Notification is sent to one user
            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent notification deletion if user is deleted
        }

    }


}
