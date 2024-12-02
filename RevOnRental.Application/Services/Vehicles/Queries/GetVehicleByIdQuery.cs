using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Vehicles.Queries
{
    public class GetVehicleByIdQuery : IRequest<VehicleDto>
    {
        public int VehicleId { get; set; }

    }
    public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto>
    {
        private readonly IAppDbContext _context;

        public GetVehicleByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<VehicleDto> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var data= await _context.Vehicles.Select(x => new VehicleDto
            {
                Id = x.Id,
                AvailabilityStatus = x.AvailabilityStatus,
                Brand = x.Brand,
                Model = x.Model,
                NumberOfVehicle = x.NumberOfVehicle,
                VehicleType = x.VehicleType,
            }).FirstOrDefaultAsync(v => v.Id == request.VehicleId);
            return data;
        }
    }
}
