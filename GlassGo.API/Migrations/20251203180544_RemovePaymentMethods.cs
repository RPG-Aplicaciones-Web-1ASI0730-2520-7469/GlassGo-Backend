using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace GlassGo.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovePaymentMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment_methods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payment_methods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    account = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    bank = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_payment_methods", x => x.id);
                    table.ForeignKey(
                        name: "f_k_payment_methods_users_user_temp_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_payment_methods_user_id",
                table: "payment_methods",
                column: "user_id");
        }
    }
}
