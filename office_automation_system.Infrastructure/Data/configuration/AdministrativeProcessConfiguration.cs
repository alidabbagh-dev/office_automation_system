using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using office_automation_system.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Data.configuration
{
    public class AdministrativeConfiguration : IEntityTypeConfiguration<AdministrativeProcess>
    {
        public void Configure(EntityTypeBuilder<AdministrativeProcess> builder)
        {

            // A process can have many approval steps (1:N)
            // Each approval step belongs to one AdministrativeProcess
            builder
                .HasMany<ProcessApprovalStep>()
                .WithOne(p => p.AdministrativeProcess) // navigation to parent
                .HasForeignKey(p => p.AdministrativeProcessId)
                .OnDelete(DeleteBehavior.Restrict); // do not delete steps when process is deleted

            // A process can have many requests (1:N)
            builder
                .HasMany<Request>()
                .WithOne(r => r.Process)
                .HasForeignKey(r => r.ProcessId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete of requests when process is deleted


        }
    }
}
