using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contarcts.Responses.Specialization
{
    public class GetAllSpecializationResponseList
    {
      public  List<SpecializationResponse> specializations { get; set; }
    }

   public record SpecializationResponse
    {
        public Guid splId {  get; set; }
        public string Category { get; set; }
    }
}
