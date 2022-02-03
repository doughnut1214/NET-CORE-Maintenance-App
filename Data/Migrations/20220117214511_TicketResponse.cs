using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketApplication.Data.Migrations
{
    public partial class TicketResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminResponse",
                table: "Ticket");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Ticket",
                newName: "TicketId");

            migrationBuilder.CreateTable(
                name: "TicketResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketResponse_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketResponse_TicketId",
                table: "TicketResponse",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketResponse");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "Ticket",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "AdminResponse",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
