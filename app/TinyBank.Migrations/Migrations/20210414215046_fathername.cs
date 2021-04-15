using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations.Migrations
{
    public partial class fathername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "AccountCard",
            //    newName: "AccountCard",
            //    newSchema: "model");

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                schema: "model",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FatherName",
                schema: "model",
                table: "Customer");

            //migrationBuilder.RenameTable(
            //    name: "AccountCard",
            //    schema: "model",
            //    newName: "AccountCard");
        }
    }
}
