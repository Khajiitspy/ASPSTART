﻿using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ASPSTART.Data.Entities;
using ASPSTART.Models.Seeder;
using ASPSTART.Services;
using ASPSTART.Interfaces;
using System.Net;
using Microsoft.CodeAnalysis.Scripting;
using ASPSTART.Data.Entities.Identity;
using ASPSTART.Constants;
using Microsoft.AspNetCore.Identity;
using ASPSTART.SMTP;

namespace ASPSTART.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ASPSTARTDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var SMTPService = scope.ServiceProvider.GetRequiredService<ISMTPService>();

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
                        for (int i = 0; i < categoryEntities.Count; i++)
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
            if(!context.Roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
                var admin = new RoleEntity { Name = Roles.Admin };
                var result = await roleManager.CreateAsync(admin);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Role {Roles.Admin} Created");
                }
                else
                {
                    Console.WriteLine($"Role Creation Failed:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }

                var user = new RoleEntity { Name = Roles.User };
                result = await roleManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Role {Roles.User} Created");
                }
                else
                {
                    Console.WriteLine($"Role Creation Failed:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }

            }
            if (!context.Users.Any())
            {
                string email = "admin@gmail.com";
                var user = new UserEntity
                {
                    UserName = email,
                    Email = email,
                    LastName = "Abyss",
                    FirstName = "Magic",
                };

                var result = await userManager.CreateAsync(user, "123456");
                if (result.Succeeded)
                {
                    Console.WriteLine($"User {user.FirstName}, {user.LastName} Created");
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }
                else
                {
                    Console.WriteLine($"User Creation Failed:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }
            }

            if (!context.Products.Any())
            {
                //var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Products.json");

                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var products = JsonSerializer.Deserialize<List<SeederProductModel>>(jsonData);

                        foreach (var product in products)
                        {
                            // Знайти відповідну категорію
                            var category = await context.Categories
                                .FirstOrDefaultAsync(c => c.Name == product.CategoryName);

                            if (category == null)
                            {
                                Console.WriteLine($"Category '{product.CategoryName}' not found for product '{product.Name}'");
                                continue;
                            }

                            var productEntity = new ProductEntity
                            {
                                Name = product.Name,
                                Description = product.Description,
                                CategoryId = category.Id,
                                ProductImages = new List<ProductImageEntity>()
                            };

                            int priority = 0;
                            try
                            {
                                foreach (var imageUrl in product.Images)
                                {

                                    var savedImageUrl = await imageService.SaveImageFromUrlAsync(imageUrl);
                                    productEntity.ProductImages.Add(new ProductImageEntity
                                    {
                                        Name = savedImageUrl,
                                        Priotity = priority++
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("-----Error Add {0}------", ex.Message);
                                continue;
                            }

                            await context.Products.AddAsync(productEntity);
                        }

                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Json Parse Product Data: {0}", ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Products.json file not found");
                }
            }


        }
    }
}