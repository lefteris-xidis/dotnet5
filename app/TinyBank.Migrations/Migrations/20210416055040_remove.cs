using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations.Migrations
{
    public partial class remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FatherName",
                schema: "model",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CardHolder",
                schema: "model",
                table: "Card");

            //migrationBuilder.RenameTable(
            //    name: "AccountCard",
            //    newName: "AccountCard",
            //    newSchema: "model");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "AccountCard",
            //    schema: "model",
            //    newName: "AccountCard");

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                schema: "model",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardHolder",
                schema: "model",
                table: "Card",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
