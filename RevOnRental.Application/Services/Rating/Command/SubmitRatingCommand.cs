using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Rating.Command
{
    public class SubmitRatingCommand : IRequest<bool>
    {
        public int BusinessId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; } // Rating value (e.g., 1 to 5)
        public string Review { get; set; } // Optional review text
    }
    public class SubmitRatingHandler : IRequestHandler<SubmitRatingCommand, bool>
    {
        private readonly IAppDbContext _context;

        public SubmitRatingHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(SubmitRatingCommand request, CancellationToken cancellationToken)
        {

            // Validate the rating value
            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            // Check if the business exists
            var business = await _context.Businesses
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(b => b.Id == request.BusinessId, cancellationToken);

            if (business == null)
            {
                throw new ArgumentException("Business not found.");
            }

            // Create a new ReviewRating entry
            var reviewRating = new ReviewRating
            {
                UserId = request.UserId,
                BusinessId = request.BusinessId,
                Rating = request.Rating,
                Review = request.Review,
                ReviewDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            // Add the new rating to the database
            await _context.ReviewRatings.AddAsync(reviewRating, cancellationToken);

            // Update the business's average rating
           business.TotalRating = business.Reviews.Count + 1;
           business.AverageRating = (business.Reviews.Sum(r => r.Rating) + request.Rating) / business.TotalRating;

            business.Reviews.Add(reviewRating);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
