using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations.Migrations
{
    public partial class cardholder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "AccountCard",
            //    newName: "AccountCard",
            //    newSchema: "model");

            migrationBuilder.AddColumn<string>(
                name: "CardHolder",
                schema: "model",
                table: "Card",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardHolder",
                schema: "model",
                table: "Card");

            //migrationBuilder.RenameTable(
            //    name: "AccountCard",
            //    schema: "model",
            //    newName: "AccountCard");
        }
    }
}
