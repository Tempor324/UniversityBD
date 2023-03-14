using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using UniverBD_v2.Controllers;

namespace UniverBD_v2;

public class UniversityDataBase
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        //Use MVC
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = "APIconfigure.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename)); 
        });

        string connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<UniversityContext>(options => options.UseMySql(connection,
            new MySqlServerVersion(new Version(8, 0, 32))));

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
    }
}