using System;
using System.Collections.Generic;

namespace UniverBD_v2;

public partial class Studentgroup
{
    public string GroupId { get; set; } = null!;

    public int? YearNum { get; set; }

    public string? Specialization { get; set; }

    public int? TeacherId { get; set; }

    public virtual ICollection<Student> Students { get; } = new List<Student>();

    public virtual Teacher? Teacher { get; set; }

    public virtual ICollection<WeekSchedule> WeekSchedules { get; } = new List<WeekSchedule>();
}
