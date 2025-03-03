using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Booking.Models;

public partial class HotelBookingContext : DbContext
{
    public HotelBookingContext()
    {
    }

    public HotelBookingContext(DbContextOptions<HotelBookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChambreNew> ChambreNews { get; set; }

    public virtual DbSet<ReservationNew> ReservationNews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(" Server=DESKTOP-IR3GGB1;Database=hotel_Booking;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChambreNew>(entity =>
        {
            entity.HasKey(e => e.IdChambre).HasName("PK__Chambre___50CF822B375F6A72");

            entity.ToTable("Chambre_New");

            entity.Property(e => e.DescriptionChambre).HasColumnType("text");
            entity.Property(e => e.StatutChambre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypeChambre)
                .HasMaxLength(50)
                .IsUnicode(false);
              entity.Property(e => e.PhotoUrl) // Add this configuration
        .HasMaxLength(255) // Optional: Set a length limit
        .IsUnicode(false);
        });

        modelBuilder.Entity<ReservationNew>(entity =>
        {
            entity.HasKey(e => e.IdReservation).HasName("PK__Reservat__7E69A57B7067A2D2");

            entity.ToTable("Reservation_New");

            entity.Property(e => e.DateDebut).HasColumnType("datetime");
            entity.Property(e => e.DateFin).HasColumnType("datetime");
            entity.Property(e => e.DateReservation).HasColumnType("datetime");
            entity.Property(e => e.StatutReservation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypeReservation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.IdChambreNavigation).WithMany(p => p.ReservationNews)
                .HasForeignKey(d => d.IdChambre)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__IdCha__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
