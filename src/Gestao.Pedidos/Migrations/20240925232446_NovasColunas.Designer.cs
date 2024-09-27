﻿// <auto-generated />
using System;
using Gestao.Pedidos.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Gestao.Pedidos.Migrations
{
    [DbContext(typeof(GestaoPedidosDbContext))]
    [Migration("20240925232446_NovasColunas")]
    partial class NovasColunas
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Gestao.Pedidos.Pagamentos.Pagamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId")
                        .IsUnique();

                    b.ToTable("Pagamento");
                });

            modelBuilder.Entity("Gestao.Pedidos.Recepcao.Desconto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TipoDesconto")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("Id");

                    b.ToTable("Descontos", (string)null);

                    b.HasDiscriminator<string>("TipoDesconto").HasValue("Desconto");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Gestao.Pedidos.Recepcao.ItemPedido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PedidoIdPedido")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PedidoIdPedido");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ItemsPedido");
                });

            modelBuilder.Entity("Gestao.Pedidos.Recepcao.Pedido", b =>
                {
                    b.Property<Guid>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataPedido")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdPedido");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("Gestao.Pedidos.Recepcao.Produto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DescontoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("PrecoUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("QuantidadeEstoque")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DescontoId");

                    b.ToTable("Produtos", (string)null);
                });

            modelBuilder.Entity("Gestao.Pedidos.Recepcao.DescontoQuantidade", b =>
                {
                    b.HasBaseType("Gestao.Pedidos.Recepcao.Desconto");

                    b.Property<int>("QuantidadeAplicavel")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorEmReais")
                        .HasColumnType("decimal(18,2)");

                    b.HasDiscriminator().HasValue("Quantidade");
                });

            modelBuilder.Entity("Gestao.Pedidos.Recepcao.DescontoSazonal", b =>
                {
                    b.HasBaseType("Gestao.Pedidos.Recepcao.Desconto");

                    b.Property<DateTime>("DataFinal")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInicial")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Porcentagem")
                        .HasColumnType("decimal(5,2)");

                    b.HasDiscriminator().HasValue("Sazonal");
                });

            modelBuilder.Entity("Gestao.Pedidos.Pagamentos.Pagamento", b =>
                {
                    b.HasOne("Gestao.Pedidos.Recepcao.Pedido", "Pedido")
                        .WithOne("Pagamento")
                        .HasForeignKey("Gestao.Pedidos.Pagamentos.Pagamento", "PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("Gestao.Pedidos.Recepcao.ItemPedido", b =>
                {
                    b.HasOne("Gestao.Pedidos.Recepcao.Pedido", null)
                        .WithMany("Itens")
                        .HasForeignKey("PedidoIdPedido");

                    b.HasOne("Gestao.Pedidos.Recepcao.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("Gestao.Pedidos.Recepcao.Produto", b =>
                {
                    b.HasOne("Gestao.Pedidos.Recepcao.Desconto", "Desconto")
                        .WithMany()
                        .HasForeignKey("DescontoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Desconto");
                });

            modelBuilder.Entity("Gestao.Pedidos.Recepcao.Pedido", b =>
                {
                    b.Navigation("Itens");

                    b.Navigation("Pagamento")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
