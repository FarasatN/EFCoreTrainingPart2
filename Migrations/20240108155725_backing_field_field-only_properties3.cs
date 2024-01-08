using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreTrainingPart2.Migrations
{
    /// <inheritdoc />
    public partial class backing_field_fieldonly_properties3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Persons",
                newName: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Persons",
                newName: "Name");
        }
    }
}
