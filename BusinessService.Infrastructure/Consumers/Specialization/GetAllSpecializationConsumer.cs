using BusinessService.Infrastructure.Persistance;
using Contarcts.Requests.Specialization;
using Contarcts.Responses.Specialization;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Infrastructure.Consumers.Specialization
{
    public class GetAllSpecializationConsumer : IConsumer<GetAllSpecializationReq>
    {
        private readonly BusinessDbContext _businessDbContext;
        public GetAllSpecializationConsumer(BusinessDbContext businessDbContext)
        {
            _businessDbContext = businessDbContext;
        }

        public async Task Consume(ConsumeContext<GetAllSpecializationReq> context)
        {
            try
            {
                var res = await _businessDbContext.Specializations
                    .Select(a => new SpecializationResponse
                    {
                        splId = a.Id,
                        Category = a.Category,
                    })
                    .ToListAsync();

                 await context.RespondAsync(new GetAllSpecializationResponseList { specializations = res });
            }
            catch (Exception ex)
            {
                throw new Exception("Not found");
            }
        }
    }
}
