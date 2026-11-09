using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF EXISTS (
                    SELECT 1
                    FROM sys.foreign_keys
                    WHERE name = N'FK_Products_Categories_Category'
                      AND parent_object_id = OBJECT_ID(N'dbo.Products'))
                    ALTER TABLE [Products] DROP CONSTRAINT [FK_Products_Categories_Category];

                IF EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_Products_Category'
                      AND object_id = OBJECT_ID(N'dbo.Products'))
                    DROP INDEX [IX_Products_Category] ON [Products];

                IF COL_LENGTH(N'dbo.Products', N'Category') IS NOT NULL
                    ALTER TABLE [Products] DROP COLUMN [Category];

                IF COL_LENGTH(N'dbo.Products', N'CategoryId') IS NULL
                    ALTER TABLE [Products] ADD [CategoryId] int NOT NULL
                        CONSTRAINT [DF_Products_CategoryId] DEFAULT (1);
                """);

            migrationBuilder.Sql(
                """
                UPDATE [Products]
                SET [CategoryId] = CASE [Id]
                    WHEN 1 THEN 2
                    WHEN 2 THEN 1
                    WHEN 3 THEN 2
                    WHEN 4 THEN 1
                    WHEN 5 THEN 2
                    WHEN 6 THEN 1
                    ELSE 1
                END
                WHERE [CategoryId] IS NULL OR [CategoryId] = 0;
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE [Products] ALTER COLUMN [CategoryId] int NOT NULL;

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_Products_CategoryId'
                      AND object_id = OBJECT_ID(N'dbo.Products'))
                    CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.foreign_keys
                    WHERE name = N'FK_Products_Categories_CategoryId'
                      AND parent_object_id = OBJECT_ID(N'dbo.Products'))
                    ALTER TABLE [Products] ADD CONSTRAINT [FK_Products_Categories_CategoryId]
                        FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "CategoryId" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "Category",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category",
                table: "Products",
                column: "Category");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_Category",
                table: "Products",
                column: "Category",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
