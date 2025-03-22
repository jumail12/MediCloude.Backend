using AuthService.Application.Interfaces.IRepos;
using AuthService.Application.Interfaces.IServices;
using Contarcts.Requests.Specialization;
using Contarcts.Responses.Specialization;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Admin_authCmd.Handler
{
    public class DrLicenseApproveCommandHandler : IRequestHandler<DrLicenseApproveCommand, string>
    {
        private readonly IRequestClient<SpecializationExistsReq> _requestClient;
        private readonly IDrRepo _repo;
        private readonly IEmailService _emailService;

        public DrLicenseApproveCommandHandler(IRequestClient<SpecializationExistsReq> requestClient, IDrRepo drRepo,IEmailService emailService)
        {
            _repo = drRepo;
            _requestClient = requestClient;
            _emailService = emailService;
        }
        public async Task<string> Handle(DrLicenseApproveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var allDrs = await _repo.GetAllDrs();
                var doctor = allDrs.FirstOrDefault(a => a.Id == request.DrId);

                if (doctor == null)
                {
                    throw new ValidationException("dr not found");
                }

                var res = await _requestClient.GetResponse<SpecializationExistsResponse>(new SpecializationExistsReq(doctor.Specialization_id ?? Guid.Empty));
                var isEx = res.Message;
                if (res == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                if (!isEx.Exists)
                {
                    throw new Exception("Specialization category not found");
                }


                string pattern = @"^[A-Z]{2,3}/?\d{5,7}/?\d{4}$";

                if (!Regex.IsMatch(doctor.Medical_license_number, pattern))
                {
                    throw new Exception("Medical license number is not valid");
                }

                doctor.Is_approved = true;
                await _repo.SaveAsync();
                await _emailService.DrLicenseApprovedEmail(doctor.Email);
                
                return "Verfication completed and approved";

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
