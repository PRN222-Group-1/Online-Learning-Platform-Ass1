using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Learning_Platform_Ass1.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomLearningPathSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Learning_Paths",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "created_by_user_id",
                table: "Learning_Paths",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_custom_path",
                table: "Learning_Paths",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "source_assessment_id",
                table: "Learning_Paths",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Learning_Paths_created_by_user_id",
                table: "Learning_Paths",
                column: "created_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Learning_Paths_users_created_by_user_id",
                table: "Learning_Paths",
                column: "created_by_user_id",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Learning_Paths_users_created_by_user_id",
                table: "Learning_Paths");

            migrationBuilder.DropIndex(
                name: "IX_Learning_Paths_created_by_user_id",
                table: "Learning_Paths");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Learning_Paths");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "Learning_Paths");

            migrationBuilder.DropColumn(
                name: "is_custom_path",
                table: "Learning_Paths");

            migrationBuilder.DropColumn(
                name: "source_assessment_id",
                table: "Learning_Paths");
        }
    }
}
