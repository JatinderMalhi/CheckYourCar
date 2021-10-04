using CheckYourCar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//db
namespace CheckYourCar.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CarMake> CarsMake { get; set; }

        public DbSet<CarModel> CarsModel { get; set; }
        public DbSet<CarsRecall> CarsRecalls { get; set; }
        public DbSet<CarUsers> CarUsers { get; set; }

    }
}
