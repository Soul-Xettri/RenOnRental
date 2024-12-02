using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.RentalBooking.Queries
{
   
        public class GetUserBookingHistoryQuery : IRequest<List<UserBookingHistoryDto>>
        {
            public int UserId { get; set; }
        }
    public class GetUserBookingHistoryQueryHandler : IRequestHandler<GetUserBookingHistoryQuery, List<UserBookingHistoryDto>>
    {
        private readonly IAppDbContext _context;

        public GetUserBookingHistoryQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserBookingHistoryDto>> Handle(GetUserBookingHistoryQuery request, CancellationToken cancellationToken)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Vehicle)
                .ThenInclude(x=>x.Business)
                .Where(r => r.UserID == request.UserId).Select(x => new UserBookingHistoryDto
                {
                    UserId= x.UserID,
                    UserName=x.User.FullName,
                    BusinessName = x.Vehicle.Business.BusinessName,
                    Model= x.Vehicle.Model,
                    Brand=x.Vehicle.Brand,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    VehicleType=x.Vehicle.VehicleType,
                    VehicleId=x.VehicleId,
                    TotalPrice=x.TotalPrice,
                    CreatedDate= x.CreatedDate,
                    Photo = new PhotoDto
                    {
                        FileContent = x.Vehicle.FileContent,
                        ContentType = x.Vehicle.ContentType,
                        FileName = $"{x.Vehicle.Brand}-{x.Vehicle.Model}.jpg",
                    }
                })
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }
    }

}
