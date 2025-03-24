using AuthService.ApiResponses;
using AuthService.Application.Commands.Admin_authCmd;
using AuthService.Application.Common.DTOs.AdminDTOs;
using AuthService.Application.Common.DTOs.CommonDtos;
using AuthService.Application.Quries.Admin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles ="Admin")]
        [HttpPatch("dr-license-approve")]
        public async Task<IActionResult> LicenseApprove([FromBody] DrLicenseApproveCommand cmd)
        {
            try
            {
                var res = await _sender.Send(cmd);
                return Ok(new ApiResponse<string>(200, "success",res, null));
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

        [Authorize(Roles ="Admin")]
        [HttpPatch("dr-license-reject")]
        public async Task<IActionResult> LicenseReject([FromBody] DrLicenseRejectCommand cmd)
        {
            try
            {
                var res = await _sender.Send(cmd);
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

        [Authorize(Roles = "Admin")]
        [HttpGet("all-dr-verify-pending")]
        public async Task<IActionResult> GetAllVerifyPendingDrs(int pageNumber, int pageSize)
        {
            try
            {
                var res = await _sender.Send(new DrLicenseGetAllQuery(pageNumber,pageSize));
                return Ok(new ApiResponse<DrLicenseGetAllPageResDto>(200, "success", res, null));
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> TokenService([FromBody] AdminRefreshTokenCommand adminRefreshToken)
        {
            try
            {
                var res = await _sender.Send(adminRefreshToken);
                return Ok(new ApiResponse<RefreshTokenResDto>(200, "Success", res, null));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse<string>(401, "Unauthroized", null, ex.Message));
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
