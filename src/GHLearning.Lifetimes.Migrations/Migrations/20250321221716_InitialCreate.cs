using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHLearning.Lifetimes.Migrations.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterDatabase()
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "todolist",
				columns: table => new
				{
					id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "識別碼", collation: "ascii_general_ci")
						.Annotation("MySql:CharSet", "ascii"),
					title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "標題", collation: "utf8mb4_general_ci")
						.Annotation("MySql:CharSet", "utf8mb4"),
					content = table.Column<string>(type: "text", nullable: false, comment: "內容", collation: "utf8mb4_general_ci")
						.Annotation("MySql:CharSet", "utf8mb4"),
					status = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "狀態(1:待處理 2:已完成 3:已刪除)"),
					created_at = table.Column<DateTime>(type: "datetime(6)", maxLength: 6, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "創建時間"),
					updated_at = table.Column<DateTime>(type: "datetime(6)", maxLength: 6, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "最後更新時間")
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.id);
				})
				.Annotation("MySql:CharSet", "utf8mb4")
				.Annotation("Relational:Collation", "utf8mb4_general_ci");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "todolist");
		}
	}
}
