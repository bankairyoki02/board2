using Microsoft.EntityFrameworkCore;
using SchedulingBoard.Domain.Entities;

namespace SchedulingBoard.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(SchedulingBoardDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // Check if data already exists
        if (await context.Warehouses.AnyAsync())
        {
            return; // Database has been seeded
        }

        // Seed Warehouses
        var warehouses = new List<Warehouse>
        {
            new Warehouse
            {
                WarehouseCode = "WH001",
                CompanyCode = "COMP01",
                CompanyDesc = "Main Production Facility",
                CountryCode = "US",
                LocationCode = "NY001",
                Abbreviation = "MPF",
                Division = "PROD",
                DivisionDesc = "Production Division",
                DefaultCrewOpsEmpno = "EMP001",
                DefaultCrewOpsName = "John Smith",
                DefaultCrewOpsEmail = "john.smith@company.com",
                TouringRevenueGroup = "TRG-A",
                IsActive = true
            },
            new Warehouse
            {
                WarehouseCode = "WH002",
                CompanyCode = "COMP01",
                CompanyDesc = "Secondary Warehouse",
                CountryCode = "US",
                LocationCode = "CA001",
                Abbreviation = "SW",
                Division = "DIST",
                DivisionDesc = "Distribution Division",
                DefaultCrewOpsEmpno = "EMP002",
                DefaultCrewOpsName = "Jane Doe",
                DefaultCrewOpsEmail = "jane.doe@company.com",
                TouringRevenueGroup = "TRG-B",
                IsActive = true
            }
        };

        context.Warehouses.AddRange(warehouses);
        await context.SaveChangesAsync();

        // Seed Projects
        var projects = new List<Project>
        {
            new Project
            {
                ProjectNumber = "PRJ-2024-001",
                ProjectName = "Q1 Production Run",
                Description = "First quarter production schedule",
                StartDate = DateTime.Today.AddDays(-30),
                EndDate = DateTime.Today.AddDays(60),
                Status = "Active",
                EntityNo = "ENT001",
                EntityDesc = "Production Entity",
                IsActive = true
            },
            new Project
            {
                ProjectNumber = "PRJ-2024-002",
                ProjectName = "Special Order Processing",
                Description = "Custom order fulfillment project",
                StartDate = DateTime.Today.AddDays(-15),
                EndDate = DateTime.Today.AddDays(45),
                Status = "Active",
                EntityNo = "ENT002",
                EntityDesc = "Special Orders Entity",
                IsActive = true
            }
        };

        context.Projects.AddRange(projects);
        await context.SaveChangesAsync();

        // Seed Parts
        var parts = new List<Part>
        {
            new Part
            {
                PartNumber = "PART-001",
                PartDescription = "Standard Widget Component",
                JobType = "ASSEMBLY",
                JobDescription = "Widget Assembly Job",
                IsQualification = false,
                IsActive = true,
                StandardCost = 25.50m,
                UnitOfMeasure = "EA"
            },
            new Part
            {
                PartNumber = "PART-002",
                PartDescription = "Premium Widget Component",
                JobType = "ASSEMBLY",
                JobDescription = "Premium Widget Assembly",
                IsQualification = true,
                IsActive = true,
                StandardCost = 45.75m,
                UnitOfMeasure = "EA"
            }
        };

        context.Parts.AddRange(parts);
        await context.SaveChangesAsync();

        // Seed Schedule Items
        var scheduleItems = new List<ScheduleItem>
        {
            new ScheduleItem
            {
                ProjectId = projects[0].Id,
                WarehouseId = warehouses[0].Id,
                PartId = parts[0].Id,
                ScheduledDate = DateTime.Today.AddDays(5),
                Quantity = 100,
                Status = "Planned",
                IsLate = false,
                IsProposal = false,
                Notes = "Standard production run"
            },
            new ScheduleItem
            {
                ProjectId = projects[0].Id,
                WarehouseId = warehouses[1].Id,
                PartId = parts[1].Id,
                ScheduledDate = DateTime.Today.AddDays(10),
                Quantity = 50,
                Status = "Planned",
                IsLate = false,
                IsProposal = true,
                Notes = "Proposed premium run"
            },
            new ScheduleItem
            {
                ProjectId = projects[1].Id,
                WarehouseId = warehouses[0].Id,
                PartId = parts[0].Id,
                ScheduledDate = DateTime.Today.AddDays(-2),
                Quantity = 25,
                Status = "In Progress",
                IsLate = true,
                IsProposal = false,
                Notes = "Urgent special order - running late"
            },
            new ScheduleItem
            {
                ProjectId = projects[1].Id,
                WarehouseId = warehouses[1].Id,
                PartId = parts[1].Id,
                ScheduledDate = DateTime.Today.AddDays(15),
                Quantity = 75,
                Status = "Planned",
                IsLate = false,
                IsProposal = false,
                Notes = "Future delivery scheduled"
            }
        };

        context.ScheduleItems.AddRange(scheduleItems);
        await context.SaveChangesAsync();
    }
}