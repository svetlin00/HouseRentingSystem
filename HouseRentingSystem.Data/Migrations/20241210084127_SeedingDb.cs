using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HouseRentingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "IsActive", "PricePerMonth", "RenterId", "Title" },
                values: new object[,]
                {
                    { new Guid("4a8b95e9-4109-4dee-980d-c871937ea507"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("d00e43ce-0e5a-421c-aa12-0ebf7b5d0926"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "It has the best comfort you will ever ask for. With two bedrooms,it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", false, 1200.00m, null, "Family House Comfort" },
                    { new Guid("8a933b72-ba97-42f3-97ec-24b8323e5909"), "North London, UK (near the border)", new Guid("d00e43ce-0e5a-421c-aa12-0ebf7b5d0926"), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wpcontent/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", false, 2100.00m, new Guid("d17f077f-1d35-4f06-4d97-08dd18f5ab75"), "Big House Marina" },
                    { new Guid("99b8e7e7-1218-4f97-b08c-0485c2340f62"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("d00e43ce-0e5a-421c-aa12-0ebf7b5d0926"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This luxurious house is everything you will need. It isust excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", false, 2000.00m, null, "Grand House" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("4a8b95e9-4109-4dee-980d-c871937ea507"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("8a933b72-ba97-42f3-97ec-24b8323e5909"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("99b8e7e7-1218-4f97-b08c-0485c2340f62"));
        }
    }
}
