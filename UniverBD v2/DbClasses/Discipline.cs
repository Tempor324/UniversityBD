using System;
using System.Collections.Generic;

namespace UniverBD_v2;

public partial class Discipline
{
    public string DisciplineId { get; set; } = null!;

    public string DisciplineName { get; set; } = null!;

    public virtual ICollection<WeekSchedule> WeekSchedules { get; } = new List<WeekSchedule>();

    public virtual ICollection<Teacher> Teachers { get; } = new List<Teacher>();
}
