using BusinessService.ApiResponses;
using BusinessService.Aplication.Quries.Gemini;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiChatController : ControllerBase
    {
        private readonly ISender _sender;
        public GeminiChatController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost("chatbot-ask")]
        public async Task<IActionResult> GeminiBot([FromBody]GeminQuery geminQuery)
        {
            try
            {
                var res= await _sender.Send(geminQuery);
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
