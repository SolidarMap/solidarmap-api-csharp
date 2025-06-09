using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolidarMap.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_SMP_TIPO_RECURSOS_C",
                columns: table => new
                {
                    ID_RECURSO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    RECURSO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SMP_TIPO_RECURSOS_C", x => x.ID_RECURSO);
                });

            migrationBuilder.CreateTable(
                name: "T_SMP_TIPO_USUARIOS_C",
                columns: table => new
                {
                    ID_TIPO_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_TIPO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SMP_TIPO_USUARIOS_C", x => x.ID_TIPO_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "T_SMP_TIPO_ZONAS_C",
                columns: table => new
                {
                    ID_ZONA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ZONA = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SMP_TIPO_ZONAS_C", x => x.ID_ZONA);
                });

            migrationBuilder.CreateTable(
                name: "T_SMP_USUARIOS_C",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_TIPO_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SMP_USUARIOS_C", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_T_SMP_USUARIOS_C_T_SMP_TIPO_USUARIOS_C_ID_TIPO_USUARIO",
                        column: x => x.ID_TIPO_USUARIO,
                        principalTable: "T_SMP_TIPO_USUARIOS_C",
                        principalColumn: "ID_TIPO_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_SMP_AJUDAS_C",
                columns: table => new
                {
                    ID_AJUDA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_RECURSO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(1)", nullable: false),
                    DATA_PUBLICACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SMP_AJUDAS_C", x => x.ID_AJUDA);
                    table.ForeignKey(
                        name: "FK_T_SMP_AJUDAS_C_T_SMP_TIPO_RECURSOS_C_ID_RECURSO",
                        column: x => x.ID_RECURSO,
                        principalTable: "T_SMP_TIPO_RECURSOS_C",
                        principalColumn: "ID_RECURSO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_SMP_AJUDAS_C_T_SMP_USUARIOS_C_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "T_SMP_USUARIOS_C",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_SMP_AVALIACOES_C",
                columns: table => new
                {
                    ID_AVALIACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_AJUDA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOTA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    COMENTARIO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DATA_AVALIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SMP_AVALIACOES_C", x => x.ID_AVALIACAO);
                    table.ForeignKey(
                        name: "FK_T_SMP_AVALIACOES_C_T_SMP_AJUDAS_C_ID_AJUDA",
                        column: x => x.ID_AJUDA,
                        principalTable: "T_SMP_AJUDAS_C",
                        principalColumn: "ID_AJUDA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_SMP_AVALIACOES_C_T_SMP_USUARIOS_C_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "T_SMP_USUARIOS_C",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_SMP_LOCALIZACOES_C",
                columns: table => new
                {
                    ID_LOCALIZACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_AJUDA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_ZONA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LATITUDE = table.Column<decimal>(type: "DECIMAL(12,8)", precision: 12, scale: 8, nullable: false),
                    LONGITUDE = table.Column<decimal>(type: "DECIMAL(12,8)", precision: 12, scale: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SMP_LOCALIZACOES_C", x => x.ID_LOCALIZACAO);
                    table.ForeignKey(
                        name: "FK_T_SMP_LOCALIZACOES_C_T_SMP_AJUDAS_C_ID_AJUDA",
                        column: x => x.ID_AJUDA,
                        principalTable: "T_SMP_AJUDAS_C",
                        principalColumn: "ID_AJUDA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_SMP_LOCALIZACOES_C_T_SMP_TIPO_ZONAS_C_ID_ZONA",
                        column: x => x.ID_ZONA,
                        principalTable: "T_SMP_TIPO_ZONAS_C",
                        principalColumn: "ID_ZONA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_SMP_MENSAGENS_C",
                columns: table => new
                {
                    ID_MENSAGEM = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_AJUDA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MENSAGEM = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    DATA_ENVIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SMP_MENSAGENS_C", x => x.ID_MENSAGEM);
                    table.ForeignKey(
                        name: "FK_T_SMP_MENSAGENS_C_T_SMP_AJUDAS_C_ID_AJUDA",
                        column: x => x.ID_AJUDA,
                        principalTable: "T_SMP_AJUDAS_C",
                        principalColumn: "ID_AJUDA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_SMP_MENSAGENS_C_T_SMP_USUARIOS_C_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "T_SMP_USUARIOS_C",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_SMP_AJUDAS_C_ID_RECURSO",
                table: "T_SMP_AJUDAS_C",
                column: "ID_RECURSO");

            migrationBuilder.CreateIndex(
                name: "IX_T_SMP_AJUDAS_C_ID_USUARIO",
                table: "T_SMP_AJUDAS_C",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_T_SMP_AVALIACOES_C_ID_AJUDA",
                table: "T_SMP_AVALIACOES_C",
                column: "ID_AJUDA");

            migrationBuilder.CreateIndex(
                name: "IX_T_SMP_AVALIACOES_C_ID_USUARIO",
                table: "T_SMP_AVALIACOES_C",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_T_SMP_LOCALIZACOES_C_ID_AJUDA",
                table: "T_SMP_LOCALIZACOES_C",
                column: "ID_AJUDA");

            migrationBuilder.CreateIndex(
                name: "IX_T_SMP_LOCALIZACOES_C_ID_ZONA",
                table: "T_SMP_LOCALIZACOES_C",
                column: "ID_ZONA");

            migrationBuilder.CreateIndex(
                name: "IX_T_SMP_MENSAGENS_C_ID_AJUDA",
                table: "T_SMP_MENSAGENS_C",
                column: "ID_AJUDA");

            migrationBuilder.CreateIndex(
                name: "IX_T_SMP_MENSAGENS_C_ID_USUARIO",
                table: "T_SMP_MENSAGENS_C",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_T_SMP_USUARIOS_C_ID_TIPO_USUARIO",
                table: "T_SMP_USUARIOS_C",
                column: "ID_TIPO_USUARIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_SMP_AVALIACOES_C");

            migrationBuilder.DropTable(
                name: "T_SMP_LOCALIZACOES_C");

            migrationBuilder.DropTable(
                name: "T_SMP_MENSAGENS_C");

            migrationBuilder.DropTable(
                name: "T_SMP_TIPO_ZONAS_C");

            migrationBuilder.DropTable(
                name: "T_SMP_AJUDAS_C");

            migrationBuilder.DropTable(
                name: "T_SMP_TIPO_RECURSOS_C");

            migrationBuilder.DropTable(
                name: "T_SMP_USUARIOS_C");

            migrationBuilder.DropTable(
                name: "T_SMP_TIPO_USUARIOS_C");
        }
    }
}
