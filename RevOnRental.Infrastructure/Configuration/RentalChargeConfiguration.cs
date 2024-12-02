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
    public class RentalChargeConfiguration : IEntityTypeConfiguration<RentalCharge>
    {
        public void Configure(EntityTypeBuilder<RentalCharge> builder)
        {
            builder.HasKey(rc => rc.Id);

            builder.HasOne(rc => rc.Vehicle)
                .WithOne(v => v.RentalCharges) 
                .HasForeignKey<RentalCharge>(rc => rc.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.Property(rc => rc.HourlyRate)
                .IsRequired()
                .HasColumnType("float");
            builder.Property(rc => rc.FullDayRate)
              .IsRequired()
              .HasColumnType("float");
            builder.Property(rc => rc.HalfDayRate)
              .IsRequired()
              .HasColumnType("float");

        }
    }
}
