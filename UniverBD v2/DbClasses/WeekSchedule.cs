using System;
using System.Collections.Generic;

namespace UniverBD_v2;

public partial class WeekSchedule
{
    public int DayNum { get; set; }

    public int Lesson { get; set; }

    public string? GroupId { get; set; }

    public string? DisciplineId { get; set; }

    public int? TeacherId { get; set; }

    public virtual Discipline? Discipline { get; set; }

    public virtual Studentgroup? Group { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
