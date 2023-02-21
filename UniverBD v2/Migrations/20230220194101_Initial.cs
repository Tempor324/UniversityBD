using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniverBD_v2.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "disciplines",
                columns: table => new
                {
                    disciplineId = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    disciplineName = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.disciplineId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    teacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    teacherName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.teacherId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "studentgroups",
                columns: table => new
                {
                    groupId = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    yearNum = table.Column<int>(type: "int", nullable: true),
                    specialization = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    teacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.groupId);
                    table.ForeignKey(
                        name: "studentgroups_ibfk_1",
                        column: x => x.teacherId,
                        principalTable: "teachers",
                        principalColumn: "teacherId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "teacherdisciplines",
                columns: table => new
                {
                    teacherId = table.Column<int>(type: "int", nullable: false),
                    disciplineId = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.teacherId, x.disciplineId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "teacherdisciplines_ibfk_1",
                        column: x => x.teacherId,
                        principalTable: "teachers",
                        principalColumn: "teacherId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "teacherdisciplines_ibfk_2",
                        column: x => x.disciplineId,
                        principalTable: "disciplines",
                        principalColumn: "disciplineId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    studentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    studentName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    groupId = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    birthDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.studentId);
                    table.ForeignKey(
                        name: "students_ibfk_1",
                        column: x => x.groupId,
                        principalTable: "studentgroups",
                        principalColumn: "groupId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "week_shedule",
                columns: table => new
                {
                    dayNum = table.Column<int>(type: "int", nullable: false),
                    lesson = table.Column<int>(type: "int", nullable: false),
                    groupId = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    disciplineId = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    teacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.dayNum, x.lesson })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "week_shedule_FK",
                        column: x => x.groupId,
                        principalTable: "studentgroups",
                        principalColumn: "groupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "week_shedule_FK_1",
                        column: x => x.disciplineId,
                        principalTable: "disciplines",
                        principalColumn: "disciplineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "week_shedule_FK_2",
                        column: x => x.teacherId,
                        principalTable: "teachers",
                        principalColumn: "teacherId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "teacherId",
                table: "studentgroups",
                column: "teacherId");

            migrationBuilder.CreateIndex(
                name: "groupId",
                table: "students",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "disciplineId",
                table: "teacherdisciplines",
                column: "disciplineId");

            migrationBuilder.CreateIndex(
                name: "week_shedule_FK",
                table: "week_shedule",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "week_shedule_FK_1",
                table: "week_shedule",
                column: "disciplineId");

            migrationBuilder.CreateIndex(
                name: "week_shedule_FK_2",
                table: "week_shedule",
                column: "teacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "teacherdisciplines");

            migrationBuilder.DropTable(
                name: "week_shedule");

            migrationBuilder.DropTable(
                name: "studentgroups");

            migrationBuilder.DropTable(
                name: "disciplines");

            migrationBuilder.DropTable(
                name: "teachers");
        }
    }
}
