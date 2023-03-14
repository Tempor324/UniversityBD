using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniverBD_v2;

public partial class UniversityContext : DbContext
{
    public UniversityContext()
    {
    }

    public UniversityContext(DbContextOptions<UniversityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Studentgroup> Studentgroups { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<WeekSchedule> WeekSchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=remote;password=123;database=university", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.HasKey(e => e.DisciplineId).HasName("PRIMARY");

            entity.ToTable("disciplines");

            entity.Property(e => e.DisciplineId)
                .HasMaxLength(5)
                .HasColumnName("disciplineId");
            entity.Property(e => e.DisciplineName)
                .HasMaxLength(60)
                .HasColumnName("disciplineName");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PRIMARY");

            entity.ToTable("students");

            entity.HasIndex(e => e.GroupId, "groupId");

            entity.Property(e => e.StudentId).HasColumnName("studentId");
            entity.Property(e => e.BirthDate).HasColumnName("birthDate");
            entity.Property(e => e.GroupId)
                .HasMaxLength(15)
                .HasColumnName("groupId");
            entity.Property(e => e.StudentName)
                .HasMaxLength(30)
                .HasColumnName("studentName");

            entity.HasOne(d => d.Group).WithMany(p => p.Students)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("students_ibfk_1");
        });

        modelBuilder.Entity<Studentgroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PRIMARY");

            entity.ToTable("studentgroups");

            entity.HasIndex(e => e.TeacherId, "teacherId");

            entity.Property(e => e.GroupId)
                .HasMaxLength(15)
                .HasColumnName("groupId");
            entity.Property(e => e.Specialization)
                .HasMaxLength(40)
                .HasColumnName("specialization");
            entity.Property(e => e.TeacherId).HasColumnName("teacherId");
            entity.Property(e => e.YearNum).HasColumnName("yearNum");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Studentgroups)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("studentgroups_ibfk_1");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PRIMARY");

            entity.ToTable("teachers");

            entity.Property(e => e.TeacherId).HasColumnName("teacherId");
            entity.Property(e => e.TeacherName)
                .HasMaxLength(30)
                .HasColumnName("teacherName");

            entity.HasMany(d => d.Disciplines).WithMany(p => p.Teachers)
                .UsingEntity<Dictionary<string, object>>(
                    "Teacherdiscipline",
                    r => r.HasOne<Discipline>().WithMany()
                        .HasForeignKey("DisciplineId")
                        .HasConstraintName("teacherdisciplines_ibfk_2"),
                    l => l.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("teacherdisciplines_ibfk_1"),
                    j =>
                    {
                        j.HasKey("TeacherId", "DisciplineId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("teacherdisciplines");
                        j.HasIndex(new[] { "DisciplineId" }, "disciplineId");
                        j.IndexerProperty<int>("TeacherId").HasColumnName("teacherId");
                        j.IndexerProperty<string>("DisciplineId")
                            .HasMaxLength(5)
                            .HasColumnName("disciplineId");
                    });
        });

        modelBuilder.Entity<WeekSchedule>(entity =>
        {
            entity.HasKey(e => new { e.DayNum, e.Lesson })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("week_shedule");

            entity.HasIndex(e => e.GroupId, "week_shedule_FK");

            entity.HasIndex(e => e.DisciplineId, "week_shedule_FK_1");

            entity.HasIndex(e => e.TeacherId, "week_shedule_FK_2");

            entity.Property(e => e.DayNum).HasColumnName("dayNum");
            entity.Property(e => e.Lesson).HasColumnName("lesson");
            entity.Property(e => e.DisciplineId)
                .HasMaxLength(5)
                .HasColumnName("disciplineId");
            entity.Property(e => e.GroupId)
                .HasMaxLength(15)
                .HasColumnName("groupId");
            entity.Property(e => e.TeacherId).HasColumnName("teacherId");

            entity.HasOne(d => d.Discipline).WithMany(p => p.WeekSchedules)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("week_shedule_FK_1");

            entity.HasOne(d => d.Group).WithMany(p => p.WeekSchedules)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("week_shedule_FK");

            entity.HasOne(d => d.Teacher).WithMany(p => p.WeekSchedules)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("week_shedule_FK_2");
        });
        
    ////test. Warning: may destroy BD
    //    modelBuilder.Entity<Teacher>().HasData(
    //        new Teacher { TeacherId = 1, TeacherName = "Тамара Петрова Евгеньевна" },
    //        new Teacher { TeacherId = 2, TeacherName = "Евгений Петров Евгеньевич" }
    //        );

    //    modelBuilder.Entity<Discipline>().HasData(
    //        new Discipline { DisciplineId = "1.1", DisciplineName = "Алгебра" },
    //        new Discipline { DisciplineId = "1.2", DisciplineName = "Геометрия" },
    //        new Discipline { DisciplineId = "2.1", DisciplineName = "Русский язык" }
    //        );

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
