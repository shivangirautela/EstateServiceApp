using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstateServiceApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EstateServiceApp.Data
{
    public class PropertyDbContext : IdentityDbContext<ApplicationUser>
    {
        public PropertyDbContext(DbContextOptions<PropertyDbContext> options)
            : base(options)
        {
        }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyGallery> PropertyGallery { get; set; }
        public DbSet<UserActivity> UserActivites { get; set; }
        public DbSet<FeaturesModel> Features { get; set; }
    }
}
