using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ppsr_registration.Migrations
{
    /// <inheritdoc />
    public partial class AddGrantorNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Registrations_GrantorFirstName_GrantorLastName",
                table: "Registrations",
                columns: new[] { "GrantorFirstName", "GrantorLastName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Registrations_GrantorFirstName_GrantorLastName",
                table: "Registrations");
        }
    }
}
