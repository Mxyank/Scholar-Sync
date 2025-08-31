using Microsoft.AspNetCore.Mvc;
using Scholarship_Plaatform_Backend.Exceptions;
using Scholarship_Plaatform_Backend.Models;
using Scholarship_Plaatform_Backend.Services;

namespace Scholarship_Plaatform_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScholarshipApplicationController : ControllerBase
    {
        private readonly ScholarshipApplicationService _scholarshipApplicationServices;

        public ScholarshipApplicationController(ScholarshipApplicationService service)
        {
            _scholarshipApplicationServices = service;
        }

        /* Fetches all scholarship applications. */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScholarshipApplication>>> GetAllScholarshipApplications()
        {
            try
            {
                var scholarships = await _scholarshipApplicationServices.GetAllScholarshipApplications();
                return Ok(scholarships);
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /* Fetches a scholarship application by user ID. */
        [HttpGet("{userId}")]
        public async Task<ActionResult<ScholarshipApplication>> GetScholarshipApplicationByUserId(int userId)
        {
            try
            {
                var exs = await _scholarshipApplicationServices.GetScholarshipApplicationsByUserId(userId);
                if (exs != null)
                {
                    return Ok(exs);
                }
                return NotFound("Cannot Find any scholarship application");
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /* Adds a new scholarship application. */
        [HttpPost]
        public async Task<ActionResult> AddScholarshipApplication([FromBody] ScholarshipApplication scholarshipApplication)
        {
            try
            {
                bool isAdded = await _scholarshipApplicationServices.AddScholarshipApplication(scholarshipApplication);
                if (isAdded)
                    return Ok(new { mssg = "Scholarship application added successfully" });
                else
                    return BadRequest("Failed to add scholarship application");
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /* Updates an existing scholarship application by ID. */
        [HttpPut("{scholarshipApplicationId}")]
        public async Task<ActionResult> UpdateScholarshipApplication(int scholarshipApplicationId, [FromBody] ScholarshipApplication scholarshipApplication)
        {
            try
            {
                var isUpdated = await _scholarshipApplicationServices.UpdateScholarshipApplication(scholarshipApplicationId, scholarshipApplication);
                if (isUpdated)
                    return Ok(new { mssg = "Scholarship application updated successfully" });

                return NotFound("Cannot find any scholarship.");
            }
            catch (ScholarshipException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /* Deletes a scholarship application by ID. */
        [HttpDelete("{scholarshipApplicationId}")]
        public async Task<ActionResult> DeleteScholarshipApplication(int scholarshipApplicationId)
        {
            try
            {
                bool isDeleted = await _scholarshipApplicationServices.DeleteScholarshipApplication(scholarshipApplicationId);
                if (isDeleted)
                    return Ok(new { msg = "Scholarship deleted successfully" });

                return NotFound("Cannot find any scholarship.");
            }
            catch (ScholarshipException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /* Gets the number of applications for a scholarship. */
        [HttpGet("count/{scholarshipId}")]
        public IActionResult GetApplicationCount(int scholarshipId)
        {
            try
            {
                var count = _scholarshipApplicationServices.GetApplicationCountForScholarship(scholarshipId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting application count for scholarship ID {scholarshipId}: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the application count.");
            }
        }
    }
}
