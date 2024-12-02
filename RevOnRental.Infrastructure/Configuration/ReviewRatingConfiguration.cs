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
    public class ReviewRatingConfiguration : IEntityTypeConfiguration<ReviewRating>
    {
        public void Configure(EntityTypeBuilder<ReviewRating> builder)
        {
            builder.HasKey(rr => rr.Id);
            builder.Property(rr => rr.Rating).IsRequired();
            builder.Property(rr => rr.Review).HasMaxLength(5000);
            builder.Property(rr => rr.ReviewDate).IsRequired();

            // Relationship with User
            builder.HasOne(rr => rr.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(rr => rr.UserId).OnDelete(DeleteBehavior.Cascade); ;

            // Relationship with Business
            builder.HasOne(rr => rr.Business)
                .WithMany(b => b.Reviews)
                .HasForeignKey(rr => rr.BusinessId).OnDelete(DeleteBehavior.Cascade); ;

        }
    }

}
