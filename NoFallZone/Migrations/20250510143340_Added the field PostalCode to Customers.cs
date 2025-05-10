using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFallZone.Migrations
{
    /// <inheritdoc />
    public partial class AddedthefieldPostalCodetoCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostalCode",
                table: "Customers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Customers");
        }
    }
}
