using Microsoft.AspNetCore.Mvc;
using Scholarship_Plaatform_Backend.Models;
using Scholarship_Plaatform_Backend.Services;

namespace Scholarship_Plaatform_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _services;

        public FeedbackController(FeedbackService service)
        {
            _services = service;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            try
            {
                /* method to get all the feedbacks
                  returns all the feedbacks
                  if the list is empty then its a bad request
                 */
                var exs = await _services.GetAllFeedbacks();
                Console.WriteLine(exs);
                return Ok(exs);
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksByUserId(int userId)
        {
            try
            {
                /* method to get a Feedback of a User by its id
                   if exists then return Feedback
                   else Bad request
                 */
                var exs = await _services.GetFeedbacksByUserId(userId);
                return Ok(exs);
            }
            catch (SystemException ex)
            {
                return StatusCode(200, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            try
            {
                /* method to add a Feedback */
                var exs = await _services.AddFeedback(feedback);
                if (exs != null)
                {
                    return Ok(new { msg = "Feedback added successfully" });
                }
                else
                {
                    return BadRequest("Failed to get the data");
                }
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{feedbackId}")]
        public async Task<ActionResult> DeleteFeedback(int feedbackId)
        {
            try
            {
                /* check if the Feedback object to be deleted is avalailable
                    if available, delete the Feedback
                    else return NotFound
                 */
                bool isDeleted = await _services.DeleteFeedback(feedbackId);
                if (isDeleted)
                {
                    return Ok(new { msg = "Feedback deleted successfully" });
                }
                return NotFound("Cannot find any feedback");
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
