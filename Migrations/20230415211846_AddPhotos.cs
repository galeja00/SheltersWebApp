using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cv8_ASP.NET_v2.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoImg",
                table: "Harbars",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "photoImg",
                table: "Dogs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoImg",
                table: "Harbars");

            migrationBuilder.DropColumn(
                name: "photoImg",
                table: "Dogs");
        }
    }
}
