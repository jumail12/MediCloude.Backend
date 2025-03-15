using AuthService.ApiResponses;
using AuthService.Application.Commands.Patient_authCmd;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientAuthController : ControllerBase
    {
        private readonly ISender _sender;
        public PatientAuthController(ISender send)
        {
            _sender = send;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientRegisterCommand command)
        {
            try
            {
                var res=await _sender.Send(command);
                return Ok(new ApiResponse<string>(200,"Success",res,null));
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
        public async Task<IActionResult> VerfyOtp([FromBody] PatientEmailVerifyCommand patientEmailVerify)
        {
            try
            {
                var res=await _sender.Send(patientEmailVerify);
                return Ok(new ApiResponse<string>(200, "Success", res, null));
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
        public async Task<IActionResult> ForgotPassword([FromBody]PatientForgotPasswordCommand patientForgotPassword)
        {
            try
            {
                var res= await _sender.Send(patientForgotPassword);
                return Ok(new ApiResponse<string>(200, "Success", res, null));
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
        public async Task<IActionResult> VerfyOtpRePass([FromBody]PatientEmailVerfyRePassCommand patientEmailVerfyRePass)
        {
            try
            {
                var res=await _sender.Send(patientEmailVerfyRePass);
                return Ok(new ApiResponse<string>(200, "Success", res, null));
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
        public async Task<IActionResult> ResetPassword([FromBody]PatientResPassCommand patientResPass)
        {
            try
            {
                var res=await _sender.Send(patientResPass);
                return Ok(new ApiResponse<string>(200, "Success", res, null));
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
