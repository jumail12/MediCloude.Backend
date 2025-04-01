using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistance;
using Contarcts.Requests.Patient;
using Contarcts.Responses.Patient;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Consumers.Patient
{
    public class PatientByIdConsumer : IConsumer<PatientByIdRequest>
    {
        private readonly AuthDbContext _authDbContext;
        public PatientByIdConsumer(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task Consume(ConsumeContext<PatientByIdRequest> context)
        {
            try
            {
                var patient = await _authDbContext.Patients.FirstOrDefaultAsync(a=>a.Patient_id==context.Message.id);
                if (patient == null)
                {
                    throw new Exception("Patient not found");
                }

                var res = new PatientByIdResponse()
                {
                    Patient_id = context.Message.id,
                    Patient_name=patient.Patient_name,
                    Phone=patient.Phone,
                    Address=patient.Address,
                    Dob=patient.Dob,
                    Email=patient.Email,
                    Gender=patient.Gender,
                };

                await context.RespondAsync(res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
