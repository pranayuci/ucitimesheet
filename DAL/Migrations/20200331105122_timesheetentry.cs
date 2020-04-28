using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class timesheetentry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimesheetEntries",
                columns: table => new
                {
                    TimesheetEntryId = table.Column<Guid>(nullable: false),
                    Day = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    WorkHours = table.Column<float>(nullable: false),
                    LunchHours = table.Column<float>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    isSubmitted = table.Column<bool>(nullable: false),
                    SubmittedDate = table.Column<DateTime>(nullable: false),
                    SubmittedBy = table.Column<string>(nullable: true),
                    SubmittedFrom = table.Column<string>(nullable: true),
                    isApproved = table.Column<bool>(nullable: false),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    ApprovedBy = table.Column<string>(nullable: true),
                    ApprovedFrom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetEntries", x => x.TimesheetEntryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimesheetEntries");
        }
    }
}
