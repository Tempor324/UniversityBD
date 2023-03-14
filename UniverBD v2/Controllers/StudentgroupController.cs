using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniverBD_v2.Controllers
{
    [Route("Studentgroups")]
    public class StudentgroupController : ControllerBase
    {
        private UniversityContext db;
        public StudentgroupController(UniversityContext db)
        {
            this.db = db;
        }

        [NonAction]
        public async Task PrepareOneRecordAsync(Studentgroup output)
        {
            if (output.TeacherId != null)
            {
                output.Teacher = await db.Teachers.FirstOrDefaultAsync(obj => obj.TeacherId == output.TeacherId);
                output.Teacher.Studentgroups.Remove(output);
            }
        }

        [NonAction]
        public async Task PrepareManyRecordsAsync(Studentgroup output)
        {
            var sts = await db.Students.Include(p => p.Group).Where(p => p.GroupId == output.GroupId).ToListAsync();
            foreach (Student student in sts)// AddRange не работает, приходится добавлять перебором
            {
                student.Group = null;
                //output.Students.Add(student); добавляются на этапе загрузки из БД
            }//работает, но реализация - такой себе костыль...
        }

        [HttpGet]
        public async Task<List<Studentgroup>> Index()
        {
            return await db.Studentgroups.ToListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IResult> GetOneAsync(string id)
        {
            Studentgroup? output = await db.Studentgroups.FirstOrDefaultAsync(output => output.GroupId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            await PrepareManyRecordsAsync(output);
            await PrepareOneRecordAsync(output);
            return Results.Json(output);
        }

        [HttpDelete]
        public async Task<IResult> DeleteAsync(string id)
        {
            Studentgroup? output = await db.Studentgroups.FirstOrDefaultAsync(output => output.GroupId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.Studentgroups.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        [HttpPost]
        public async Task<Studentgroup> PostAsync(Studentgroup newRecord)
        {
            await db.Studentgroups.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;
        }

        [HttpPut]
        //[Route("{id}")]
        public async Task<IResult> PutAsync(Studentgroup userData)
        {
            var output = await db.Studentgroups.FirstOrDefaultAsync(output => output.GroupId == userData.GroupId);
            if (output == null)
                return Results.NotFound(new { message = "запись не найдена" });
            output.GroupId = userData.GroupId;
            output.YearNum = userData.YearNum;
            output.Specialization = userData.Specialization;
            output.TeacherId = userData.TeacherId;
            await db.SaveChangesAsync();
            return Results.Json(output);
        }
    }
}