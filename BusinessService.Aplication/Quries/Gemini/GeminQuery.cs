using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Gemini
{
    public record class GeminQuery(string msg) : IRequest<string>;
}
