using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Semesters_SemesterId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_Classes_ClassId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_Classes_ClassId",
                table: "StudentGrades");

            migrationBuilder.AlterColumn<bool>(
                name: "IsStatus",
                table: "Users",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsStatus",
                table: "Subjects",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "ClassSemesterId",
                table: "StudentGrades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassSemesterId1",
                table: "StudentGrades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassSemesterId",
                table: "ClassSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsStatus",
                table: "Classes",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "ClassSemesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSemesters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSemesters_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassSemesters_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_ClassSemesterId",
                table: "StudentGrades",
                column: "ClassSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_ClassSemesterId1",
                table: "StudentGrades",
                column: "ClassSemesterId1");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_ClassSemesterId",
                table: "ClassSchedules",
                column: "ClassSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSemesters_ClassId",
                table: "ClassSemesters",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSemesters_SemesterId",
                table: "ClassSemesters",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Semesters_SemesterId",
                table: "Classes",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_ClassSemesters_ClassSemesterId",
                table: "ClassSchedules",
                column: "ClassSemesterId",
                principalTable: "ClassSemesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_Classes_ClassId",
                table: "ClassSchedules",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_ClassSemesters_ClassSemesterId",
                table: "StudentGrades",
                column: "ClassSemesterId",
                principalTable: "ClassSemesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_ClassSemesters_ClassSemesterId1",
                table: "StudentGrades",
                column: "ClassSemesterId1",
                principalTable: "ClassSemesters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_Classes_ClassId",
                table: "StudentGrades",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Semesters_SemesterId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_ClassSemesters_ClassSemesterId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_Classes_ClassId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_ClassSemesters_ClassSemesterId",
                table: "StudentGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_ClassSemesters_ClassSemesterId1",
                table: "StudentGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_Classes_ClassId",
                table: "StudentGrades");

            migrationBuilder.DropTable(
                name: "ClassSemesters");

            migrationBuilder.DropIndex(
                name: "IX_StudentGrades_ClassSemesterId",
                table: "StudentGrades");

            migrationBuilder.DropIndex(
                name: "IX_StudentGrades_ClassSemesterId1",
                table: "StudentGrades");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedules_ClassSemesterId",
                table: "ClassSchedules");

            migrationBuilder.DropColumn(
                name: "ClassSemesterId",
                table: "StudentGrades");

            migrationBuilder.DropColumn(
                name: "ClassSemesterId1",
                table: "StudentGrades");

            migrationBuilder.DropColumn(
                name: "ClassSemesterId",
                table: "ClassSchedules");

            migrationBuilder.AlterColumn<bool>(
                name: "IsStatus",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsStatus",
                table: "Subjects",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsStatus",
                table: "Classes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Semesters_SemesterId",
                table: "Classes",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_Classes_ClassId",
                table: "ClassSchedules",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_Classes_ClassId",
                table: "StudentGrades",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
