using AgriEnergyConnect_ST10290044_Part2.Data;
using AgriEnergyConnect_ST10290044_Part2.Models;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect_ST10290044_Part2.Services
{
    public class SeedService
    {

        
            public static async Task SeedDatabase(IServiceProvider serviceProvider)
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();

                try
                {
                    // Ensure the database is ready
                    logger.LogInformation("Ensuring the database is created.");
                    await context.Database.EnsureCreatedAsync();

                    // Add roles
                    logger.LogInformation("Seeding roles.");
                    await AddRoleAsync(roleManager, "Farmer");
                    await AddRoleAsync(roleManager, "Employee");

                    // Add farmer user
                    logger.LogInformation("Seeding farmer user.");
                    var FarmerEmail = "farmer1@gmail.com";
                    if (await userManager.FindByEmailAsync(FarmerEmail) == null)
                    {
                        var farmerUser = new Users
                        {
                            FullName = "Farmer",
                            UserName = FarmerEmail,
                            NormalizedUserName = FarmerEmail.ToUpper(),
                            Email = FarmerEmail,
                            NormalizedEmail = FarmerEmail.ToUpper(),
                            EmailConfirmed = true,
                            SecurityStamp = Guid.NewGuid().ToString()
                        };

                        var result = await userManager.CreateAsync(farmerUser, "Farmer@1234");
                        if (result.Succeeded)
                        {
                            logger.LogInformation("Assigning farmer role to the farmer user.");
                            await userManager.AddToRoleAsync(farmerUser, "Farmer");
                        }
                        else
                        {
                            logger.LogError("Failed to create farmer user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");

                }

            }

            private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

    }

