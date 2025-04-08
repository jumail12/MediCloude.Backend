using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Interfaces.IServices
{
    public interface IGeminiService 
    {
        Task<string> AskGeminiAsync(string userMessage);
    }
}
