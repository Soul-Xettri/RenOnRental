using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Dtos.BusinessDashboard;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Vehicles.Queries
{
    public class GetVehicleTypeDetailsQuery : IRequest<List<VehicleTypeDetailsDto>>
    {
        public int BusinessId { get; set; }
        public VehicleType VehicleType { get; set; }
      
    }

    public class GetVehicleTypeDetailsHandler : IRequestHandler<GetVehicleTypeDetailsQuery, List<VehicleTypeDetailsDto>>
    {
        private readonly IAppDbContext _context;

        public GetVehicleTypeDetailsHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleTypeDetailsDto>> Handle(GetVehicleTypeDetailsQuery request, CancellationToken cancellationToken)
        {
            // Fetch all vehicles of the specified type for the given business
            var vehicles = await _context.Vehicles
                .Where(v => v.BusinessID == request.BusinessId && v.VehicleType == request.VehicleType)
                .Include(v => v.RentalCharges).GroupBy(v => new { v.Brand, v.Model })
                .Select(group => new VehicleTypeDetailsDto
                {
                    id = group.First().Id,  // Changed from group.Key.id to group.First().Id
                    VehicleType = (int)request.VehicleType,
                    Brand = group.Key.Brand,
                    Model = group.Key.Model,
                    TotalQuantity = group.Sum(v => v.NumberOfVehicle),
                    AvailableQuantity = group.Sum(v => v.NumberOfAvailableVehicle),
                    AvailabilityStatus = group.Any(v => v.AvailabilityStatus),
                    HourlyRate = (decimal)group.First().RentalCharges.HourlyRate,
                    HalfDayRate = (decimal)group.First().RentalCharges.HalfDayRate,
                    FullDayRate = (decimal)group.First().RentalCharges.FullDayRate,
                    Photo = new PhotoDto
                    {
                        FileName = group.First().FileName,
                        ContentType = group.First().ContentType,
                        FileContent = group.First().FileContent,
                    }

                })
                .ToListAsync(cancellationToken);


            return vehicles;
        }
    }

}
