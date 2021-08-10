using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Migrations
{
    public partial class applicationsUsermigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "308690dc-ae51-480f-824d-7dca6714c3e2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308690dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "d4daf628-1d9b-45ba-b5b9-1b431459ee63");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "90384155-dee0-40c9-bb1e-b5ed07afc04e", 0, null, "932142df-6e29-467e-8896-17c1280353ae", "admin@fakexicheng.com", true, false, null, "ADMIN@FAKEXICHENG.COM", "ADMIN@FAKEXICHENG.COM", "AQAAAAEAACcQAAAAEEU1+OsinbgrrYhWdt6YT0WfP1rlZ9V6mqrTSxYiiJjusUt04YvhPOGYqR3ypFppLg==", "123456789", false, "0c04bc01-fbb8-4025-b6bf-f8861d660584", false, "admin@fakexicheng.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "90384155-dee0-40c9-bb1e-b5ed07afc04e", "308690dc-ae51-480f-824d-7dca6714c3e2" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90384155-dee0-40c9-bb1e-b5ed07afc04e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308690dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "f6eb271b-e324-473d-88dd-8f06e8eb3390");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "ApplicationUserId" },
                values: new object[] { "90384155-dee0-40c9-bb1e-b5ed07afc04e", "308690dc-ae51-480f-824d-7dca6714c3e2", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "308690dc-ae51-480f-824d-7dca6714c3e2", 0, null, "bbe1b026-00eb-42ca-8d2a-00ed8c9ea75c", "admin@fakexicheng.com", true, false, null, "ADMIN@FAKEXICHENG.COM", "ADMIN@FAKEXICHENG.COM", "AQAAAAEAACcQAAAAEMCImmwYv780Cho0/I0aEqw6NP3d05GSlae1U2szuM7YZl+YN/kIOkGXcg6dfMWw5w==", "123456789", false, "b601d791-55ce-4b62-a0c4-d964d50d2d4f", false, "admin@fakexicheng.com" });
        }
    }
}
