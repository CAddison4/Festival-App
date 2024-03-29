﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TeamRedInternalProject.ViewModel;

namespace TeamRedInternalProject.Models;

public partial class ConcertContext : DbContext
{
    private readonly IConfiguration _configuration;
    public ConcertContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ConcertContext(DbContextOptions<ConcertContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Festival> Festivals { get; set; }

    public virtual DbSet<FestivalTicketType> FestivalTicketTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration.GetSection("ConnectionStrings")["Production"]);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("PK__Artist__4F43936775A826E0");

            entity.ToTable("Artist");

            entity.Property(e => e.ArtistId).HasColumnName("artistID");
            entity.Property(e => e.ArtistBio)
                .HasMaxLength(4095)
                .IsUnicode(false)
                .HasColumnName("artistBio");
            entity.Property(e => e.ArtistName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("artistName");
            entity.Property(e => e.ImgPath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("imgPath");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRoles",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Festival>(entity =>
        {
            entity.HasKey(e => e.FestivalId).HasName("PK__Festival__E88AEF6A24D851F7");

            entity.ToTable("Festival");

            entity.Property(e => e.FestivalId).HasColumnName("festivalID");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.IsCurrent).HasColumnName("isCurrent");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("location");

            entity.HasMany(d => d.Artists).WithMany(p => p.Festivals)
                .UsingEntity<Dictionary<string, object>>(
                    "FestivalArtist",
                    r => r.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__FestivalA__artis__4C364F0E"),
                    l => l.HasOne<Festival>().WithMany()
                        .HasForeignKey("FestivalId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__FestivalA__festi__4B422AD5"),
                    j =>
                    {
                        j.HasKey("FestivalId", "ArtistId").HasName("PK__Festival__8C7ED65CE8B10951");
                        j.ToTable("FestivalArtist");
                    });
        });

        modelBuilder.Entity<FestivalTicketType>(entity =>
        {
            entity.HasKey(e => new { e.FestivalId, e.TicketTypeId }).HasName("PK__Festival__B5921AAB104A8DF8");

            entity.ToTable("FestivalTicketType");

            entity.Property(e => e.FestivalId).HasColumnName("festivalID");
            entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Festival).WithMany(p => p.FestivalTicketTypes)
                .HasForeignKey(d => d.FestivalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FestivalT__festi__50FB042B");

            entity.HasOne(d => d.TicketType).WithMany(p => p.FestivalTicketTypes)
                .HasForeignKey(d => d.TicketTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FestivalT__ticke__51EF2864");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__0809337DE31AA855");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.OrderDate)
                .HasColumnType("date")
                .HasColumnName("orderDate");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__email__44952D46");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__3333C670B16C70F5");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("ticketID");
            entity.Property(e => e.FestivalId).HasColumnName("festivalID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeID");

            entity.HasOne(d => d.Festival).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FestivalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__festival__54CB950F");

            entity.HasOne(d => d.Order).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__orderID__56B3DD81");

            entity.HasOne(d => d.TicketType).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__ticketTy__55BFB948");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(e => e.TicketTypeId).HasName("PK__TicketTy__D18F5C147DD2A8A4");

            entity.ToTable("TicketType");

            entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeID");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.HasMany(e => e.FestivalTicketTypes);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__User__AB6E6165560BDD04");

            entity.ToTable("User");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Admin).HasColumnName("admin");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("lastName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
