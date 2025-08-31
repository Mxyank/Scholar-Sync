
using Microsoft.AspNetCore.Mvc;
using Scholarship_Plaatform_Backend.Exceptions;
using Scholarship_Plaatform_Backend.Models;
using Scholarship_Plaatform_Backend.Services;

namespace Scholarship_Plaatform_Backend.Controllers
{
    [ApiController]
    [Route("api/scholarship")]
    public class ScholarshipController : ControllerBase
    {
        private readonly ScholarshipService _service;

        public ScholarshipController(ScholarshipService service)
        {
            _service = service;
        }

        // 1. Get all scholarships

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scholarship>>> GetAllScholarships()
        {
            try
            {
                var scholarships = await _service.GetAllScholarships();
                return Ok(scholarships);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 2. Get scholarship by scholarshipId

        [HttpGet("{scholarshipId}")]
        public async Task<ActionResult<Scholarship>> GetScholarshipById(int scholarshipId)
        {
            try
            {
                var scholarship = await _service.GetScholarshipById(scholarshipId);
                return Ok(scholarship);
            }
            catch (ScholarshipException)
            {
                return NotFound("Cannot find any scholarship");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 3. Add a new scholarship

        [HttpPost]
        public async Task<ActionResult> AddScholarship([FromBody] Scholarship scholarship)
        {
            try
            {
                bool isAdded = await _service.AddScholarship(scholarship);
                if (isAdded)
                {
                    return Ok(new { mssg = "Scholarship added successfully" });
                }
                //return Internal Server Error
                return StatusCode(500, "Failed to add scholarship");
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

        // 4. Update an existing scholarship

        [HttpPut("{scholarshipId}")]
        public async Task<ActionResult> UpdateScholarship(int scholarshipId, [FromBody] Scholarship scholarship)
        {
            try
            {
                bool isUpdated = await _service.UpdateScholarship(scholarshipId, scholarship);
                if (isUpdated)
                {
                    return Ok(new { mssg = "Scholarship updated successfully" });
                }
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

        // 5. Delete a scholarship

        [HttpDelete("{scholarshipId}")]
        public async Task<ActionResult> DeleteScholarship(int scholarshipId)
        {
            try
            {
                bool isDeleted = await _service.DeleteScholarship(scholarshipId);
                if (isDeleted)
                {
                    return Ok(new { mssg = "Scholarship deleted successfully" });
                }
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

    }

}
