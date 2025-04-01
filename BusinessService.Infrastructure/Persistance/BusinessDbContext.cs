using BusinessService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace BusinessService.Infrastructure.Persistance
{
    public class BusinessDbContext : DbContext
    {
        public BusinessDbContext(DbContextOptions<BusinessDbContext> options): base(options) { }

        public DbSet<Specialization_doctor> Specializations { get; set; }
        public DbSet<DrAvailability> DrAvailabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
