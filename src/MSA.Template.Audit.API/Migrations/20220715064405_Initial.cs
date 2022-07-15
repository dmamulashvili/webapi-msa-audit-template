using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSA.Template.Audit.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Producer = table.Column<string>(type: "text", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    InitiatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityName = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<string>(type: "text", nullable: false),
                    PropertyName = table.Column<string>(type: "text", nullable: false),
                    PropertyOriginalValue = table.Column<string>(type: "text", nullable: true),
                    PropertyCurrentValue = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityLog_CreationDate",
                table: "EntityLog",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_EntityLog_EntityId",
                table: "EntityLog",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityLog_EntityName",
                table: "EntityLog",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_EntityLog_Producer",
                table: "EntityLog",
                column: "Producer");

            migrationBuilder.CreateIndex(
                name: "IX_EntityLog_PropertyName",
                table: "EntityLog",
                column: "PropertyName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityLog");
        }
    }
}
