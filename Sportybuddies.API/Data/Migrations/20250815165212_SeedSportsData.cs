using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sportybuddies.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedSportsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sports",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("a0a0a0a0-0001-4000-8000-000000000001"), "Fitness training and strength building activities in a fitness center environment.", "Gym" },
                    { new Guid("a0a0a0a0-0002-4000-8000-000000000002"), "Combat sport involving striking with fists while wearing protective gloves.", "Boxing" },
                    { new Guid("a0a0a0a0-0003-4000-8000-000000000003"), "Water sport of riding waves on a surfboard in ocean or sea environments.", "Surfing" },
                    { new Guid("a0a0a0a0-0004-4000-8000-000000000004"), "Team sport played on a rectangular court with a hoop at each end.", "Basketball" },
                    { new Guid("a0a0a0a0-0005-4000-8000-000000000005"), "Winter sport descending snow-covered slopes on a snowboard.", "Snowboarding" },
                    { new Guid("a0a0a0a0-0006-4000-8000-000000000006"), "Outdoor activity of walking on trails and paths in natural environments.", "Hiking" },
                    { new Guid("a0a0a0a0-0007-4000-8000-000000000007"), "Physical, mental, and spiritual practice combining poses, breathing, and meditation.", "Yoga" },
                    { new Guid("a0a0a0a0-0008-4000-8000-000000000008"), "Sport of riding bicycles for competition or recreation on road or off-road.", "Cycling" },
                    { new Guid("a0a0a0a0-0009-4000-8000-000000000009"), "Individual or team racing sport that requires the use of one's entire body to move through water.", "Swimming" },
                    { new Guid("a0a0a0a0-0010-4000-8000-000000000010"), "Racquet sport played individually against a single opponent or between two teams of two players each.", "Tennis" },
                    { new Guid("a0a0a0a0-0011-4000-8000-000000000011"), "Athletic sport involving running over various distances on track, road, or trails.", "Running" },
                    { new Guid("a0a0a0a0-0012-4000-8000-000000000012"), "Winter sport of sliding down snow-covered slopes on skis.", "Skiing" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0001-4000-8000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0002-4000-8000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0003-4000-8000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0004-4000-8000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0005-4000-8000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0006-4000-8000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0007-4000-8000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0008-4000-8000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0009-4000-8000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0010-4000-8000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0011-4000-8000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: new Guid("a0a0a0a0-0012-4000-8000-000000000012"));
        }
    }
}
