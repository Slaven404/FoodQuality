using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QualityManager.Migrations
{
    /// <inheritdoc />
    public partial class FoodAnalysis_Result : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "FoodAnalyses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "FoodAnalyses");
        }
    }
}
