using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebGenQRCode.Data;
using WebGenQRCode.Data.Entities.Identity;
using WebGenQRCode.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppQrDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("QrConnection"))); //реєструє контекст бази даних (DbContext) у контейнері залежностей

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<AppQrDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddSwaggerGen(); //додаємо swagger - кажемо що він є

builder.Services.AddControllers();

const string reactCorsPolicy = "ReactClient";

var reactCorsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(reactCorsPolicy, policy =>
    {
        policy.WithOrigins(reactCorsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors(reactCorsPolicy); //дозволяємо використання cors правил

app.UseSwagger(); //використай swagger
app.UseSwaggerUI(); //додай граф. інтерфейс

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

await app.SeedData();

app.Run();
