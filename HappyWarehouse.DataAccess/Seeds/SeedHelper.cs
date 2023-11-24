using HappyWarehouse.DataAccess.DataContext;
using HappyWarehouse.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace HappyWarehouse.DataAccess.Seeds
{
    public class SeedHelper
    {
        private readonly HappyWarehouseDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SeedHelper(HappyWarehouseDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _dbContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitialiseAsync()
        {
            if (_dbContext.Database.IsSqlite())
            {
                await _dbContext.Database.MigrateAsync();
            }
            else
            {
                await _dbContext.Database.EnsureCreatedAsync();
            }
        }

        public async Task SeedAsync()
        {
            await SeedIdentityAsync();
            await SeedCountries();
        }

        private async Task SeedIdentityAsync()
        {
            var roles = await _roleManager.Roles.Select(r=> r.Name).ToListAsync();

            if (!roles.Contains("Admin"))
            {
                await _roleManager.CreateAsync(
                    new ApplicationRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        Id = 1
                    });
            }

            if (!roles.Contains("Management"))
            {
                await _roleManager.CreateAsync(
                    new ApplicationRole
                    {
                        Name = "Management",
                        NormalizedName = "MANAGEMENT",
                        Id = 2
                    }); 
            }

            if (!roles.Contains("Auditor"))
            {
                await _roleManager.CreateAsync(
                    new ApplicationRole
                    {
                        Name = "Auditor",
                        NormalizedName = "AUDITOR",
                        Id = 3
                    }); 
            }

            var user = await _userManager.FindByEmailAsync("admin@happywarehouse.com");

            if (user == null)
            {
                var adminUserName = "admin@happywarehouse.com";
                var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName, FullName = "Admin", Id = 1 };

                await _userManager.CreateAsync(adminUser, "“P@ssw0rd");
                adminUser = await _userManager.FindByNameAsync(adminUserName);
                await _userManager.AddToRoleAsync(adminUser!, "Admin");
            }

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task SeedCountries()
        {
            var seedCountries = GetSeedData();
            var existingCountries = _dbContext.Countries.AsNoTracking().Select(c => c.Name).ToHashSet();
            var countries = new List<Country>();

            foreach (var country in seedCountries)
            {
                if (existingCountries.Contains(country.Name))
                {
                    continue;
                }

                countries.Add(new Country() { Name = country.Name });
            }

            await _dbContext.Countries.AddRangeAsync(countries);
            _dbContext.SaveChanges();
        }

        private static IEnumerable<SeedCountry> GetSeedData()
        {
            var path = @"Seeds/Data/countries.json";
            var fileContent = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path));

            return JObject.Parse(fileContent)
                .SelectToken("country_state")
                .ToObject<IEnumerable<SeedCountry>>();
        }
    }

    class SeedCountry
    {
        public string Name { get; set; }
    }
}
