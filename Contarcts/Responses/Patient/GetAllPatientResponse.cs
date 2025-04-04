using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contarcts.Responses.Patient
{
    public class GetAllPatientResponse
    {
       public List<PatientByIdResponse> patients {  get; set; }
    }
}
