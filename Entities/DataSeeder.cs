using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Citizen> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(AppDbContext context, UserManager<Citizen> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedUsers()
        {
            if (_userManager.Users.Any()) return;
            var citizens = new List<Citizen>
            {
                new()
                {
                    FirstName = "Василий",
                    LastName = "Раздорский",
                    Patronymic = "Юрьевич",
                    Email = "vasiliy@mail.ru",
                    UserName = "vasiliy@mail.ru",
                    BirthDate = DateTime.Parse("25.04.2003")
                },
                new()
                {
                    FirstName = "Иван",
                    LastName = "Алимский",
                    Patronymic = "Денисович",
                    Email = "vanya@mail.ru",
                    UserName = "vanya@mail.ru",
                    BirthDate = DateTime.Parse("30.08.2003")
                }
            };
            await _userManager.CreateAsync(citizens[0], "123");
            await _userManager.AddToRolesAsync(citizens[0], roles: new List<string> { "user", "admin" });
            await _userManager.CreateAsync(citizens[1], "123");
            await _userManager.AddToRolesAsync(citizens[1], roles: new List<string> { "user", "admin" });
        }

        public async Task SeedRoles()
        {
            if (_roleManager.Roles.Any()) return;
            await _roleManager.CreateAsync(new IdentityRole("user"));
            await _roleManager.CreateAsync(new IdentityRole("admin"));
        }
        
        public async Task SeedAsync()
        {
            await SeedRoles();
            await SeedUsers();
        }
    }
}
