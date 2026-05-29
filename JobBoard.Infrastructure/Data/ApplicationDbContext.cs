using Microsoft.EntityFrameworkCore;
using JobBoard.Domain.Entities;

namespace JobBoard.Infrastructure.Data
{
    // DbContext: Lớp trung tâm của Entity Framework Core
    // Sử dụng TPH (Table Per Hierarchy) cho kế thừa User → Candidate/Employer
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Candidate> Candidates => Set<Candidate>();
        public DbSet<Employer> Employers => Set<Employer>();
        public DbSet<JobPost> JobPosts => Set<JobPost>();
        public DbSet<JobApplication> JobApplications => Set<JobApplication>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Candidate>("Candidate")
                .HasValue<Employer>("Employer");
                
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasField("_passwordHash");

            modelBuilder.Entity<JobPost>(entity =>
            {
                entity.HasKey(j => j.Id);
                entity.Property(j => j.Title).IsRequired().HasMaxLength(200);
                entity.Property(j => j.Description).IsRequired();
                entity.Property(j => j.Salary).HasColumnType("decimal(18,2)");
                entity.Property(j => j.Location).IsRequired().HasMaxLength(100);
                entity.HasOne(j => j.Employer).WithMany(e => e.JobPosts)
                      .HasForeignKey(j => j.EmployerId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<JobApplication>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasOne(a => a.Candidate).WithMany(c => c.Applications)
                      .HasForeignKey(a => a.CandidateId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.JobPost).WithMany(j => j.Applications)
                      .HasForeignKey(a => a.JobPostId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(a => new { a.CandidateId, a.JobPostId }).IsUnique();
            });
        }
    }
}
