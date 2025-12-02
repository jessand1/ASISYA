using Microsoft.EntityFrameworkCore;
using ProviderOptimizer.Domain.Entities;

namespace ProviderOptimizer.Persistence
{
    public class ProviderOptimizerDbContext : DbContext
    {
        public ProviderOptimizerDbContext(DbContextOptions<ProviderOptimizerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Provider> Providers { get; set; } = null!;
        public DbSet<ProviderAvailability> ProviderAvailabilities { get; set; } = null!;
        public DbSet<ProviderLocation> ProviderLocations { get; set; } = null!;
        public DbSet<ProviderWorkload> ProviderWorkloads { get; set; } = null!;
        public DbSet<OptimizationRequest> OptimizationRequests { get; set; } = null!;
        public DbSet<OptimizationResult> OptimizationResults { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("optimizer");

            modelBuilder.Entity<Provider>(b =>
            {
                b.ToTable("providers");
                b.HasKey(x => x.ProviderId);
                b.Property(x => x.Name)
                 .HasMaxLength(150)
                 .IsRequired();
                b.Property(x => x.Phone)
                 .HasMaxLength(20);
                b.Property(x => x.ServiceTypes)
                 .HasMaxLength(400)
                 .IsRequired();
                b.Property(x => x.Rating)
                 .HasColumnType("decimal(3,2)")
                 .HasDefaultValue(5.00m);
                b.Property(x => x.CreatedAt)
                 .HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasOne(x => x.Availability)
                 .WithOne(a => a.Provider)
                 .HasForeignKey<ProviderAvailability>(a => a.ProviderId);
                b.HasOne(x => x.Location)
                 .WithOne(l => l.Provider)
                 .HasForeignKey<ProviderLocation>(l => l.ProviderId);
                b.HasOne(x => x.Workload)
                 .WithOne(w => w.Provider)
                 .HasForeignKey<ProviderWorkload>(w => w.ProviderId);
                b.HasMany(x => x.OptimizationResults)
                 .WithOne(r => r.Provider)
                 .HasForeignKey(r => r.ProviderId);
            });

            modelBuilder.Entity<ProviderAvailability>(b =>
            {
                b.ToTable("provider_availability");
                b.HasKey(x => x.ProviderId);
                b.Property(x => x.IsAvailable)
                 .HasDefaultValue(true);
                b.Property(x => x.LastUpdate)
                 .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            modelBuilder.Entity<ProviderLocation>(b =>
            {
                b.ToTable("provider_location");
                b.HasKey(x => x.ProviderId);
                b.Property(x => x.Latitude)
                 .HasColumnType("decimal(10,6)");
                b.Property(x => x.Longitude)
                 .HasColumnType("decimal(10,6)");
                b.Property(x => x.UpdatedAt)
                 .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            modelBuilder.Entity<ProviderWorkload>(b =>
            {
                b.ToTable("provider_workload");
                b.HasKey(x => x.ProviderId);
                b.Property(x => x.ActiveCases)
                 .HasDefaultValue(0);
                b.Property(x => x.UpdatedAt)
                 .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            modelBuilder.Entity<OptimizationRequest>(b =>
            {
                b.ToTable("optimization_requests");
                b.HasKey(x => x.RequestId);
                b.Property(x => x.ClientLatitude)
                 .HasColumnType("decimal(10,6)");
                b.Property(x => x.ClientLongitude)
                 .HasColumnType("decimal(10,6)");
                b.Property(x => x.ServiceType)
                 .HasMaxLength(50)
                 .IsRequired();
                b.Property(x => x.CreatedAt)
                 .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            modelBuilder.Entity<OptimizationResult>(b =>
            {
                b.ToTable("optimization_results");
                b.HasKey(x => x.ResultId);
                b.Property(x => x.EtaMinutes)
                 .IsRequired();
                b.Property(x => x.DistanceKm)
                 .HasColumnType("decimal(6,2)");
                b.Property(x => x.Score)
                 .HasColumnType("decimal(5,2)");
                b.Property(x => x.CreatedAt)
                 .HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasOne(x => x.Request)
                 .WithMany(r => r.Results)
                 .HasForeignKey(x => x.RequestId);
            });
        }
    }
}
