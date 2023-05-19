using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanFiesta.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmailForNewsletter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailForNewsletter",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailForNewsletter",
                table: "AspNetUsers");
        }
    }
}
