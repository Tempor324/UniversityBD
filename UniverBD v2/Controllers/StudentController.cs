using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniverBD_v2.Controllers
{
    [Route("students")]
    public class StudentController : ControllerBase
    {
        private UniversityContext db;
        public StudentController(UniversityContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Получает модель, на которую ссылается внешний ключ, и удаляет из списка Students ссылку на переданный экземпляр модели Student. 
        /// Нужно для избегания рекурсии в JSON-файле, отправляемом клиенту
        /// </summary>
        /// <param name="output"></param>
        [NonAction]
        private async Task PrepareOneRecordAsync(Student output)
        {
            if (output.GroupId != null)
            {
                output.Group = await db.Studentgroups.FirstOrDefaultAsync(obj => obj.GroupId == output.GroupId);
                output.Group.Students.Remove(output);
            }
        }

        [HttpGet]
        public async Task<List<Student>> Index()
        {
            return await db.Students.ToListAsync();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IResult> GetOneAsync(int id)
        {
            Student? output = await db.Students.FirstOrDefaultAsync(output => output.StudentId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            await PrepareOneRecordAsync(output);
            return Results.Json(output);
        }

        [HttpDelete]
        public async Task<IResult> DeleteAsync(int id)
        {
            Student? output = await db.Students.FirstOrDefaultAsync(output => output.StudentId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.Students.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        /// <summary>
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns></returns>
        /// <remarks>
        /// correct sample request:
        /// 
        /// {
        ///   "studentName": "string",
        ///   "groupId": "string",
        ///   "birthDate": "yyyy-mm-nn",
        /// }
        /// </remarks>
        [HttpPost]
        public async Task<Student> PostAsync(Student newRecord)
        {
            await db.Students.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;
        }

        [HttpPut]
        //[Route("{id:int}")]
        public async Task<IResult> PutAsync(Student userData)
        {
            var output = await db.Students.FirstOrDefaultAsync(output => output.StudentId == userData.StudentId);
            if (output == null)
                return Results.NotFound(new { message = "Запись не найдена" });
            output.StudentId = userData.StudentId;
            output.StudentName = userData.StudentName;
            output.GroupId = userData.GroupId;
            output.BirthDate = userData.BirthDate;
            await db.SaveChangesAsync();
            return Results.Json(output);
        }
    }
}