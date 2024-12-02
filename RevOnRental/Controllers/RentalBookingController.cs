using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.RentalBooking.Queries;

namespace RevOnRental.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalBookingController : BaseController
    {
     
        [AllowAnonymous]
        [HttpGet("business/{businessId}")]
        public async Task<IActionResult> GetRentalsByBusiness(int businessId)
        {
            var rentals = await Mediator.Send(new GetRentalsByBusinessIdQuery { BusinessId=businessId});
            if (rentals == null || !rentals.Any())
            {
                return NotFound("No rentals found for the specified business.");
            }
            return Ok(rentals);
        }

        [HttpGet("booking-history/{userId}")]
        public async Task<IActionResult> GetBookingHistory(int userId)
        {
            try
            {
                var rentals = await Mediator.Send(new GetUserBookingHistoryQuery { UserId=userId });
                if (rentals == null || !rentals.Any())
                {
                    return NotFound("No booking history found for the specified user.");
                }

                return Ok(rentals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
