using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudStorage.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddParentIdToResource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Resources",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ParentId",
                table: "Resources",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Resources_ParentId",
                table: "Resources",
                column: "ParentId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Resources_ParentId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_ParentId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Resources");
        }
    }
}
