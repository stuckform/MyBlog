using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBlog.Migrations
{
    public partial class _09 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactForm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactForm",
                columns: table => new
                {
                    Body = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    PhoneNum = table.Column<string>(type: "text", nullable: true),
                    Subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                });
        }
    }
}
