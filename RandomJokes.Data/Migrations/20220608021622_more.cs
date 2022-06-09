using Microsoft.EntityFrameworkCore.Migrations;

namespace RandomJokes.Data.Migrations
{
    public partial class more : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Jokes");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Jokes",
                newName: "Setup");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Jokes",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Answer",
                table: "Jokes",
                newName: "Punchline");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Jokes",
                newName: "Likes");

            migrationBuilder.RenameColumn(
                name: "Setup",
                table: "Jokes",
                newName: "Question");

            migrationBuilder.RenameColumn(
                name: "Punchline",
                table: "Jokes",
                newName: "Answer");

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Jokes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
