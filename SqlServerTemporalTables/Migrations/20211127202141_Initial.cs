using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerTemporalTables.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Livro",
                columns: table => new
                {
                    LivroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "varchar(200)", nullable: false),
                    Autor = table.Column<string>(type: "varchar(100)", nullable: false),
                    AnoPublicacao = table.Column<int>(type: "int", nullable: false),
                    FimValidade = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "FimValidade")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "InicioValidade"),
                    InicioValidade = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "FimValidade")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "InicioValidade")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro", x => x.LivroId);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LivroHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "FimValidade")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "InicioValidade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livro")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LivroHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "FimValidade")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "InicioValidade");
        }
    }
}
