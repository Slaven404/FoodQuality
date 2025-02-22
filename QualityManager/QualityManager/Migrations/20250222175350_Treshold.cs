using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QualityManager.Migrations
{
    /// <inheritdoc />
    public partial class Treshold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tresholds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Low = table.Column<long>(type: "bigint", nullable: false),
                    High = table.Column<long>(type: "bigint", nullable: false),
                    AnalysisTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tresholds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tresholds_AnalysisTypes_AnalysisTypeId",
                        column: x => x.AnalysisTypeId,
                        principalTable: "AnalysisTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Tresholds",
                columns: new[] { "Id", "AnalysisTypeId", "High", "Low" },
                values: new object[,]
                {
                    { 1L, 1L, 77777777L, 22222222L },
                    { 2L, 2L, 7777777L, 2222222L },
                    { 3L, 3L, 777777L, 222222L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tresholds_AnalysisTypeId",
                table: "Tresholds",
                column: "AnalysisTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tresholds");
        }
    }
}
