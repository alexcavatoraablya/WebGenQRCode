using Microsoft.EntityFrameworkCore;
using WebGenQRCode.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppQrDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("QrConnection"))); //реєструє контекст бази даних (DbContext) у контейнері залежностей

// Add services to the container.
builder.Services.AddSwaggerGen(); //додаємо swagger - кажемо що він є

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger(); //використай swagger
app.UseSwaggerUI(); //додай граф. інтерфейс

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
