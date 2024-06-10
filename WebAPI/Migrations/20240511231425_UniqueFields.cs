using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UniqueFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Views_DiscussionId",
                table: "Views");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionVotes_DiscussionId",
                table: "DiscussionVotes");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_Topics_DiscussionId",
                table: "Discussions_Topics");

            migrationBuilder.DropIndex(
                name: "IX_CommentsVotes_DiscussionId",
                table: "CommentsVotes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DiscussionId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Views_DiscussionId_UserId",
                table: "Views",
                columns: new[] { "DiscussionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_Name",
                table: "Topics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionVotes_DiscussionId_UserId",
                table: "DiscussionVotes",
                columns: new[] { "DiscussionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_Topics_DiscussionId_TopicId",
                table: "Discussions_Topics",
                columns: new[] { "DiscussionId", "TopicId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_Title",
                table: "Discussions",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentsVotes_DiscussionId_UserId",
                table: "CommentsVotes",
                columns: new[] { "DiscussionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DiscussionId_UserId",
                table: "Comments",
                columns: new[] { "DiscussionId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Views_DiscussionId_UserId",
                table: "Views");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Topics_Name",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionVotes_DiscussionId_UserId",
                table: "DiscussionVotes");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_Topics_DiscussionId_TopicId",
                table: "Discussions_Topics");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_Title",
                table: "Discussions");

            migrationBuilder.DropIndex(
                name: "IX_CommentsVotes_DiscussionId_UserId",
                table: "CommentsVotes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DiscussionId_UserId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Views_DiscussionId",
                table: "Views",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionVotes_DiscussionId",
                table: "DiscussionVotes",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_Topics_DiscussionId",
                table: "Discussions_Topics",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsVotes_DiscussionId",
                table: "CommentsVotes",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DiscussionId",
                table: "Comments",
                column: "DiscussionId");
        }
    }
}
