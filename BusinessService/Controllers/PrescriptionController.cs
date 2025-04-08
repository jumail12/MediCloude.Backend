using BusinessService.ApiResponses;
using BusinessService.Aplication.Commands.Prescription;
using BusinessService.Aplication.Common.DTOs.Prescription;
using BusinessService.Aplication.Quries.Prescription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly ISender _sender;
        public PrescriptionController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles ="Doctor")]
        [HttpPost("add-prescription")]
        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionAddCommand prescriptionAdd)
        {
            try
            {
                var res= await _sender.Send(prescriptionAdd);
                return Ok(new ApiResponse<string>(200, "success", res, null));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Validation failed", null, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "An Internal Error occurred", null, ex.Message));
            }
        }

        [Authorize(Roles ="Patient")]
        [HttpGet("patient=prescriptions")]
        public async Task<IActionResult> GetPatientPrscriptions(int pageNumber, int pageSize)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                var res = await _sender.Send(new GetAllPrescriptionPatientQuery(Guid.Parse(userId), pageNumber, pageSize));
                return Ok(new ApiResponse<PatientPrescription_PaginationResDto>(200, "success", res, null));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Validation failed", null, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "An Internal Error occurred", null, ex.Message));
            }
        }

    }
}
