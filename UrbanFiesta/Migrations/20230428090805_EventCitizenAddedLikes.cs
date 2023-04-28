using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanFiesta.Migrations
{
    /// <inheritdoc />
    public partial class EventCitizenAddedLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CitizenId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CitizenId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsEnded",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsStarted",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CitizenId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Events",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PosterUrl",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CitizenEvent",
                columns: table => new
                {
                    LikedEventsId = table.Column<int>(type: "int", nullable: false),
                    LikesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenEvent", x => new { x.LikedEventsId, x.LikesId });
                    table.ForeignKey(
                        name: "FK_CitizenEvent_AspNetUsers_LikesId",
                        column: x => x.LikesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CitizenEvent_Events_LikedEventsId",
                        column: x => x.LikedEventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CitizenEvent_LikesId",
                table: "CitizenEvent",
                column: "LikesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CitizenEvent");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PosterUrl",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Events",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnded",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStarted",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CitizenId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CitizenId",
                table: "AspNetUsers",
                column: "CitizenId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CitizenId",
                table: "AspNetUsers",
                column: "CitizenId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
