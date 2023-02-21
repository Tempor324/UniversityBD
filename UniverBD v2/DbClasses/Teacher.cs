using System;
using System.Collections.Generic;

namespace UniverBD_v2;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string TeacherName { get; set; } = null!;

    public virtual ICollection<Studentgroup> Studentgroups { get; } = new List<Studentgroup>();

    public virtual ICollection<WeekSchedule> WeekSchedules { get; } = new List<WeekSchedule>();

    public virtual ICollection<Discipline> Disciplines { get; } = new List<Discipline>();
}
