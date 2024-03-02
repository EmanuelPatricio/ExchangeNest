using Application.Abstractions.Data;
using Domain.Countries;
using Domain.ExchangePrograms;
using Domain.GenericStatuses;
using Domain.Organizations;
using Domain.States;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<ExchangeProgram> ExchangePrograms { get; set; }
    public DbSet<GenericStatus> GenericStatuses { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Domain.Applications.Application> Applications { get; set; }
    public DbSet<Domain.Applications.ApplicationDocument> ApplicationDocuments { get; set; }
    public DbSet<Domain.Configurations.Configuration> Configurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await base.SaveChangesAsync(ct);
    }
}
