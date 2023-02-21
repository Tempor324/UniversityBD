using Microsoft.EntityFrameworkCore;
using System;

namespace UniverBD_v2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<UniversityContext>(options => options.UseMySql(connection,
                new MySqlServerVersion(new Version(8, 0, 32))));

            var app = builder.Build();

            //// Configure the HTTP request pipeline.
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


            app.MapGet("/students/{id:int}", async (int id, UniversityContext db) =>
            {
                Student? output = await db.Students.FirstOrDefaultAsync(output => output.StudentId == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                return Results.Json(output);
            });
            app.MapGet("/Disciplines/{id}", async (string id, UniversityContext db) =>
            {
                Discipline? output = await db.Disciplines.FirstOrDefaultAsync(output => output.DisciplineId == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                return Results.Json(output);
            });
            app.MapGet("/Studentgroups/{id}", async (string id, UniversityContext db) =>
            {
                Studentgroup? output = await db.Studentgroups.FirstOrDefaultAsync(output => output.GroupId == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                return Results.Json(output);
            });
            app.MapGet("/Teachers/{id:int}", async (int id, UniversityContext db) =>
            {
                Teacher? output = await db.Teachers.FirstOrDefaultAsync(output => output.TeacherId == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                return Results.Json(output);
            });
            app.MapGet("/Schedule/{id:int}", async (int id, UniversityContext db) =>
            {
                WeekSchedule? output = await db.WeekSchedules.FirstOrDefaultAsync(output => output.DayNum == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                return Results.Json(output);
            });


            app.MapDelete("/students/{id:int}", async (int id, UniversityContext db) =>
            {
                Student? output = await db.Students.FirstOrDefaultAsync(output => output.StudentId == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                db.Students.Remove(output);
                await db.SaveChangesAsync();
                return Results.Json(output);
            });
            app.MapDelete("/Disciplines/{id}", async (string id, UniversityContext db) =>
            {
                Discipline? output = await db.Disciplines.FirstOrDefaultAsync(output => output.DisciplineId == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                db.Disciplines.Remove(output);
                await db.SaveChangesAsync();
                return Results.Json(output);
            });
            app.MapDelete("/Studentgroups/{id}", async (string id, UniversityContext db) =>
            {
                Studentgroup? output = await db.Studentgroups.FirstOrDefaultAsync(output => output.GroupId == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                db.Studentgroups.Remove(output);
                await db.SaveChangesAsync();
                return Results.Json(output);
            });
            app.MapDelete("/Teachers/{id:int}", async (int id, UniversityContext db) =>
            {
                Teacher? output = await db.Teachers.FirstOrDefaultAsync(output => output.TeacherId == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                db.Teachers.Remove(output);
                await db.SaveChangesAsync();
                return Results.Json(output);
            });
            app.MapDelete("/Schedule/{id:int}", async (int id, UniversityContext db) =>
            {
                WeekSchedule? output = await db.WeekSchedules.FirstOrDefaultAsync(output => output.DayNum == id);
                if (output == null)
                    return Results.NotFound(new { message = "запись отсутствует" });
                db.WeekSchedules.Remove(output);
                await db.SaveChangesAsync();
                return Results.Json(output);
            });


            app.MapPost("/students", async (Student newRecord, UniversityContext db) =>
            {
                await db.Students.AddAsync(newRecord);
                await db.SaveChangesAsync();
                return newRecord;
            });
            app.MapPost("/Disciplines", async (Discipline newRecord, UniversityContext db) =>
            {
                await db.Disciplines.AddAsync(newRecord);
                await db.SaveChangesAsync();
                return newRecord;
            });
            app.MapPost("/Studentgroups", async (Studentgroup newRecord, UniversityContext db) =>
            {
                await db.Studentgroups.AddAsync(newRecord);
                await db.SaveChangesAsync();
                return newRecord;
            });
            app.MapPost("/Teachers", async (Teacher newRecord, UniversityContext db) =>
            {
                await db.Teachers.AddAsync(newRecord);
                await db.SaveChangesAsync();
                return newRecord;
            });
            app.MapPost("/Schedule", async (WeekSchedule newRecord, UniversityContext db) =>
            {
                await db.WeekSchedules.AddAsync(newRecord);
                await db.SaveChangesAsync();
                return newRecord;
            });


            app.MapPut("/students", async (Student userData, UniversityContext db) =>
            {
                var output = await db.Students.FirstOrDefaultAsync(output => output.StudentId == userData.StudentId);
                if (output == null)
                    return Results.NotFound(new { message = "Пользователь не найден" });
                output.StudentId = userData.StudentId;
                output.StudentName = userData.StudentName;
                output.GroupId = userData.GroupId;
                output.BirthDate = userData.BirthDate;
                await db.SaveChangesAsync();
                return Results.Json(output);
            });
            app.MapPut("/Disciplines", async (Discipline userData, UniversityContext db) =>
            {
                var output = await db.Disciplines.FirstOrDefaultAsync(output => output.DisciplineId == userData.DisciplineId);
                if (output == null)
                    return Results.NotFound(new { message = "Пользователь не найден" });
                output.DisciplineId = userData.DisciplineId;
                output.DisciplineName = userData.DisciplineName;
                await db.SaveChangesAsync();
                return Results.Json(output);
            });
            app.MapPut("/Studentgroups", async (Studentgroup userData, UniversityContext db) =>
            {
                var output = await db.Studentgroups.FirstOrDefaultAsync(output => output.GroupId == userData.GroupId);
                if (output == null)
                    return Results.NotFound(new { message = "Пользователь не найден" });
                output.GroupId = userData.GroupId;
                output.YearNum = userData.YearNum;
                output.Specialization = userData.Specialization;
                output.TeacherId = userData.TeacherId;
                await db.SaveChangesAsync();
                return Results.Json(output);
            });
            app.MapPut("/Teachers", async (Teacher userData, UniversityContext db) =>
            {
                var output = await db.Teachers.FirstOrDefaultAsync(output => output.TeacherId == userData.TeacherId);
                if (output == null)
                    return Results.NotFound(new { message = "Пользователь не найден" });
                output.TeacherId = userData.TeacherId;
                output.TeacherName = userData.TeacherName;
                await db.SaveChangesAsync();
                return Results.Json(output);
            });
            app.MapPut("/Schedule", async (WeekSchedule userData, UniversityContext db) =>
            {
                var output = await db.WeekSchedules.FirstOrDefaultAsync(output => output.DayNum == userData.DayNum);
                if (output == null)
                    return Results.NotFound(new { message = "Пользователь не найден" });
                output.DayNum = userData.DayNum;
                output.Lesson = userData.Lesson;
                output.GroupId = userData.GroupId;
                output.DisciplineId = userData.DisciplineId;
                output.TeacherId = userData.TeacherId;
                await db.SaveChangesAsync();
                return Results.Json(output);
            });
            //put-запрос нужно проработать. Может, и другие стоит пересмотреть.

            app.Run();
        }
    }
}