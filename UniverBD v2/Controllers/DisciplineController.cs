using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniverBD_v2.Controllers

{
    [Route("disciplines")]
    public class DisciplineController : ControllerBase
    {
        private UniversityContext db;
        public DisciplineController(UniversityContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<List<Discipline>> Index()
        {
            return await db.Disciplines.ToListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IResult> GetOneAsync(string id)
        {
            Discipline? output = await db.Disciplines.FirstOrDefaultAsync(output => output.DisciplineId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            return Results.Json(output);
        }

        [HttpDelete]
        public async Task<IResult> DeleteAsync(string id)
        {
            Discipline? output = await db.Disciplines.FirstOrDefaultAsync(output => output.DisciplineId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.Disciplines.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        [HttpPost]
        public async Task<Discipline> PostAsync(Discipline newRecord)
        {
            await db.Disciplines.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;
        }

        [HttpPut]
        //[Route("{id}")]
        public async Task<IResult> PutAsync(Discipline userData)
        {
            var output = await db.Disciplines.FirstOrDefaultAsync(output => output.DisciplineId == userData.DisciplineId);
            if (output == null)
                return Results.NotFound(new { message = "Запись не найдена" });
            output.DisciplineId = userData.DisciplineId;
            output.DisciplineName = userData.DisciplineName;
            await db.SaveChangesAsync();
            return Results.Json(output);
        }
    }
}
