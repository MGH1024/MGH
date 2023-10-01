using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MGH.EF.API.Migrations
{
    /// <inheritdoc />
    public partial class ownOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_CityTitle",
                schema: "dbo",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_StateTitle",
                schema: "dbo",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_CityTitle",
                schema: "dbo",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Address_StateTitle",
                schema: "dbo",
                table: "Posts");
        }
    }
}
