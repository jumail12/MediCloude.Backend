using BusinessService.ApiResponses;
using BusinessService.Aplication.Common.DTOs.Doctor;
using BusinessService.Aplication.Quries.Doctor;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorViewController : ControllerBase
    {
        private readonly ISender _sender;

        public DoctorViewController(ISender sender)
        {
            _sender = sender;
        }

        
        [HttpGet("id")]
        public async Task<IActionResult> GetDrById(Guid Id)
        {
            try
            {
                var res = await _sender.Send(new DrByIdQuery(Id));
                return Ok(new ApiResponse<DrByIdResDto>(200, "success", res, null));
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
