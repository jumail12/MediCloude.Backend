using BusinessService.ApiResponses;
using BusinessService.Aplication.Commands.DrAvailabilityCommand;
using BusinessService.Aplication.Common.DTOs.Availability;
using BusinessService.Aplication.Common.DTOs.Doctor;
using BusinessService.Aplication.Quries.DrAvailability;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrAvailabilityController : ControllerBase
    {
        private readonly ISender _sender;
        public DrAvailabilityController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("add-slot")]
        public async Task<IActionResult> AddAvailability([FromBody]DrAvailabilityAddCmd cmd)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<string>(401, "Unauthorized access", null, "UserId not found in context."));
                }
                var drid = Guid.Parse(userId); 
                var res = await _sender.Send(new DrAvailabilityAddCmdWithDrId(cmd,drid));
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

        
        [HttpGet("available-slots")]
        public async Task<IActionResult> DrAvailableSlots(Guid drid)
        {
            try
            {
                var res = await _sender.Send(new DrAvailabilityByIdQuery(drid));
                return Ok(new ApiResponse<List<DrAvailabilityByIdResDto>>(200, "success", res, null));
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
