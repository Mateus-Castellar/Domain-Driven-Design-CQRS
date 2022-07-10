﻿// <auto-generated />
using System;
using DDD.Pagamentos.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DDD.Pagamentos.Data.Migrations
{
    [DbContext(typeof(PagamentosContext))]
    [Migration("20220710033946_TabelasPagamento")]
    partial class TabelasPagamento
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DDD.Pagamentos.Business.Entities.Pagamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CvvCartao")
                        .IsRequired()
                        .HasColumnType("varchar(4)");

                    b.Property<string>("ExpiracaoCartao")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("NomeCartao")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<string>("NumeroCartao")
                        .IsRequired()
                        .HasColumnType("varchar(16)");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Pagamentos", (string)null);
                });

            modelBuilder.Entity("DDD.Pagamentos.Business.Entities.Transacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PagamentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("StatusTransacao")
                        .HasColumnType("int");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PagamentoId")
                        .IsUnique();

                    b.ToTable("Transacoes", (string)null);
                });

            modelBuilder.Entity("DDD.Pagamentos.Business.Entities.Transacao", b =>
                {
                    b.HasOne("DDD.Pagamentos.Business.Entities.Pagamento", "Pagamento")
                        .WithOne("Transacao")
                        .HasForeignKey("DDD.Pagamentos.Business.Entities.Transacao", "PagamentoId")
                        .IsRequired();

                    b.Navigation("Pagamento");
                });

            modelBuilder.Entity("DDD.Pagamentos.Business.Entities.Pagamento", b =>
                {
                    b.Navigation("Transacao");
                });
#pragma warning restore 612, 618
        }
    }
}