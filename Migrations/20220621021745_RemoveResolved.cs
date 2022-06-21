using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CP.Api.Migrations
{
    public partial class RemoveResolved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resolved",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Resolved",
                table: "Comments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
