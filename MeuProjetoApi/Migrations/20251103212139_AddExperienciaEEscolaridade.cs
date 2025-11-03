using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuProjetoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddExperienciaEEscolaridade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "ExperienciasProfissionais");

            migrationBuilder.DropColumn(
                name: "DataFim",
                table: "ExperienciasProfissionais");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "ExperienciasProfissionais");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Contatos");

            migrationBuilder.RenameColumn(
                name: "Cargo",
                table: "ExperienciasProfissionais",
                newName: "Funcao");

            migrationBuilder.RenameColumn(
                name: "CEP",
                table: "Enderecos",
                newName: "Cep");

            migrationBuilder.RenameColumn(
                name: "UF",
                table: "Enderecos",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "Telefone",
                table: "Contatos",
                newName: "Observacao");

            migrationBuilder.RenameColumn(
                name: "EhPrincipal",
                table: "Contatos",
                newName: "IsPrincipal");

            migrationBuilder.AddColumn<int>(
                name: "AnoEntrada",
                table: "ExperienciasProfissionais",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnoSaida",
                table: "ExperienciasProfissionais",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "ExperienciasProfissionais",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPrincipal",
                table: "Enderecos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Contatos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Valor",
                table: "Contatos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnoEntrada",
                table: "ExperienciasProfissionais");

            migrationBuilder.DropColumn(
                name: "AnoSaida",
                table: "ExperienciasProfissionais");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "ExperienciasProfissionais");

            migrationBuilder.DropColumn(
                name: "IsPrincipal",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Contatos");

            migrationBuilder.RenameColumn(
                name: "Funcao",
                table: "ExperienciasProfissionais",
                newName: "Cargo");

            migrationBuilder.RenameColumn(
                name: "Cep",
                table: "Enderecos",
                newName: "CEP");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Enderecos",
                newName: "UF");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "Contatos",
                newName: "Telefone");

            migrationBuilder.RenameColumn(
                name: "IsPrincipal",
                table: "Contatos",
                newName: "EhPrincipal");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "ExperienciasProfissionais",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFim",
                table: "ExperienciasProfissionais",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "ExperienciasProfissionais",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Contatos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
