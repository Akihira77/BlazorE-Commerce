using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEcommerce.Server.Migrations
{
	/// <inheritdoc />
	public partial class modifyOrderHeaderEntity : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "PaymentStatus",
				table: "OrderHeaders");

			migrationBuilder.AddColumn<bool>(
				name: "IsPaid",
				table: "OrderHeaders",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "IsPaid",
				table: "OrderHeaders");

			migrationBuilder.AddColumn<int>(
				name: "PaymentStatus",
				table: "OrderHeaders",
				type: "int",
				nullable: false,
				defaultValue: 0);
		}
	}
}