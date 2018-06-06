﻿// <auto-generated />
using DOMAIN;
using INFRAESTRUCTURE;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180601055233_initialize")]
    partial class initialize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DOMAIN.Categoria", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<string>("Descricao");

                    b.Property<bool>("Excluido");

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("DOMAIN.Fornecedor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CnpjCpf");

                    b.Property<DateTime?>("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<string>("Email");

                    b.Property<bool>("Excluido");

                    b.Property<string>("Nome");

                    b.Property<string>("Telefone");

                    b.HasKey("Id");

                    b.HasIndex("CnpjCpf")
                        .IsUnique()
                        .HasFilter("([CnpjCpf] IS NOT NULL)");

                    b.ToTable("Fornecedores");
                });

            modelBuilder.Entity("DOMAIN.Permissao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<bool>("Excluido");

                    b.Property<int>("Nivel");

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("Permissoes");
                });

            modelBuilder.Entity("DOMAIN.Proposta", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoriaId");

                    b.Property<DateTime?>("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<string>("Descricao")
                        .HasMaxLength(500);

                    b.Property<bool>("Excluido");

                    b.Property<Guid>("FornecedorId");

                    b.Property<string>("NomeProposta");

                    b.Property<int>("Status");

                    b.Property<string>("Valor");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("FornecedorId");

                    b.ToTable("Propostas");
                });

            modelBuilder.Entity("DOMAIN.PropostaAnexo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType");

                    b.Property<DateTime?>("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<bool>("Excluido");

                    b.Property<byte[]>("FileContent");

                    b.Property<string>("Nome");

                    b.Property<Guid?>("PropostaId");

                    b.HasKey("Id");

                    b.HasIndex("PropostaId");

                    b.ToTable("PropostaAnexos");
                });

            modelBuilder.Entity("DOMAIN.PropostaHistorico", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<bool>("Excluido");

                    b.Property<Guid>("PropostaId");

                    b.Property<int>("PropostaStatus");

                    b.Property<Guid>("UsuarioId");

                    b.HasKey("Id");

                    b.HasIndex("PropostaId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("PropostasHistoricos");
                });

            modelBuilder.Entity("DOMAIN.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(18);

                    b.Property<DateTime?>("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<DateTime>("DataNacimento");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("Excluido");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("Senha")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("DOMAIN.UsuarioPermissao", b =>
                {
                    b.Property<Guid>("UsuarioId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PermissaoId");

                    b.HasKey("UsuarioId");

                    b.HasIndex("PermissaoId");

                    b.ToTable("UsuarioPermissoes");
                });

            modelBuilder.Entity("DOMAIN.Proposta", b =>
                {
                    b.HasOne("DOMAIN.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DOMAIN.Fornecedor", "Fornecedor")
                        .WithMany()
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DOMAIN.PropostaAnexo", b =>
                {
                    b.HasOne("DOMAIN.Proposta", "Proposta")
                        .WithMany("PropostaAnexos")
                        .HasForeignKey("PropostaId");
                });

            modelBuilder.Entity("DOMAIN.PropostaHistorico", b =>
                {
                    b.HasOne("DOMAIN.Proposta", "Propostas")
                        .WithMany("PropostaHistoricos")
                        .HasForeignKey("PropostaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DOMAIN.UsuarioPermissao", b =>
                {
                    b.HasOne("DOMAIN.Permissao", "Permissoes")
                        .WithMany("UsuarioPermissoes")
                        .HasForeignKey("PermissaoId")
                        .HasConstraintName("FK_UsuarioPermissoes_Permissoes");

                    b.HasOne("DOMAIN.Usuario", "Usuario")
                        .WithOne("UsuarioPermissoes")
                        .HasForeignKey("DOMAIN.UsuarioPermissao", "UsuarioId")
                        .HasConstraintName("FK_UsuarioPermissoes_Usuarios");
                });
#pragma warning restore 612, 618
        }
    }
}
