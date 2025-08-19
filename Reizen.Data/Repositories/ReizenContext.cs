using System;
using System.Collections.Generic;
using Reizen.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models;

namespace Reizen.Data.Repositories;

public partial class ReizenContext : DbContext
{
    public ReizenContext ()
    {
    }

    public ReizenContext (DbContextOptions<ReizenContext> options)
        : base (options)
    {
    }

    public virtual DbSet<DestinationDAL> Destinations
    {
        get; set;
    }

    public virtual DbSet<BookingDAL> Bookings
    {
        get; set;
    }

    public virtual DbSet<ClientDAL> Clients
    {
        get; set;
    }

    public virtual DbSet<CountryDAL> Countries
    {
        get; set;
    }

    public virtual DbSet<TripDAL> Trips
    {
        get; set;
    }

    public virtual DbSet<ContinentDAL> Continents
    {
        get; set;
    }

    public virtual DbSet<ResidenceDAL> Residences
    {
        get; set;
    }
    public virtual DbSet<CQRSMessage> OutboxCQRSMessages
    {
        get; set;
    }

    //    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer ("Server=.\\sqlexpress;Database=trips;Trusted_Connection=true;TrustServerCertificate=true");

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DestinationDAL> (entity =>
        {
            entity.HasKey (e => e.Code).HasName ("PK__Destinations__357D4CF8ABF0313B");

            entity.ToTable ("destinations");

            entity.Property (e => e.Code)
                .HasMaxLength (5)
                .IsUnicode (false)
                .IsFixedLength ()
                .HasColumnName ("code");
            entity.Property (e => e.PlaceName)
                .HasMaxLength (20)
                .IsUnicode (false)
                .HasColumnName ("placename");

            entity.HasOne (d => d.Country).WithMany (p => p.Destinations)
                .HasForeignKey (d => d.CountryId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("Destinations_Countries");
        });

        modelBuilder.Entity<BookingDAL> (entity =>
        {
            entity.HasKey (e => e.Id).HasName ("PK__bookings__3213E83FE0274BF8");

            entity.ToTable ("bookings");

            entity.Property (e => e.Id).HasColumnName ("id");
            entity.Property (e => e.NumberOfMinors).HasColumnName ("numberOfMinors");
            entity.Property (e => e.NumberOfAdults).HasColumnName ("numberOfAdults");
            entity.Property (e => e.HasCancellationInsurance).HasColumnName ("cancellationInsurance");
            entity.Property (e => e.BookedOnDate).HasColumnName ("bookedondate");
            entity.Property (e => e.ClientId).HasColumnName ("clientid");
            entity.Property (e => e.TripId).HasColumnName ("tripid");

            entity.HasOne (d => d.Client).WithMany (p => p.Bookings)
                .HasForeignKey (d => d.ClientId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("bookings_clients");

            entity.HasOne (d => d.Trip).WithMany (p => p.Bookings)
                .HasForeignKey (d => d.TripId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("bookings_trips");
        });

        modelBuilder.Entity<ClientDAL> (entity =>
        {
            entity.HasKey (e => e.Id).HasName ("PK__clients__3213E83F8BE1A164");

            entity.ToTable ("clients");

            entity.Property (e => e.Id).HasColumnName ("id");
            entity.Property (e => e.Address)
                .HasMaxLength (50)
                .IsUnicode (false)
                .HasColumnName ("address");
            entity.Property (e => e.FamilyName)
                .HasMaxLength (50)
                .IsUnicode (false)
                .HasColumnName ("familyname");
            entity.Property (e => e.FirstName)
                .HasMaxLength (50)
                .IsUnicode (false)
                .HasColumnName ("firstname");
            entity.Property (e => e.ResidenceId).HasColumnName ("residenceid");

            entity.HasOne (d => d.Residence).WithMany (p => p.Clients)
                .HasForeignKey (d => d.ResidenceId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("clients_residences");
        });

        modelBuilder.Entity<CountryDAL> (entity =>
        {
            entity.HasKey (e => e.Id).HasName ("PK__Countries__3213E83FF994C39E");

            entity.ToTable ("countries");

            entity.HasIndex (e => e.Name, "UQ__Countries__72E1CD78CA87044C").IsUnique ();

            entity.Property (e => e.Id).HasColumnName ("id");
            entity.Property (e => e.Name)
                .HasMaxLength (50)
                .IsUnicode (false)
                .HasColumnName ("name");
            entity.Property (e => e.Continentid).HasColumnName ("Continentid");

            entity.HasOne (d => d.Continent).WithMany (p => p.Countries)
                .HasForeignKey (d => d.Continentid)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("Countries_continents");
        });

        modelBuilder.Entity<TripDAL> (entity =>
        {
            entity.HasKey (e => e.Id).HasName ("PK__trips__3213E83F18B8A173");

            entity.ToTable ("trips");

            entity.Property (e => e.Id).HasColumnName ("id");
            entity.Property (e => e.NumberOfDays).HasColumnName ("numberOfDays");
            entity.Property (e => e.NumberOfMinors).HasColumnName ("numberOfMinors");
            entity.Property (e => e.NumberOfAdults).HasColumnName ("numberOfAdults");
            entity.Property (e => e.DestinationCode)
                .HasMaxLength (5)
                .IsUnicode (false)
                .IsFixedLength ()
                .HasColumnName ("DestinationCode");
            entity.Property (e => e.PricePerPerson)
                .HasColumnType ("decimal(10, 2)")
                .HasColumnName ("pricePerPerson");
            entity.Property (e => e.DateOfDeparture).HasColumnName ("dateofdeparture");

            entity.HasOne (d => d.Destination).WithMany (p => p.Trips)
                .HasForeignKey (d => d.DestinationCode)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("trips_Destinations");
        });

        modelBuilder.Entity<ContinentDAL> (entity =>
        {
            entity.HasKey (e => e.Id).HasName ("PK__continents__3213E83FCC03D392");

            entity.ToTable ("continents");

            entity.HasIndex (e => e.Name, "UQ__continents__72E1CD7890D795ED").IsUnique ();

            entity.Property (e => e.Id).HasColumnName ("id");
            entity.Property (e => e.Name)
                .HasMaxLength (50)
                .IsUnicode (false)
                .HasColumnName ("name");
        });

        modelBuilder.Entity<ResidenceDAL> (entity =>
        {
            entity.HasKey (e => e.Id).HasName ("PK__residences__3213E83F42709BCC");

            entity.ToTable ("residences");

            entity.Property (e => e.Id).HasColumnName ("id");
            entity.Property (e => e.Name)
                .HasMaxLength (50)
                .IsUnicode (false)
                .HasColumnName ("name");
            entity.Property (e => e.PostalCode).HasColumnName ("postalcode");
        });

        OnModelCreatingPartial (modelBuilder);
    }
    partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
}

