using Microsoft.EntityFrameworkCore;

namespace UniverBD_v2
{
    /// <summary>
    /// Описаны методы и делегаты для сокращения кода в методе Main, а также для обработки данных.
    /// </summary>
    public static class Handler //обработчик
    {
        /// <summary>
        /// Получает модель, на которую ссылается внешний ключ, и удаляет из списка Studentgroups ссылку на переданный экземпляр модели Studentgroup. 
        /// Нужно для избегания рекурсии в JSON-файле, отправляемом клиенту
        /// </summary>
        /// <param name="output"></param>
        /// <param name="db"></param>
        public static async Task PrepareOneRecordAsync(Studentgroup output, UniversityContext db)
        {
            if (output.TeacherId != null)
            {
                output.Teacher = await db.Teachers.FirstOrDefaultAsync(obj => obj.TeacherId == output.TeacherId);
                output.Teacher.Studentgroups.Remove(output);
            }
        }
        /// <summary>
        /// Получает модель, на которую ссылается внешний ключ, и удаляет из списка Students ссылку на переданный экземпляр модели Student. 
        /// Нужно для избегания рекурсии в JSON-файле, отправляемом клиенту
        /// </summary>
        /// <param name="output"></param>
        /// <param name="db"></param>
        public static async Task PrepareOneRecordAsync(Student output, UniversityContext db)
        {
            if (output.GroupId != null)
            {
                output.Group = await db.Studentgroups.FirstOrDefaultAsync(obj => obj.GroupId == output.GroupId);
                output.Group.Students.Remove(output);
            }
        }
        /// <summary>
        /// Получает модели, на которые ссылаются внешние ключи, и удаляет из их списков ссылку на переданный экземпляр модели WeekSchedule. 
        /// Нужно для избегания рекурсии в JSON-файле, отправляемом клиенту
        /// </summary>
        /// <param name="output"></param>
        /// <param name="db"></param>
        public static async Task PrepareOneRecordAsync(WeekSchedule output, UniversityContext db)
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
        /// <summary>
        /// Получает модели из списка ссылающихся моделей Student и в каждой модели удаляет ссылку на экземпляр StudentGroup.
        /// Нужно для избегания рекурсии в JSON-файле, отправляемом клиенту.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="db"></param>
        public static async Task PrepareManyRecordsAsync(Studentgroup output, UniversityContext db)
        {
            var sts = await db.Students.Include(p => p.Group).Where(p => p.GroupId == output.GroupId).ToListAsync();
            foreach (Student student in sts)// AddRange не работает, приходится добавлять перебором
            {
                student.Group = null;
                //output.Students.Add(student); добавляются на этапе загрузки из БД
            }//работает, но реализация - такой себе костыль...
        }
        /// <summary>
        /// Получает модели из списка ссылающихся моделей Group и в каждой модели удаляет ссылку на экземпляр Teacher.
        /// Нужно для избегания рекурсии в JSON-файле, отправляемом клиенту.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="db"></param>
        public static async Task PrepareManyRecordsAsync(Teacher output, UniversityContext db)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<IResult> StudentGetHandlerAsync(int id, UniversityContext db)
        {
            Student? output = await db.Students.FirstOrDefaultAsync(output => output.StudentId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            await PrepareOneRecordAsync(output, db);
            return Results.Json(output);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<IResult> DisciplineGetHandlerAsync(string id, UniversityContext db)
        {
            Discipline? output = await db.Disciplines.FirstOrDefaultAsync(output => output.DisciplineId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            return Results.Json(output);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<IResult> StudentgroupGetHandlerAsync(string id, UniversityContext db)
        {
            Studentgroup? output = await db.Studentgroups.FirstOrDefaultAsync(output => output.GroupId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            await PrepareManyRecordsAsync(output, db);
            await PrepareOneRecordAsync(output, db);
            return Results.Json(output);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<IResult> TeacherGetHandlerAsync(int id, UniversityContext db)
        {
            Teacher? output = await db.Teachers.FirstOrDefaultAsync(output => output.TeacherId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            await PrepareManyRecordsAsync(output, db);
            return Results.Json(output);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<IResult> ScheduleGetHandlerAsync(int id, UniversityContext db)
        {
            WeekSchedule? output = await db.WeekSchedules.FirstOrDefaultAsync(output => output.DayNum == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            return Results.Json(output);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<IResult> StudentDeleteHandlerAsync(int id, UniversityContext db)
        {
            Student? output = await db.Students.FirstOrDefaultAsync(output => output.StudentId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.Students.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        public static async Task<IResult> DisciplineDeleteHandlerAsync(string id, UniversityContext db)
        {
            Discipline? output = await db.Disciplines.FirstOrDefaultAsync(output => output.DisciplineId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.Disciplines.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        public static async Task<IResult> StudentgroupDeleteHandlerAsync(string id, UniversityContext db)
        {
            Studentgroup? output = await db.Studentgroups.FirstOrDefaultAsync(output => output.GroupId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.Studentgroups.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        public static async Task<IResult> TeacherDeleteHandlerAsync(int id, UniversityContext db)
        {
            Teacher? output = await db.Teachers.FirstOrDefaultAsync(output => output.TeacherId == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.Teachers.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        public static async Task<IResult> ScheduleDeleteHandlerAsync(int id, UniversityContext db)
        {
            WeekSchedule? output = await db.WeekSchedules.FirstOrDefaultAsync(output => output.DayNum == id);
            if (output == null)
                return Results.NotFound(new { message = "запись отсутствует" });
            db.WeekSchedules.Remove(output);
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        public static async Task<Student> StudentPostHandlerAsync(Student newRecord, UniversityContext db)
        {
            await db.Students.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;
        }

        public static async Task<Discipline> DisciplinePostHandlerAsync(Discipline newRecord, UniversityContext db)
        {
            await db.Disciplines.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;
        }

        public static async Task<Studentgroup> StudentgroupPostHandlerAsync(Studentgroup newRecord, UniversityContext db)
        {
            await db.Studentgroups.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;
        }

        public static async Task<Teacher> TeacherPostHandlerAsync(Teacher newRecord, UniversityContext db)
        {
            await db.Teachers.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;
        }

        public static async Task<WeekSchedule> WeekSchedulePostHandlerAsync(WeekSchedule newRecord, UniversityContext db)
        {
            await db.WeekSchedules.AddAsync(newRecord);
            await db.SaveChangesAsync();
            return newRecord;//только сейчас задумался: почему везде у меня IResult, а в Post-ах - голые типы?..
        }

        public static async Task<IResult> StudentPutHandlerAsync(Student userData, UniversityContext db)
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
        }

        public static async Task<IResult> DisciplinePutHandlerAsync(Discipline userData, UniversityContext db)
        {
            var output = await db.Disciplines.FirstOrDefaultAsync(output => output.DisciplineId == userData.DisciplineId);
            if (output == null)
                return Results.NotFound(new { message = "Пользователь не найден" });
            output.DisciplineId = userData.DisciplineId;
            output.DisciplineName = userData.DisciplineName;
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        public static async Task<IResult> StudentgroupPutHandlerAsync(Studentgroup userData, UniversityContext db)
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
        }

        public static async Task<IResult> TeacherPutHandlerAsync(Teacher userData, UniversityContext db)
        {
            var output = await db.Teachers.FirstOrDefaultAsync(output => output.TeacherId == userData.TeacherId);
            if (output == null)
                return Results.NotFound(new { message = "Пользователь не найден" });
            output.TeacherId = userData.TeacherId;
            output.TeacherName = userData.TeacherName;
            await db.SaveChangesAsync();
            return Results.Json(output);
        }

        public static async Task<IResult> WeekSchedulePutHandlerAsync(WeekSchedule userData, UniversityContext db)
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

        //put-запрос нужно проработать. Может, и другие стоит пересмотреть.
    }
}
