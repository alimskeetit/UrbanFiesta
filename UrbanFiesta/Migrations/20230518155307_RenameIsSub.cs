using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanFiesta.Migrations
{
    /// <inheritdoc />
    public partial class RenameIsSub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSubcribed",
                table: "AspNetUsers",
                newName: "IsSubscribed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSubscribed",
                table: "AspNetUsers",
                newName: "IsSubcribed");
        }
    }
}
