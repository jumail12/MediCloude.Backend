using BusinessService.ApiResponses;
using BusinessService.Aplication.Commands.Notification;
using BusinessService.Aplication.Common.DTOs.Doctor;
using BusinessService.Aplication.Quries.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ISender _sender;
        public NotificationController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications(string recipient_type, int pageNumber, int pageSize)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                var res = await _sender.Send(new GetNotificationByIdQuery(Guid.Parse(userId),recipient_type,pageNumber,pageSize));
                return Ok(new ApiResponse<NotificationPageResDto>(200, "success", res, null));
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

        [Authorize]
        [HttpPatch("IsRead")]
        public async Task<IActionResult> Updatestaus(Guid nId,string recipient_type)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                var res = await _sender.Send(new UpdateNotficationStatusCommand(Guid.Parse(userId),nId,recipient_type));
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

        [Authorize]
        [HttpDelete("notification")]
        public async Task<IActionResult> DeleteNot(Guid nId, string recipient_type)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                var res = await _sender.Send(new DeleteNotificationCommand(Guid.Parse(userId), nId, recipient_type));
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

        [Authorize]
        [HttpGet("notifications/length")]
        public async Task<IActionResult> Length(string recipient_type)
        {
            try
            {
                var userId = Convert.ToString(HttpContext.Items["UserId"]);
                var res = await _sender.Send(new NotificationCoundQuery(Guid.Parse(userId),recipient_type));
                return Ok(new ApiResponse<int>(200, "success", res, null));
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
