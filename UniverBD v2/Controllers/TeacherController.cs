using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniverBD_v2.Controllers
{
    [Route("teacher")]
    public class TeacherController : ControllerBase
    {
        private UniversityContext db;
        public TeacherController(UniversityContext db)
        {
            this.db = db;
        }

        [NonAction]
        public async Task PrepareManyRecordsAsync(Teacher output)
        {
            var groups = await db.Studentgroups.Include(p => p.Teacher).Where(p => p.TeacherId == output.TeacherId).ToListAsync();
            foreach (Studentgroup group in groups)
            {
                group.Teacher = null;
                //output.Studentgroups.Add(group);
            }
            //var disciplines = db.Disciplines.Include(p => p.Teachers).Where(p => p.TeacherId == id).ToList();
            //foreach (Studentgroup group in groups)
            //{
            //    group.Teacher = null;
            //    //output.Studentgroups.Add(group);
            //}
            //пока трогать не буду. Может, есть какие-то уже готовые механизмы для работы со связью "многие-ко-многим"
        }

        [HttpGet]
        public async Task<List<Teacher>> Index()
        {
            return await db.Teachers.ToListAsync();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IResult> GetOneAsync(int id)
        {
            Teacher? output = await db.Teachers.FirstOrDefaultAsync(output => output.TeacherId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            await PrepareManyRecordsAsync(output);
            return Results.Json(output);
        }

        [HttpDelete]
        public async Task<IResult> DeleteAsync(int id)
        {
            Teacher? output = await db.Teachers.FirstOrDefaultAsync(output => output.TeacherId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.Teachers.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        [HttpPost]
        public async Task<Teacher> PostAsync(Teacher newRecord)
        {
            await db.Teachers.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;
        }

        [HttpPut]
        //[Route("{id:int}")]
        public async Task<IResult> PutAsync(Teacher userData)
        {
            var output = await db.Teachers.FirstOrDefaultAsync(output => output.TeacherId == userData.TeacherId);
            if (output == null)
                return Results.NotFound(new { message = "Пользователь не найден" });
            output.TeacherId = userData.TeacherId;
            output.TeacherName = userData.TeacherName;
            await db.SaveChangesAsync();
            return Results.Json(output);
        }
    }
}
