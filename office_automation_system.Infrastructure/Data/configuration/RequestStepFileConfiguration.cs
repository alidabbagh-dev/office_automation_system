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
    public class RequestStepFileConfiguration : IEntityTypeConfiguration<RequestStepFile>
    {
        public void Configure(EntityTypeBuilder<RequestStepFile> builder)
        {

            // Each RequestStepFile belongs to one RequestStep
            builder
                .HasOne(f => f.RequestStep)
                .WithMany()
                .HasForeignKey(f => f.RequestStepId)
                .OnDelete(DeleteBehavior.Restrict); // keep file even if step is deleted


        }
    }
}
