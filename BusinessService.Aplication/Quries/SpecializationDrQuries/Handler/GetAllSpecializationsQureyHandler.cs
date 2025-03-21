using BusinessService.Aplication.Common.DTOs.SpecializationDtos;
using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.SpecializationDrQuries.Handler
{
    public class GetAllSpecializationsQureyHandler : IRequestHandler<GetAllSpecializationsQurey, List<GetAllSpecializationResDto>>
    {
        private readonly ISpecializationRepo _repo;
        public GetAllSpecializationsQureyHandler(ISpecializationRepo repo)
        {
            _repo = repo;
        }

        public async Task<List<GetAllSpecializationResDto>> Handle(GetAllSpecializationsQurey request, CancellationToken cancellationToken)
        {
            try
            {
                var all = await _repo.GetAllSplAsync();

                var res = all.Select(a=> new GetAllSpecializationResDto()
                {
                    Id = a.Id,
                    Category = a.Category,
                }).ToList();

                return res;
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
