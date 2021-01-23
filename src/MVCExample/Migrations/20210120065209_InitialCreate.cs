using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCExample.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "M_CHANNEL",
                columns: table => new
                {
                    CHANNEL_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CHANNEL_NAME = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    END_POINT = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    SECRET_KEY = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    TOKEN_KEY = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.CHANNEL_ID);
                });

            migrationBuilder.CreateTable(
                name: "M_SUBSCRIBE",
                columns: table => new
                {
                    CHANNEL_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    EVENT_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    EVENT_NAME = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribe", x => new { x.CHANNEL_ID, x.EVENT_ID });
                });

            migrationBuilder.CreateTable(
                name: "ZZ_EVENT",
                columns: table => new
                {
                    EVENT_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EVENT_NAME = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    EVENT_URL = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EVENT_ID);
                });

            migrationBuilder.InsertData(
                table: "ZZ_EVENT",
                columns: new[] { "EVENT_ID", "EVENT_NAME", "EVENT_URL" },
                values: new object[] { 809, "รับเข้าระบบ/collection mail", "/api/collection/mail" });

            migrationBuilder.InsertData(
                table: "ZZ_EVENT",
                columns: new[] { "EVENT_ID", "EVENT_NAME", "EVENT_URL" },
                values: new object[] { 815, "บันทึกผลจัดส่ง/proof of delivery", "/api/delivery/proof" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "M_CHANNEL");

            migrationBuilder.DropTable(
                name: "M_SUBSCRIBE");

            migrationBuilder.DropTable(
                name: "ZZ_EVENT");
        }
    }
}
