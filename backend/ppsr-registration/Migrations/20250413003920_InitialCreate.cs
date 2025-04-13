﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ppsr_registration.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GrantorFirstName = table.Column<string>(type: "TEXT", nullable: false),
                    GrantorMiddleNames = table.Column<string>(type: "TEXT", nullable: true),
                    GrantorLastName = table.Column<string>(type: "TEXT", nullable: false),
                    VIN = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Duration = table.Column<string>(type: "TEXT", nullable: false),
                    SPG_ACN = table.Column<string>(type: "TEXT", nullable: false),
                    SPG_OrgName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");
        }
    }
}
