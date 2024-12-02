using Microsoft.AspNetCore.Mvc;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Vehicles.Command;
using RevOnRental.Application.Services.Vehicles.Queries;
using RevOnRental.Domain.Models;

namespace RevOnRental.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : BaseController
    {
        

        // GET: api/vehicle/GetAllVehicles
        [HttpGet("GetAllVehicles")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await Mediator.Send(new GetAllVehicleQuery());
            return Ok(vehicles);
        }

        // GET: api/vehicle/GetById/{id}
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var dto = new GetVehicleByIdQuery
            {
                VehicleId = id
            };
            var vehicle = await Mediator.Send(dto);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        // POST: api/vehicle/add
        [HttpPost("add")]
        public async Task<IActionResult> AddVehicle([FromForm] AddVehicleCommand vehicle)
        {
            try
            {
                var isAdded = await Mediator.Send(vehicle);
                if (isAdded)
                {
                    return Ok(new { message = "Vehicle added successfully." });
                }
                return BadRequest(new { message = "Failed to add vehicle." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/vehicle/update/{id}
        [HttpPut("update")]
        public async Task<IActionResult> UpdateVehicle( [FromBody] UpdateVehicleCommand vehicle)
        {
            try
            {
                var isUpdated = await Mediator.Send(vehicle);
                if (!isUpdated)
                {
                    return NotFound(new { message = "Vehicle not found." });
                }
                return Ok(new { message = "Vehicle updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/vehicle/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                var dto = new DeleteVehicleCommand {  Id=id};
                var isDeleted = await Mediator.Send(dto);
                if (!isDeleted)
                {
                    return NotFound(new { message = "Vehicle not found." });
                }
                return Ok(new { message = "Vehicle deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}