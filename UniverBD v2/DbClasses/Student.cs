using System;
using System.Collections.Generic;

namespace UniverBD_v2;

public partial class Student
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = null!;

    public string? GroupId { get; set; }

    public DateOnly? BirthDate { get; set; }

    public virtual Studentgroup? Group { get; set; }
}
