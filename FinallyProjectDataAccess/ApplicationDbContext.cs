using FinallyProjectEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinallyProjectDataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Bağlantı kısmı appsettings.json dosyasında ekli 
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationUser> applicationUser { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<PImages> PImages { get; set; }
        public DbSet<userCart> userCarts {  get; set; }
    }
}
