using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanFiesta.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCitizen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailForNewsletterConfirmed",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "CodeForConfirmEmailForNewsletter",
                table: "AspNetUsers",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeForConfirmEmailForNewsletter",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "IsEmailForNewsletterConfirmed",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
