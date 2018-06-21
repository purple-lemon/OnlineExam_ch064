using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeHistories_UsersCode_CodeId",
                table: "CodeHistories");

            migrationBuilder.RenameColumn(
                name: "CodeId",
                table: "CodeHistories",
                newName: "UserCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_CodeHistories_CodeId",
                table: "CodeHistories",
                newName: "IX_CodeHistories_UserCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeHistories_UsersCode_UserCodeId",
                table: "CodeHistories",
                column: "UserCodeId",
                principalTable: "UsersCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeHistories_UsersCode_UserCodeId",
                table: "CodeHistories");

            migrationBuilder.RenameColumn(
                name: "UserCodeId",
                table: "CodeHistories",
                newName: "CodeId");

            migrationBuilder.RenameIndex(
                name: "IX_CodeHistories_UserCodeId",
                table: "CodeHistories",
                newName: "IX_CodeHistories_CodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeHistories_UsersCode_CodeId",
                table: "CodeHistories",
                column: "CodeId",
                principalTable: "UsersCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
