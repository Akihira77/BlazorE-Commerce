using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEcommerce.Server.Migrations
{
	/// <inheritdoc />
	public partial class addProductRatingsToDb : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "ProductRatings",
				columns: table => new
				{
					UserId = table.Column<int>(type: "int", nullable: false),
					ProductId = table.Column<int>(type: "int", nullable: false),
					M1 = table.Column<int>(type: "int", nullable: false),
					M2 = table.Column<int>(type: "int", nullable: false),
					M3 = table.Column<int>(type: "int", nullable: false),
					M4 = table.Column<int>(type: "int", nullable: false),
					M5 = table.Column<int>(type: "int", nullable: false),
					Reviews = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ProductRatings", x => new { x.UserId, x.ProductId });
					table.ForeignKey(
						name: "FK_ProductRatings_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_ProductRatings_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ProductRatings_ProductId",
				table: "ProductRatings",
				column: "ProductId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ProductRatings");
		}
	}
}