using AuthService.ApiResponses;
using AuthService.Application.Commands.Doctor_authCmd;
using AuthService.Application.Common.DTOs.DoctorDTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAuthController : ControllerBase
    {
        private readonly ISender _send;
        public DoctorAuthController(ISender send)
        {
            _send = send;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]DrRegisterCommand drRegister)
        {
            try
            {
                var res= await _send.Send(drRegister);
                return Ok(new ApiResponse<string>(200,"success",res,null));
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

        [HttpPost("verify-otp")]
        public async Task<IActionResult> EmailVerfy([FromBody]DrEmailVerifyCommand drEmailVerify)
        {
            try
            {
                var res=await _send.Send(drEmailVerify);
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
        public async Task<IActionResult> Login([FromBody]DrLoginCommand drLogin)
        {
            try
            {
                var res= await _send.Send(drLogin);
                return Ok(new ApiResponse<DrLoginResDto>(200, "success", res, null));
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPw([FromBody]DrForgotPasswordCommand forgotPassword)
        {
            try
            {
                var res= await _send.Send(forgotPassword);
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

        [HttpPost("verify-otp-reset-password")]
        public async Task<IActionResult> VerifyRePassOtp([FromBody]DrEmailVerfyRePassCommand drEmailVerfyRePass)
        {
            try
            {
                var res=  await _send.Send(drEmailVerfyRePass);
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

        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPw([FromBody] DrResPassCommand drResPass)
        {
            try
            {
                var res = await _send.Send(drResPass);
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

        [HttpPatch("verify-license")]
        public async Task<IActionResult> VerifyLicense([FromBody]DrLicenseVerifyCommand drLicenseVerify)
        {
            try
            {
                var res= await _send.Send(drLicenseVerify);
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
