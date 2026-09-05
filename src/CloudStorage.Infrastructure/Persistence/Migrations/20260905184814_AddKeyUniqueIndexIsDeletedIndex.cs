using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudStorage.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddKeyUniqueIndexIsDeletedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Resources_IsDeleted",
                table: "Resources",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_Key",
                table: "Resources",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resources_IsDeleted",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_Key",
                table: "Resources");
        }
    }
}
