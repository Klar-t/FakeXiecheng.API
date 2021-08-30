using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Migrations
{
    public partial class ordermigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "LineItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    CreateDateUTC = table.Column<DateTime>(nullable: false),
                    TransactionMetadata = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308690dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "0a5fe395-98b9-4f3e-8b97-4e4c560991fd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90384155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ffc8e173-1eef-4f11-b879-51a0e777b99e", "AQAAAAEAACcQAAAAEJFj7LgBAe3eJcb39sKlLAZe74BLEIJJfp96Y0f/9TjE9KKKliTJT0Mhsnosfe1lrQ==", "0ccd2dc9-2503-4b06-b2f9-9f538fa8b18e" });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_OrderId",
                table: "LineItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Orders_OrderId",
                table: "LineItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Orders_OrderId",
                table: "LineItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_OrderId",
                table: "LineItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "LineItems");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308690dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "551a5743-65f8-4511-a767-bda26148ecc9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90384155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2d6f5e2a-3c73-489e-81b4-2801916117be", "AQAAAAEAACcQAAAAEBZjmvwvvaB9nD9xXR6booXTSzOyp13WIT+5/liE+DEXTjVK/6hwdMAVkiA4ahm7qw==", "e4d26824-ea73-4b7d-8687-80c989f5def4" });
        }
    }
}
