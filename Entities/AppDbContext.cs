using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class AppDbContext: IdentityDbContext<Citizen>
    {
        public DbSet<Event> Events { get; set; } = null!;

        public AppDbContext()
        {
            
        }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            //Database.EnsureCreated();
        }

    }
}
