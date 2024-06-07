using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_Management.Migrations
{
    /// <inheritdoc />
    public partial class addedCombinedDtoandfixedsomemodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "expenses",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "subscriptions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "expenses",
                newName: "Description");
        }
    }
}
