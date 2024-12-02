using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Dtos.BusinessDetails;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain.Enums;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Businesses.Queries
{
    public class GetBusinessDetailsQuery : IRequest<BusinessDetailsDto>
    {
        public int BusinessId { get; set; }
        public VehicleType VehicleType { get; set; }
    }

    public class GetBusinessDetailsHandler : IRequestHandler<GetBusinessDetailsQuery, BusinessDetailsDto>
    {
        private readonly IAppDbContext _context;

        public GetBusinessDetailsHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessDetailsDto> Handle(GetBusinessDetailsQuery request, CancellationToken cancellationToken)
        {
            // Fetch the business details
            var business = await _context.Businesses
                .Include(b => b.UserBusiness)
                    .ThenInclude(ub => ub.User)
                .Include(b => b.Vehicles.Where(x=>x.VehicleType==request.VehicleType))
                .ThenInclude(x=>x.RentalCharges)
                .Include(b => b.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(b => b.Id == request.BusinessId, cancellationToken);

            if (business == null)
            {
                throw new KeyNotFoundException("Business not found.");
            }

            // Calculate the average rating
            double averageRating = business.Reviews.Any() ? business.Reviews.Average(r => r.Rating) : 0;

            // Prepare the response DTO
            var businessDetailsDto = new BusinessDetailsDto
            {
                Id = request.BusinessId,
                BusinessName = business.BusinessName,
                Address = business.UserBusiness.User.Address,
                ContactNumber = business.UserBusiness.User.ContactNumber,
                AverageRating = averageRating,
                AvailableVehicles = business.Vehicles.Select(v => new VehicleDto
                {
                    Id = v.Id,
                    Model = v.Model,
                    Brand = v.Brand,
                    AvailabilityStatus = v.AvailabilityStatus,
                    HourlyRate = (decimal)v.RentalCharges.HourlyRate,
                    HalfDayRate = (decimal)v.RentalCharges.HalfDayRate,
                    FullDayRate = (decimal)v.RentalCharges.FullDayRate,
                    Photo = new PhotoDto
                    {
                        FileContent = v.FileContent,
                        ContentType = v.ContentType,
                        FileName = $"{v.Brand}-{v.Model}.jpg",
                    }
                }).ToList(),
                Reviews = business.Reviews.Select(r => new ReviewDto
                {
                    UserName = r.User.FullName,
                    Rating = r.Rating,
                    Review = r.Review,
                    ReviewDate = r.ReviewDate
                }).ToList()
            };

            return businessDetailsDto;
        }
    }
}
