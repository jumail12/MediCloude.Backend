using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.SpecializationDrCommand.Handler
{
    public class SpecializationDrAddCmdHandler : IRequestHandler<SpecializationDrAddCmd, string>
    {
        private readonly ISpecializationRepo _repo;
        public SpecializationDrAddCmdHandler(ISpecializationRepo repo)
        {
            _repo = repo;
        }

        public async Task<string> Handle(SpecializationDrAddCmd request, CancellationToken cancellationToken)
        {
            try
            {
                var newSpl = new Specialization_doctor
                {
                    Category = request.Category,
                    Created_on = DateTime.UtcNow,
                    Updated_on = DateTime.UtcNow,
                    Created_by = "Admin",
                    Updated_by = "Admin"
                };

                await _repo.AddSplAsync(newSpl);
                return "New speclization category added";
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }
    }
}
