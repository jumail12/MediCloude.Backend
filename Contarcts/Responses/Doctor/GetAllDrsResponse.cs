using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contarcts.Responses.Doctor
{
    public class GetAllDrsResponse
    {
        public List<DrByIdResponse> doctors { get; set; }
    }
}
