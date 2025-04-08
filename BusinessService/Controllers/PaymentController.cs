using BusinessService.ApiResponses;
using BusinessService.Aplication.Commands.AppoinmentPaymentCommand;
using BusinessService.Aplication.Common.DTOs.Payment;
using BusinessService.Aplication.Quries.Payment;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ISender _sender;
        public PaymentController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpPost("razor-order-create")]
        public async Task<IActionResult> OrderCreate(long price)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                var res =await _sender.Send(new RazorPayPaymentCreateCommand(Guid.Parse(userId),price));
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

        [Authorize(Roles = "Patient")]
        [HttpPost("payment-making")]
        public async Task<IActionResult> PaymetMaking([FromBody]PaymentAppoinmentCommand appoinment)
        {
            try
            {
                var res = await _sender.Send(appoinment);
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

        [Authorize(Roles="Doctor")]
        [HttpGet("dr-dashboard")]
        public async Task<IActionResult> DrDashBoard(int pageNumber, int pageSize)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                var res = await _sender.Send(new DrPaymentDashboardQuery(Guid.Parse(userId),pageNumber,pageSize));
                return Ok(new ApiResponse<DrPaymentDashboardResDto>(200, "success", res, null));
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
