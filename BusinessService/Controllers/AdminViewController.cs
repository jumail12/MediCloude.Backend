using BusinessService.ApiResponses;
using BusinessService.Aplication.Commands.AdminCmd;
using BusinessService.Aplication.Common.DTOs.AdminDtos;
using BusinessService.Aplication.Quries.Admin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminViewController : ControllerBase
    {
        private readonly ISender _sender;
        public AdminViewController(ISender sender)
        {
                _sender = sender;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("patients")]
        public async Task<IActionResult> GetAllPatients(string? name, int pageNumber, int pageSize)
        {
            try
            {
                var res= await _sender.Send(new AdminGetAllPatientQuery(name, pageNumber, pageSize));
                return Ok(new ApiResponse<AdminGetAllPatient_PaginationResDto>(200, "success", res, null));
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
        [HttpPatch("patient/block-unblock")]
        public async Task<IActionResult> BlockUnBlockPatient(Guid id)
        {
            try
            {
                var res = await _sender.Send(new PatientBlockUnblockCommand(id));
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
        [HttpGet("patient-id")]
        public async Task<IActionResult> PatientById(Guid Id)
        {
            try
            {
                var res = await _sender.Send(new AdminPatientByIdQuery(Id));
                return Ok(new ApiResponse<AdminPatientByIdResDto>(200, "success", res, null));
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
        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDrs(string? name, int pageNumber, int pageSize)
        {
            try
            {
                var res = await _sender.Send(new AdminGetAllDrQuery(name, pageNumber, pageSize));
                return Ok(new ApiResponse<AdminGetAllDoctor_PaginationResDto>(200, "success", res, null));
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
        [HttpGet("dr-by-id")]
        public async Task<IActionResult> DrbyId(Guid drId)
        {
            try
            {
                var res = await _sender.Send(new AdminDoctorByIdQuery(drId));
                return Ok(new ApiResponse<AdminDoctorByIdResDto>(200, "success", res, null));
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
        [HttpPatch("dr-block/unblock")]
        public async Task<IActionResult> DrBlockUnblock(Guid id)
        {
            try
            {
                var res = await _sender.Send(new DrBlockUnBlockCmd(id));
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
        [HttpGet("admin-dashboard")]
        public async Task<IActionResult> DashBoard(int pageNumber, int pageSize)
        {
            try
            {
                var res= await _sender.Send(new AdminPaymentDashBoardQuery(pageNumber, pageSize));
                return Ok(new ApiResponse<AdminPaymentDashBoardResDto>(200, "success", res, null));
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
