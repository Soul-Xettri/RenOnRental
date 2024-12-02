using Microsoft.AspNetCore.Mvc;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Services.Rating.Command;

namespace RevOnRental.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : BaseController
    {
        [HttpPost("submit-rating")]
        public async Task<ActionResult> SubmitRating([FromBody] SubmitRatingCommand ratingDto)
        {
            var result = await Mediator.Send(ratingDto);

            if (result)
            {
                return Ok("Rating submitted successfully.");
            }

            return BadRequest("Failed to submit rating.");
        }
    }
}
