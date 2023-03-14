using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniverBD_v2.Controllers
{
    [Route("schedule")]
    public class WeekScheduleController : ControllerBase // revise implementation
    {
        private UniversityContext db;
        public WeekScheduleController(UniversityContext db)
        {
            this.db = db;
        }

        [NonAction]
        public async Task PrepareOneRecordAsync(WeekSchedule output)
        {
            if (output.GroupId != null)
            {
                output.Group = await db.Studentgroups.FirstOrDefaultAsync(obj => obj.GroupId == output.GroupId);
                output.Group.WeekSchedules.Remove(output);
            }
            if (output.DisciplineId != null)
            {
                output.Discipline = await db.Disciplines.FirstOrDefaultAsync(obj => obj.DisciplineId == output.DisciplineId);
                output.Discipline.WeekSchedules.Remove(output);
            }
            if (output.TeacherId != null)
            {
                output.Teacher = await db.Teachers.FirstOrDefaultAsync(obj => obj.TeacherId == output.TeacherId);
                output.Teacher.WeekSchedules.Remove(output);
            }
        }

        [HttpGet]
        public async Task<List<WeekSchedule>> Index()
        {
            return await db.WeekSchedules.ToListAsync();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IResult> GetOneAsync(int id)
        {
            WeekSchedule? output = await db.WeekSchedules.FirstOrDefaultAsync(output => output.DayNum == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            return Results.Json(output);
        }

        [HttpDelete]
        public async Task<IResult> DeleteAsync(int num)
        {
            WeekSchedule? output = await db.WeekSchedules.FirstOrDefaultAsync(output => output.DayNum == num);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.WeekSchedules.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        [HttpPost]
        public async Task<WeekSchedule> PostAsync(WeekSchedule newRecord)
        {
            await db.WeekSchedules.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;
        }

        [HttpPut]
        //[Route("{id:int}")]
        public async Task<IResult> PutAsync(WeekSchedule userData)
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
        }
    }
}
