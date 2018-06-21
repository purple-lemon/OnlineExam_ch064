using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DAL.Migrations
{
    public partial class com : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CodeHistories_CodeId",
                table: "CodeHistories");

            migrationBuilder.CreateIndex(
                name: "IX_CodeHistories_CodeId",
                table: "CodeHistories",
                column: "CodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CodeHistories_CodeId",
                table: "CodeHistories");

            migrationBuilder.CreateIndex(
                name: "IX_CodeHistories_CodeId",
                table: "CodeHistories",
                column: "CodeId",
                unique: true);
        }
    }
}
