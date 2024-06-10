using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class MultipleCommentsAllowedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_DiscussionId_UserId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DiscussionId",
                table: "Comments",
                column: "DiscussionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_DiscussionId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DiscussionId_UserId",
                table: "Comments",
                columns: new[] { "DiscussionId", "UserId" },
                unique: true);
        }
    }
}
