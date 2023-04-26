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
    internal class AppDbContext: IdentityDbContext<Citizen>
    {
        DbSet<Citizen> Citizens => Set<Citizen>();

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
    }
}
