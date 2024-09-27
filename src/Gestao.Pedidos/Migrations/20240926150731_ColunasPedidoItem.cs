using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestao.Pedidos.Migrations
{
    /// <inheritdoc />
    public partial class ColunasPedidoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "ItemsPedido",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorDesconto",
                table: "ItemsPedido",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorUnitario",
                table: "ItemsPedido",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "ItemsPedido");

            migrationBuilder.DropColumn(
                name: "ValorDesconto",
                table: "ItemsPedido");

            migrationBuilder.DropColumn(
                name: "ValorUnitario",
                table: "ItemsPedido");
        }
    }
}
