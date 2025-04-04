using BusinessService.ApiResponses;
using BusinessService.Aplication.Commands.AppoinmentPaymentCommand;
using BusinessService.Aplication.Common.DTOs.Appoinment;
using BusinessService.Aplication.Quries.Doctor;
using BusinessService.Aplication.Quries.Patient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrUpcomingAppoinmentsController : ControllerBase
    {
        private readonly ISender _sender;
        public DrUpcomingAppoinmentsController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("dr-upcoming-appoiments")]
        public async Task<IActionResult> UpcomingAPpoinments(int pageNumber, int pageSize)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                var res = await _sender.Send(new DrUpcommingAppoinmentsQuery(Guid.Parse(userId),pageNumber,pageSize));
                return Ok(new ApiResponse<AppoinmentPaginationResDto>(200, "success", res, null));
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

        [Authorize(Roles ="Doctor")]
        [HttpGet("ap-by-id")]
        public async Task<IActionResult> AppoinmentById(Guid id)
        {
            try
            {
                var res= await _sender.Send(new DrUPAPByIdQuery(id));
                return Ok(new ApiResponse<DrUpcommingAppoinmentsResDto>(200, "success", res, null));
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
        [HttpGet("patient-up-bookings")]
        public async Task<IActionResult> GetPatientBookings(int pageNumber, int pageSize)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                var res= await _sender.Send(new PatientBookedAPPsQuery(Guid.Parse(userId),pageNumber,pageSize));
                return Ok(new ApiResponse<AppoinmentPatientPaginationResDto>(200, "success", res, null));
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
        [HttpGet("patient-up-bookings-byid")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var res =await _sender.Send(new PatientBookedAppoinmentByIdQuery(id));
                return Ok(new ApiResponse<PatientAppResDto>(200, "success", res, null));
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

        [Authorize(Roles ="Doctor")]
        [HttpPost("Appinment-alert-patient")]
        public async Task<IActionResult> AlertAppoinment(Guid appID)
        {
            try
            {
                var res = await _sender.Send(new AppoinmentStartPatientAlertNotCmd(appID));
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



    }
}
