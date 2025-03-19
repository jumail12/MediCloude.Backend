using AuthService.ApiResponses;
using AuthService.Application.Commands.Admin_authCmd;
using AuthService.Application.Common.DTOs.AdminDTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly ISender _sender;
        public AdminAuthController(ISender send)
        {
            _sender = send;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminRegisterCommand command)
        {
            try
            {
                var res= await _sender.Send(command);
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginCommand command)
        {
            try
            {
                var res= await _sender.Send(command);
                return Ok(new ApiResponse<AdminLoginResDto>(200, "success", res, null));
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
