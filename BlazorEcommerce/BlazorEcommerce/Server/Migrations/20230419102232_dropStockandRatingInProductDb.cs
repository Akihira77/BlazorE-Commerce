using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEcommerce.Server.Migrations
{
	/// <inheritdoc />
	public partial class dropStockandRatingInProductDb : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Rating",
				table: "Products");

			migrationBuilder.AlterColumn<bool>(
				name: "M5",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.AlterColumn<bool>(
				name: "M4",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.AlterColumn<bool>(
				name: "M3",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.AlterColumn<bool>(
				name: "M2",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.AlterColumn<bool>(
				name: "M1",
				table: "ProductRatings",
				type: "bit",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "int");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<long>(
				name: "Rating",
				table: "Products",
				type: "bigint",
				nullable: false,
				defaultValue: 0L);

			migrationBuilder.AlterColumn<int>(
				name: "M5",
				table: "ProductRatings",
				type: "int",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "bit");

			migrationBuilder.AlterColumn<int>(
				name: "M4",
				table: "ProductRatings",
				type: "int",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "bit");

			migrationBuilder.AlterColumn<int>(
				name: "M3",
				table: "ProductRatings",
				type: "int",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "bit");

			migrationBuilder.AlterColumn<int>(
				name: "M2",
				table: "ProductRatings",
				type: "int",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "bit");

			migrationBuilder.AlterColumn<int>(
				name: "M1",
				table: "ProductRatings",
				type: "int",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "bit");

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 1,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 2,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 3,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 4,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 5,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 6,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 7,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 8,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 9,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 10,
				column: "Rating",
				value: 0L);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 11,
				column: "Rating",
				value: 0L);
		}
	}
}