using BusinessService.ApiResponses;
using BusinessService.Aplication.Commands.SpecializationDrCommand;
using BusinessService.Aplication.Common.DTOs.SpecializationDtos;
using BusinessService.Aplication.Quries.SpecializationDrQuries;
using BusinessService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISender _sender;

        public SpecializationController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]SpecializationDrAddCmd cmd)
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res= await _sender.Send(new GetAllSpecializationsQurey());
                return Ok(new ApiResponse<List<GetAllSpecializationResDto>>(200, "success", res, null));
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
