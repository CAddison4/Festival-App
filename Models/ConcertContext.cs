using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TeamRedInternalProject.Models
{
    public partial class ConcertContext : DbContext
    {
        public ConcertContext()
        {
        }

        public ConcertContext(DbContextOptions<ConcertContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artist> Artists { get; set; } = null!;
        public virtual DbSet<Festival> Festivals { get; set; } = null!;
        public virtual DbSet<FestivalTicketType> FestivalTicketTypes { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<TicketType> TicketTypes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ADAM_PC\\SSD_SQL_SERVER;Database=Concert;Trusted_Connection=True;TrustServerCertificate=True ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("Artist");

                entity.Property(e => e.ArtistId).HasColumnName("artistID");

                entity.Property(e => e.ArtistName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("artistName");
            });

            modelBuilder.Entity<Festival>(entity =>
            {
                entity.ToTable("Festival");

                entity.Property(e => e.FestivalId).HasColumnName("festivalID");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.HasMany(d => d.Artists)
                    .WithMany(p => p.Festivals)
                    .UsingEntity<Dictionary<string, object>>(
                        "FestivalArtist",
                        l => l.HasOne<Artist>().WithMany().HasForeignKey("ArtistId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FestivalA__artis__29221CFB"),
                        r => r.HasOne<Festival>().WithMany().HasForeignKey("FestivalId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FestivalA__festi__282DF8C2"),
                        j =>
                        {
                            j.HasKey("FestivalId", "ArtistId").HasName("PK__Festival__8C7ED65C7CA0F3BA");

                            j.ToTable("FestivalArtist");

                            j.IndexerProperty<int>("FestivalId").HasColumnName("festivalID");

                            j.IndexerProperty<int>("ArtistId").HasColumnName("artistID");
                        });
            });

            modelBuilder.Entity<FestivalTicketType>(entity =>
            {
                entity.HasKey(e => new { e.FestivalId, e.TicketTypeId })
                    .HasName("PK__Festival__B5921AABDEB1D4D5");

                entity.ToTable("FestivalTicketType");

                entity.Property(e => e.FestivalId).HasColumnName("festivalID");

                entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeID");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Festival)
                    .WithMany(p => p.FestivalTicketTypes)
                    .HasForeignKey(d => d.FestivalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FestivalT__festi__2DE6D218");

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.FestivalTicketTypes)
                    .HasForeignKey(d => d.TicketTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FestivalT__ticke__2EDAF651");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("orderDate");

                entity.HasOne(d => d.EmailNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Email)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__email__2180FB33");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.Property(e => e.TicketId).HasColumnName("ticketID");

                entity.Property(e => e.FestivalId).HasColumnName("festivalID");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeID");

                entity.HasOne(d => d.Festival)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.FestivalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ticket__festival__31B762FC");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ticket__orderID__339FAB6E");

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.TicketTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ticket__ticketTy__32AB8735");
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.ToTable("TicketType");

                entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeID");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__User__AB6E6165D36BCBB7");

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
}
