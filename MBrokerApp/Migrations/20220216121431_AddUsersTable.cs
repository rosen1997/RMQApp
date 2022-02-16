using Microsoft.EntityFrameworkCore.Migrations;

namespace MBrokerApp.Migrations
{
    public partial class AddUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Ucn = table.Column<string>(maxLength: 10, nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 15, nullable: false),
                    Gender = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "City", "Country", "Gender", "Name", "PhoneNumber", "Ucn" },
                values: new object[] { 1, "ul. Studentska", "Varna", "Bulgaria", true, "Peter Petrov", "0875632908", "9206207963" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
