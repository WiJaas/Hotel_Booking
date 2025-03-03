using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_Booking.Migrations.HotelBooking
{
    /// <inheritdoc />
    public partial class AddPhotoUrlToChambreNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chambre_New",
                columns: table => new
                {
                    IdChambre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capacite = table.Column<int>(type: "int", nullable: true),
                    DescriptionChambre = table.Column<string>(type: "text", nullable: true),
                    StatutChambre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TarifParNuit = table.Column<double>(type: "float", nullable: true),
                    TypeChambre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PhotoUrl = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Chambre___50CF822B375F6A72", x => x.IdChambre);
                });

            migrationBuilder.CreateTable(
                name: "Reservation_New",
                columns: table => new
                {
                    IdReservation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdChambre = table.Column<int>(type: "int", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateReservation = table.Column<DateTime>(type: "datetime", nullable: true),
                    StatutReservation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TypeReservation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reservat__7E69A57B7067A2D2", x => x.IdReservation);
                    table.ForeignKey(
                        name: "FK__Reservati__IdCha__45F365D3",
                        column: x => x.IdChambre,
                        principalTable: "Chambre_New",
                        principalColumn: "IdChambre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_New_IdChambre",
                table: "Reservation_New",
                column: "IdChambre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservation_New");

            migrationBuilder.DropTable(
                name: "Chambre_New");
        }
    }
}
