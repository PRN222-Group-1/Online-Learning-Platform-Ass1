using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Learning_Platform_Ass1.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLearningPathEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_Learning_Path_Enrollments",
                columns: table => new
                {
                    enrollment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    path_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    enrolled_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    progress_percentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Learning_Path_Enrollments", x => x.enrollment_id);
                    table.ForeignKey(
                        name: "FK_User_Learning_Path_Enrollments_Learning_Paths_path_id",
                        column: x => x.path_id,
                        principalTable: "Learning_Paths",
                        principalColumn: "path_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Learning_Path_Enrollments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Learning_Path_Enrollments_path_id",
                table: "User_Learning_Path_Enrollments",
                column: "path_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Learning_Path_Enrollments_user_id",
                table: "User_Learning_Path_Enrollments",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Learning_Path_Enrollments");
        }
    }
}
