using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace _2021IMMS.Models
{
    public partial class _2021IMMSContext : DbContext
    {
        public _2021IMMSContext()
        {
        }

        public _2021IMMSContext(DbContextOptions<_2021IMMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Instrument> Instrument { get; set; }
        public virtual DbSet<Piece> Piece { get; set; }
        public virtual DbSet<RentalAgreement> RentalAgreement { get; set; }
        public virtual DbSet<Student> Student { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-300IG72\\SQLEXPRESS; Database=2021IMMS; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.ToTable("Administrator");

                entity.Property(e => e.AdministratorId).HasColumnName("AdministratorID");

                entity.Property(e => e.EmailAddress).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleInitial).HasMaxLength(1);

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Status).HasMaxLength(1);
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.Brand1)
                    .HasMaxLength(50)
                    .HasColumnName("Brand")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.ToTable("Instrument");

                entity.Property(e => e.InstrumentId).HasColumnName("InstrumentID");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.Condition).HasMaxLength(1);

                entity.Property(e => e.Instrument1)
                    .HasMaxLength(50)
                    .HasColumnName("Instrument");

                entity.Property(e => e.SerialNumber).HasMaxLength(20);

                entity.Property(e => e.Type).HasMaxLength(1);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Instruments)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Instrument_Brand");
            });

            modelBuilder.Entity<Piece>(entity =>
            {
                entity.ToTable("Piece");

                entity.Property(e => e.PieceId).HasColumnName("PieceID");

                entity.Property(e => e.Arranger).HasMaxLength(50);

                entity.Property(e => e.Composer).HasMaxLength(50);

                entity.Property(e => e.DateLastPlayed).HasColumnType("date");

                entity.Property(e => e.Type).HasMaxLength(1);
            });

            modelBuilder.Entity<RentalAgreement>(entity =>
            {
                entity.ToTable("RentalAgreement");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RentalAgreementID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.FacultySignature).HasMaxLength(50);

                entity.Property(e => e.InstrumentId).HasColumnName("InstrumentID");

                entity.Property(e => e.RenterSignature).HasMaxLength(50);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.HasOne(d => d.Instrument)
                    .WithMany(p => p.RentalAgreements)
                    .HasForeignKey(d => d.InstrumentId)
                    .HasConstraintName("FK_RentalAgreement_Instrument");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.RentalAgreements)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_RentalAgreement_Student");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.EmailAddress).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleInitial).HasMaxLength(1);

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.State).HasMaxLength(2);

                entity.Property(e => e.ZipCode).HasMaxLength(5);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
