using System;
using System.Collections.Generic;
using System.Linq;
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

        public DataSeeder(AppDbContext context, UserManager<Citizen> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
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
                    UserName = "bazil",
                    BirthDate = DateTime.Parse("25.04.2003") 
                },
                new()
                {
                    FirstName = "Иван",
                    LastName = "Алимский",
                    Patronymic = "Денисович",
                    Email = "vanya@mail.ru",
                    UserName = "ali",
                    BirthDate = DateTime.Parse("30.08.2003")
                }
            };
            var result = await _userManager.CreateAsync(citizens[0], "123");
            if (result.Succeeded) { }
            await _userManager.CreateAsync(citizens[1], "123");
        }
    }
}
