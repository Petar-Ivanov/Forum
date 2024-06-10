using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UniqueCommentId_UserIdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommentsVotes_DiscussionId_UserId",
                table: "CommentsVotes");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsVotes_CommentId_UserId",
                table: "CommentsVotes",
                columns: new[] { "CommentId", "UserId" },
                unique: true,
                filter: "[CommentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsVotes_DiscussionId",
                table: "CommentsVotes",
                column: "DiscussionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommentsVotes_CommentId_UserId",
                table: "CommentsVotes");

            migrationBuilder.DropIndex(
                name: "IX_CommentsVotes_DiscussionId",
                table: "CommentsVotes");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsVotes_DiscussionId_UserId",
                table: "CommentsVotes",
                columns: new[] { "DiscussionId", "UserId" },
                unique: true);
        }
    }
}
