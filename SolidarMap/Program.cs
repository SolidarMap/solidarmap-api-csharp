using SolidarMap.Connection;
using Oracle.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Global Solution - SolidarMap - Sistema de Ajuda Colaborativa em Crises",
        Version = "v1",
        Description = "O SolidarMap � um sistema que conecta pessoas em situa��es de emerg�ncia com recursos essenciais como comida, abrigo, rem�dios e �gua. A plataforma utiliza localiza��o geogr�fica, comunica��o direta e avalia��es para promover a confian�a e agilidade em momentos cr�ticos.",
        Contact = new OpenApiContact
        {
            Name = "Pedro Henrique Vasco Antonieti - RM: 556253"
        }
    });

    // Inclui os coment�rios XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});



builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();