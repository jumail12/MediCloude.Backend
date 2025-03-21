using BusinessService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace BusinessService.Infrastructure.Persistance
{
    public class BusinessDbContext : DbContext
    {
        public BusinessDbContext(DbContextOptions<BusinessDbContext> options): base(options) { }

        public DbSet<Specialization_doctor> Specializations { get; set; }

    }
}
