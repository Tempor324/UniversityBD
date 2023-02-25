using System;
using System.Collections.Generic;

namespace UniverBD_v2;

public partial class Student
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = null!;

    public string? GroupId { get; set; }

    /// <summary>
    /// при отправке put и post-запроса для свойства BirthDate достаточно указывать строку в формате "yyyy-mm-dd"
    /// </summary>
    public DateOnly? BirthDate { get; set; }

    public virtual Studentgroup? Group { get; set; }
}
