using BusinessService.Aplication.Interfaces.IServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Gemini
{
    public class GeminQueryHandler : IRequestHandler<GeminQuery, string>
    {
        private readonly IGeminiService _geminiService;
        public GeminQueryHandler(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public async Task<string> Handle(GeminQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _geminiService.AskGeminiAsync(request.msg);
                return res;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
