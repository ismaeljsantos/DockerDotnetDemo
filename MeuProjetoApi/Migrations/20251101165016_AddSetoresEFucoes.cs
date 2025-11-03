using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuProjetoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSetoresEFucoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PessoaFuncoes",
                columns: table => new
                {
                    FkPessoaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkFuncaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkSetorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoaFuncoes", x => new { x.FkPessoaId, x.FkFuncaoId, x.FkSetorId });
                    table.ForeignKey(
                        name: "FK_PessoaFuncoes_Funcoes_FkFuncaoId",
                        column: x => x.FkFuncaoId,
                        principalTable: "Funcoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PessoaFuncoes_Pessoas_FkPessoaId",
                        column: x => x.FkPessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PessoaFuncoes_Setores_FkSetorId",
                        column: x => x.FkSetorId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PessoaFuncoes_FkFuncaoId",
                table: "PessoaFuncoes",
                column: "FkFuncaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PessoaFuncoes_FkSetorId",
                table: "PessoaFuncoes",
                column: "FkSetorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PessoaFuncoes");

            migrationBuilder.DropTable(
                name: "Funcoes");

            migrationBuilder.DropTable(
                name: "Setores");
        }
    }
}
