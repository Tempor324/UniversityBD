using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

namespace UniverBD_v2
{
    public class UniversityDataBase
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

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

            //app.MapGet("/d", () => "hi");

            app.MapGet("/students", async (UniversityContext db) => await db.Students.ToListAsync());
            app.MapGet("/Disciplines", async (UniversityContext db) => await db.Disciplines.ToListAsync());
            app.MapGet("/Studentgroups", async (UniversityContext db) => await db.Studentgroups.ToListAsync());
            app.MapGet("/Teachers", async (UniversityContext db) => await db.Teachers.ToListAsync());
            app.MapGet("/Schedule", async (UniversityContext db) => await db.WeekSchedules.ToListAsync());

            app.MapGet("/students/{id:int}", Handler.StudentGetHandlerAsync);
            app.MapGet("/Disciplines/{id}", Handler.DisciplineGetHandlerAsync);
            app.MapGet("/Studentgroups/{id}", Handler.StudentgroupGetHandlerAsync);
            app.MapGet("/Teachers/{id:int}", Handler.TeacherGetHandlerAsync);
            app.MapGet("/Schedule/{id:int}", Handler.ScheduleGetHandlerAsync);

            app.MapDelete("/students/{id:int}", Handler.StudentDeleteHandlerAsync);
            app.MapDelete("/Disciplines/{id}", Handler.DisciplineDeleteHandlerAsync);
            app.MapDelete("/Studentgroups/{id}", Handler.StudentgroupDeleteHandlerAsync);
            app.MapDelete("/Teachers/{id:int}", Handler.TeacherDeleteHandlerAsync);
            app.MapDelete("/Schedule/{id:int}", Handler.ScheduleDeleteHandlerAsync);

            app.MapPost("/students", Handler.StudentPostHandlerAsync);
            app.MapPost("/Disciplines",Handler.DisciplinePostHandlerAsync);
            app.MapPost("/Studentgroups", Handler.StudentgroupPostHandlerAsync);
            app.MapPost("/Teachers", Handler.TeacherPostHandlerAsync);
            app.MapPost("/Schedule", Handler.WeekSchedulePostHandlerAsync);

            app.MapPut("/students", Handler.StudentPutHandlerAsync);
            app.MapPut("/Disciplines", Handler.DisciplinePutHandlerAsync);
            app.MapPut("/Studentgroups", Handler.StudentgroupPutHandlerAsync);
            app.MapPut("/Teachers", Handler.TeacherPutHandlerAsync);
            app.MapPut("/Schedule", Handler.WeekSchedulePutHandlerAsync);
            
            app.Run(); 
        }
    }
}