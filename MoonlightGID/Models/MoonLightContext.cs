using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MoonlightGID.Models
{
    public partial class MoonLightContext : DbContext
    {
        public MoonLightContext()
        {
        }

        public MoonLightContext(DbContextOptions<MoonLightContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Businesses> Businesses { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<Services> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=AMB3R\\AMBRIEL;Database=MoonLight;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Businesses>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.Property(e => e.ContactNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegistrationDate).HasColumnType("date");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.ContactNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("PK__Jobs__056690C2EB07A580");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Jobs_Businesses");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_Jobs_Services");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("PK__Services__C51BB00AA0F3198A");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Services_Customers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
