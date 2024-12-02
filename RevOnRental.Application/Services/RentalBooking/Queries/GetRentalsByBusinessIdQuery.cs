using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos.BusinessDetails;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Businesses.Queries;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.RentalBooking.Queries
{
    public class GetRentalsByBusinessIdQuery  : IRequest<List<RentalDto>>
    {
        public int BusinessId { get; set; }
    }

    public class GetRentalsByBusinessIdQueryHandler : IRequestHandler<GetRentalsByBusinessIdQuery, List<RentalDto>>
    {
        private readonly IAppDbContext _context;

        public GetRentalsByBusinessIdQueryHandler(IAppDbContext context)
        {
                _context=context;
        }
        public async Task<List<RentalDto>> Handle(GetRentalsByBusinessIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Vehicle)
                .Where(r => r.Vehicle.BusinessID == request.BusinessId).Select(x => new RentalDto
                {
                    Id = x.Id,
                    UserID = x.UserID,
                    UserName = x.User.FullName,
                    VehicleId = x.VehicleId,
                    Photo = new Dtos.PhotoDto
                    {
                        ContentType = x.Vehicle.ContentType,
                        FileContent = x.Vehicle.FileContent,
                        FileName = x.Vehicle.FileName,
                    },
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    TotalPrice = x.TotalPrice,
                    RentalStatus = x.RentalStatus.ToString(),

                })
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }
    }

}
