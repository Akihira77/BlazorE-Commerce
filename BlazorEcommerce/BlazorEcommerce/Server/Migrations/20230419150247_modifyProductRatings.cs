using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEcommerce.Server.Migrations
{
	/// <inheritdoc />
	public partial class modifyProductRatings : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "M1",
				table: "ProductRatings");

			migrationBuilder.DropColumn(
				name: "M2",
				table: "ProductRatings");

			migrationBuilder.DropColumn(
				name: "M3",
				table: "ProductRatings");

			migrationBuilder.DropColumn(
				name: "M4",
				table: "ProductRatings");

			migrationBuilder.DropColumn(
				name: "M5",
				table: "ProductRatings");

			migrationBuilder.AddColumn<int>(
				name: "Rate",
				table: "ProductRatings",
				type: "int",
				nullable: false,
				defaultValue: 0);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Rate",
				table: "ProductRatings");

			migrationBuilder.AddColumn<bool>(
				name: "M1",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<bool>(
				name: "M2",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<bool>(
				name: "M3",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<bool>(
				name: "M4",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<bool>(
				name: "M5",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}
	}
}