using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ASPSTART.Data.Entities;
using ASPSTART.Models.Seeder;
using ASPSTART.Services;
using ASPSTART.Interfaces;
using System.Net;
using Microsoft.CodeAnalysis.Scripting;

namespace ASPSTART.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ASPSTARTDbContext>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();

            context.Database.Migrate();

            if (!context.Categories.Any())
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var categories = JsonSerializer.Deserialize<List<SeederCateModel>>(jsonData);
                        var categoryEntities = mapper.Map<List<CateEntity>>(categories);
                        for ( int i=0; i<categoryEntities.Count; i++)
                        {
                            using (WebClient client = new WebClient())
                            {
                                byte[] bytes = await client.DownloadDataTaskAsync(categoryEntities[i].ImageUrl);
                                categoryEntities[i].ImageUrl = await imageService.SaveImageAsync(bytes);
                            }
                        }
                        await context.AddRangeAsync(categoryEntities);
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Not Found File Categories.json");
                }
            }
            if (!context.Users.Any())
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var users = JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);
                        var userEntities = mapper.Map<List<UserEntity>>(users);

                        foreach (var user in userEntities)
                        {
                            // Hash the password (if applicable)
                            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                            // Process avatar image
                            if (!string.IsNullOrEmpty(user.Avatar))
                            {
                                using (WebClient client = new WebClient())
                                {
                                    byte[] bytes = await client.DownloadDataTaskAsync(user.Avatar);
                                    user.Avatar = await imageService.SaveImageAsync(bytes);
                                }
                            }
                        }

                        await context.AddRangeAsync(userEntities);
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error parsing Users.json: {0}", ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Users.json file not found.");
                }
            }
        }

    }
}
