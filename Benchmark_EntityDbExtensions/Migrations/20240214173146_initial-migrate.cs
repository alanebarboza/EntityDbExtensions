using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Benchmark_EntityDbExtensions.Migrations
{
    /// <inheritdoc />
    public partial class initialmigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RootClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RootClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChildClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RootClassId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildClass_RootClass_RootClassId",
                        column: x => x.RootClassId,
                        principalTable: "RootClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrandChildClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChildClassId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrandChildClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrandChildClass_ChildClass_ChildClassId",
                        column: x => x.ChildClassId,
                        principalTable: "ChildClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildClass_RootClassId",
                table: "ChildClass",
                column: "RootClassId");

            migrationBuilder.CreateIndex(
                name: "IX_GrandChildClass_ChildClassId",
                table: "GrandChildClass",
                column: "ChildClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrandChildClass");

            migrationBuilder.DropTable(
                name: "ChildClass");

            migrationBuilder.DropTable(
                name: "RootClass");
        }
    }
}
