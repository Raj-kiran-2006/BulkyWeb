using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoSeedBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "SellerId", "Title" },
                values: new object[,]
                {
                    { 7, "Mira Sen", 2, "When a lighthouse begins casting a second shadow, cartographer Elian discovers a hidden coastline drawn across the night sky. Following its shifting line leads to a quiet town that appears on no map and remembers a future no one has lived.", "BULKY000007", "/Images/Books/the-glass-meridian.svg", 48, 42, 34, 38, null, "The Glass Meridian" },
                    { 8, "Arun Vale", 3, "After inheriting an old observatory, Nila finds a hand-drawn map that marks forgotten places between the stars. Each journey reveals a small act of courage on Earth, and a choice that may bring a scattered family back together.", "BULKY000008", "/Images/Books/a-map-of-quiet-stars.svg", 52, 46, 37, 41, null, "A Map of Quiet Stars" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
