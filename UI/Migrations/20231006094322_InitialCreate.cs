using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachId);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientUserId = table.Column<int>(type: "int", nullable: false),
                    DoctorUserId = table.Column<int>(type: "int", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: true),
                    PatientMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachId = table.Column<int>(type: "int", nullable: true),
                    DoctorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Attachments_AttachId",
                        column: x => x.AttachId,
                        principalTable: "Attachments",
                        principalColumn: "AttachId");
                });

            migrationBuilder.CreateTable(
                name: "Photographs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photographs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecializationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecializationDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: true),
                    PriceDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotographId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Photographs_PhotographId",
                        column: x => x.PhotographId,
                        principalTable: "Photographs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AttachId",
                table: "Appointments",
                column: "AttachId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorUserId",
                table: "Appointments",
                column: "DoctorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientUserId",
                table: "Appointments",
                column: "PatientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Photographs_UserId",
                table: "Photographs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhotographId",
                table: "Users",
                column: "PhotographId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_DoctorUserId",
                table: "Appointments",
                column: "DoctorUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_PatientUserId",
                table: "Appointments",
                column: "PatientUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photographs_Users_UserId",
                table: "Photographs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photographs_Users_UserId",
                table: "Photographs");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Photographs");
        }
    }
}
