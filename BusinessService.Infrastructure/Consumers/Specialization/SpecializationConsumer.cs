using BusinessService.Infrastructure.Persistance;
using Contarcts.Requests.Specialization;
using Contarcts.Responses.Specialization;
using MassTransit;
using Microsoft.EntityFrameworkCore;


namespace BusinessService.Infrastructure.Consumers.Specialization
{
    public class SpecializationConsumer : IConsumer<SpecializationExistsReq>
    {
        private readonly BusinessDbContext _businessDbContext;
        public SpecializationConsumer(BusinessDbContext businessDbContext)
        {
            _businessDbContext = businessDbContext;
        }

        public async Task Consume(ConsumeContext<SpecializationExistsReq> context)
        {
            try
            {
                bool IsExistsSpl = await _businessDbContext.Specializations.AnyAsync(a=>a.Id==context.Message.id);
                await context.RespondAsync(new SpecializationExistsResponse { Exists=IsExistsSpl});
            }
            catch (Exception ex)
            {
                throw new Exception("Not found");
            }
        }
    }
}
