using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CourseEnrollmentFieldAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnrollmentCount",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 1,
                column: "EnrollmentCount",
                value: 23);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnrollmentCount",
                table: "Courses");
        }
    }
}
