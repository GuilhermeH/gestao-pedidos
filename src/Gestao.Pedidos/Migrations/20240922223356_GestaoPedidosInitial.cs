using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestao.Pedidos.Migrations
{
    /// <inheritdoc />
    public partial class GestaoPedidosInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Descontos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoDesconto = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    QuantidadeAplicavel = table.Column<int>(type: "int", nullable: true),
                    ValorEmReais = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataInicial = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataFinal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Porcentagem = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Descontos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantidadeEstoque = table.Column<int>(type: "int", nullable: false),
                    DescontoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Descontos_DescontoId",
                        column: x => x.DescontoId,
                        principalTable: "Descontos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemsPedido",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PedidoIdPedido = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsPedido_Pedidos_PedidoIdPedido",
                        column: x => x.PedidoIdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido");
                    table.ForeignKey(
                        name: "FK_ItemsPedido_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemsPedido_PedidoIdPedido",
                table: "ItemsPedido",
                column: "PedidoIdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsPedido_ProdutoId",
                table: "ItemsPedido",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_DescontoId",
                table: "Produtos",
                column: "DescontoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemsPedido");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Descontos");
        }
    }
}
