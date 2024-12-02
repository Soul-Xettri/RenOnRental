using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Businesses.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Vehicles.Queries
{
    public class GetAllVehicleQuery : IRequest<List<VehicleDto>>
    {

    }
    public class GetAllVehicleQueryHandler : IRequestHandler<GetAllVehicleQuery, List<VehicleDto>>
    {
        private readonly IAppDbContext _context;

        public GetAllVehicleQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleDto>> Handle(GetAllVehicleQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Vehicles.Select(x => new VehicleDto
            {
                Id = x.Id,
                AvailabilityStatus = x.AvailabilityStatus,
                Brand = x.Brand,
                Model = x.Model,
                NumberOfVehicle = x.NumberOfVehicle,
                VehicleType = x.VehicleType,
            }).ToListAsync();
            return data;
        }
    }
}
