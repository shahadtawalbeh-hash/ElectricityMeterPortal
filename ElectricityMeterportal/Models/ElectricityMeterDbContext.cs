using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ElectricityMeterportal.Models;

public partial class ElectricityMeterDbContext : DbContext
{
    public ElectricityMeterDbContext()
    {
    }

    public ElectricityMeterDbContext(DbContextOptions<ElectricityMeterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Meter> Meters { get; set; }

    public virtual DbSet<MeterReading> MeterReadings { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<ElectricityRequest> ElectricityRequests { get; set; }
    public virtual DbSet<RequestDocument> RequestDocuments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bills__11F2FC4A0CFA206C");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BillDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Consumption).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CurrentReading).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MeterId).HasColumnName("MeterID");
            entity.Property(e => e.PreviousReading).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Meter).WithMany(p => p.Bills)
                .HasForeignKey(d => d.MeterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bills__BillDate__59063A47");
        });

        modelBuilder.Entity<Meter>(entity =>
        {
            entity.HasKey(e => e.MeterId).HasName("PK__Meters__59223B8C7431AE87");

            entity.Property(e => e.MeterId).HasColumnName("MeterID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.MeterNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<MeterReading>(entity =>
        {
            entity.HasKey(e => e.ReadingId).HasName("PK__MeterRea__C80F9C6E5BE448B0");

            entity.Property(e => e.ReadingId).HasColumnName("ReadingID");
            entity.Property(e => e.MeterId).HasColumnName("MeterID");
            entity.Property(e => e.ReadingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReadingValue).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Meter).WithMany(p => p.MeterReadings)
                .HasForeignKey(d => d.MeterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MeterRead__Readi__4F7CD00D");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A58A06DB9AE");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Bill).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__BillID__5CD6CB2B");
        });

        modelBuilder.Entity<ElectricityRequest>(entity =>
        {
            entity.ToTable("ElectricityRequests");
            entity.HasKey(e => e.ID);

            entity.Property(e => e.NationalID).IsRequired();
            entity.Property(e => e.FullName).IsRequired();
            entity.Property(e => e.PhoneNumber).IsRequired();
            entity.Property(e => e.Address).IsRequired();
            entity.Property(e => e.MeterType).IsRequired();
            entity.Property(e => e.status).HasDefaultValue("Pending");

        });


        OnModelCreatingPartial(modelBuilder);

    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
