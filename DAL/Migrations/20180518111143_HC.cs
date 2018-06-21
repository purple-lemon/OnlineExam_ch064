using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DAL.Migrations
{
    public partial class HC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskNumber",
                table: "AspNetUsers",
                newName: "DoneTaskNumber");

            migrationBuilder.AlterColumn<double>(
                name: "UserRating",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoneTaskNumber",
                table: "AspNetUsers",
                newName: "TaskNumber");

            migrationBuilder.AlterColumn<int>(
                name: "UserRating",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
