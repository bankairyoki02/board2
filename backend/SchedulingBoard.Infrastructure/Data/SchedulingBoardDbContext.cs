using Microsoft.EntityFrameworkCore;
using SchedulingBoard.Application.Interfaces;
using SchedulingBoard.Domain.Entities;

namespace SchedulingBoard.Infrastructure.Data;

public class SchedulingBoardDbContext : DbContext, ISchedulingBoardDbContext
{
    public SchedulingBoardDbContext(DbContextOptions<SchedulingBoardDbContext> options) : base(options)
    {
    }

    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseGroup> WarehouseGroups => Set<WarehouseGroup>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<PartReference> PartReferences => Set<PartReference>();
    public DbSet<ProjectPart> ProjectParts => Set<ProjectPart>();
    public DbSet<ScheduleItem> ScheduleItems => Set<ScheduleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Warehouse
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.WarehouseCode).HasMaxLength(10).IsRequired();
            entity.Property(e => e.CompanyCode).HasMaxLength(10);
            entity.Property(e => e.CompanyDesc).HasMaxLength(100);
            entity.Property(e => e.CountryCode).HasMaxLength(5);
            entity.Property(e => e.LocationCode).HasMaxLength(10);
            entity.Property(e => e.Abbreviation).HasMaxLength(20);
            entity.Property(e => e.Division).HasMaxLength(10);
            entity.Property(e => e.DivisionDesc).HasMaxLength(100);
            entity.Property(e => e.DefaultCrewOpsEmpno).HasMaxLength(20);
            entity.Property(e => e.DefaultCrewOpsName).HasMaxLength(100);
            entity.Property(e => e.DefaultCrewOpsEmail).HasMaxLength(100);
            entity.Property(e => e.TouringRevenueGroup).HasMaxLength(50);

            entity.HasIndex(e => e.WarehouseCode).IsUnique();
        });

        // Configure WarehouseGroup
        modelBuilder.Entity<WarehouseGroup>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GroupName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(200);

            entity.HasIndex(e => e.GroupName).IsUnique();
        });

        // Configure Project
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProjectNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ProjectName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.EntityNo).HasMaxLength(50);
            entity.Property(e => e.EntityDesc).HasMaxLength(200);

            entity.HasIndex(e => e.ProjectNumber).IsUnique();
        });

        // Configure Part
        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PartNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.PartDescription).HasMaxLength(200);
            entity.Property(e => e.JobType).HasMaxLength(20);
            entity.Property(e => e.JobDescription).HasMaxLength(200);
            entity.Property(e => e.UnitOfMeasure).HasMaxLength(10);
            entity.Property(e => e.StandardCost).HasPrecision(18, 4);

            entity.HasIndex(e => e.PartNumber).IsUnique();
        });

        // Configure PartReference
        modelBuilder.Entity<PartReference>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Factor).HasPrecision(18, 4);
            entity.Property(e => e.ReferenceType).HasMaxLength(50);

            entity.HasOne(e => e.ParentPart)
                .WithMany(e => e.ParentParts)
                .HasForeignKey(e => e.ParentPartId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ChildPart)
                .WithMany(e => e.ChildParts)
                .HasForeignKey(e => e.ChildPartId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ProjectPart
        modelBuilder.Entity<ProjectPart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitCost).HasPrecision(18, 4);
            entity.Property(e => e.TotalCost).HasPrecision(18, 4);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(e => e.Project)
                .WithMany(e => e.ProjectParts)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Part)
                .WithMany(e => e.ProjectParts)
                .HasForeignKey(e => e.PartId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ScheduleItem
        modelBuilder.Entity<ScheduleItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.Project)
                .WithMany(e => e.ScheduleItems)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Warehouse)
                .WithMany(e => e.ScheduleItems)
                .HasForeignKey(e => e.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Part)
                .WithMany()
                .HasForeignKey(e => e.PartId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure many-to-many relationship between Warehouse and WarehouseGroup
        modelBuilder.Entity<Warehouse>()
            .HasMany(w => w.WarehouseGroups)
            .WithMany(wg => wg.Warehouses)
            .UsingEntity(j => j.ToTable("WarehouseGroupWarehouses"));
    }
}