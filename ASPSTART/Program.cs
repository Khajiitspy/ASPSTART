using ASPSTART.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ASPSTARTDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MyConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cate}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.SeedData();

app.Run();

//using Microsoft.EntityFrameworkCore;
//using ASPSTART.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddDbContext<ASPSTARTDbContext>(opt =>
//    opt.UseNpgsql(builder.Configuration.GetConnectionString("MyConnection")));

//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
//app.UseRouting();

//app.UseAuthorization(); 

//app.MapStaticAssets(); 
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Categories}/{action=Index}/{id?}")
//    .WithStaticAssets();

//await app.SeedData();

//app.Run();

