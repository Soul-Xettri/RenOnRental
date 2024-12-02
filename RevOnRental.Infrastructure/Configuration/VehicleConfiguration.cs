using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Infrastructure.Configuration
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.VehicleType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.Brand)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.NumberOfVehicle)
                .IsRequired();

            builder.Property(v => v.AvailabilityStatus)
               .IsRequired()
               .HasDefaultValue(true);

            builder.HasOne(v => v.Business)
                .WithMany(b => b.Vehicles)
                .HasForeignKey(v => v.BusinessID).OnDelete(DeleteBehavior.Cascade);
            ;

        }
    }
}
