using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class intialDoctorSpeclizationAdminpAtient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_by = table.Column<string>(type: "text", nullable: true),
                    Updated_by = table.Column<string>(type: "text", nullable: true),
                    Is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Doctor_name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Specialization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Medical_license_number = table.Column<string>(type: "text", nullable: false),
                    Is_approved = table.Column<bool>(type: "boolean", nullable: false),
                    Is_blocked = table.Column<bool>(type: "boolean", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    About = table.Column<string>(type: "text", nullable: true),
                    Profile = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Field_experience = table.Column<double>(type: "double precision", nullable: true),
                    Created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_by = table.Column<string>(type: "text", nullable: true),
                    Updated_by = table.Column<string>(type: "text", nullable: true),
                    Is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Patient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Patient_name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Dob = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Is_blocked = table.Column<bool>(type: "boolean", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_by = table.Column<string>(type: "text", nullable: true),
                    Updated_by = table.Column<string>(type: "text", nullable: true),
                    Is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Patient_id);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_by = table.Column<string>(type: "text", nullable: true),
                    Updated_by = table.Column<string>(type: "text", nullable: true),
                    Is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Specializations");
        }
    }
}
