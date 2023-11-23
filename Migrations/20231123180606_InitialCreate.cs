using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_Platform_DB_Exam.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentScore = table.Column<double>(type: "float", nullable: true),
                    StudentFacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudentLevel = table.Column<int>(type: "int", nullable: true),
                    StudentYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Faculties_StudentFacultyId",
                        column: x => x.StudentFacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkerFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LectureName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LectureWorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_Workers_LectureWorkerId",
                        column: x => x.LectureWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FacultyLecture",
                columns: table => new
                {
                    FacultyLecturesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LectureFacultiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacultyLecture", x => new { x.FacultyLecturesId, x.LectureFacultiesId });
                    table.ForeignKey(
                        name: "FK_FacultyLecture_Faculties_LectureFacultiesId",
                        column: x => x.LectureFacultiesId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacultyLecture_Lectures_FacultyLecturesId",
                        column: x => x.FacultyLecturesId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LectureStudent",
                columns: table => new
                {
                    LectureStudentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentLecturesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureStudent", x => new { x.LectureStudentsId, x.StudentLecturesId });
                    table.ForeignKey(
                        name: "FK_LectureStudent_Lectures_StudentLecturesId",
                        column: x => x.StudentLecturesId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LectureStudent_Students_LectureStudentsId",
                        column: x => x.LectureStudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacultyLecture_LectureFacultiesId",
                table: "FacultyLecture",
                column: "LectureFacultiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_LectureWorkerId",
                table: "Lectures",
                column: "LectureWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureStudent_StudentLecturesId",
                table: "LectureStudent",
                column: "StudentLecturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentFacultyId",
                table: "Students",
                column: "StudentFacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_FacultyId",
                table: "Workers",
                column: "FacultyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacultyLecture");

            migrationBuilder.DropTable(
                name: "LectureStudent");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Faculties");
        }
    }
}
