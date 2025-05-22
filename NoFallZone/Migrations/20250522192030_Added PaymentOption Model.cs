using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoFallZone.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaymentOptionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "PaymentOptionId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentOptionName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentOptionId",
                table: "Orders",
                column: "PaymentOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOptions_Name",
                table: "PaymentOptions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentOptions_PaymentOptionId",
                table: "Orders",
                column: "PaymentOptionId",
                principalTable: "PaymentOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentOptions_PaymentOptionId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "PaymentOptions");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentOptionId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentOptionId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentOptionName",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
