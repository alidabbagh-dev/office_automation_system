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
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
           
            builder.HasIndex(u => u.UserCode)
                .IsUnique()
                .HasFilter("[UserCode] IS NOT NULL");

            builder.Property(u => u.UserCode)
           .HasMaxLength(50)
           .HasColumnType("nvarchar(50)");

            // RefreshToken
            builder.Property(u => u.RefreshToken)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // RefreshTokenExpiryTime
            builder.Property(u => u.RefreshTokenExpiryTime)
                .HasColumnType("datetime2");

           
        }
    }
}
