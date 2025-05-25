using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFallZone.Migrations
{
    /// <inheritdoc />
    public partial class AddedaWasDealfieldtoorderitemsforstatisticslogging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WasDeal",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WasDeal",
                table: "OrderItems");
        }
    }
}
