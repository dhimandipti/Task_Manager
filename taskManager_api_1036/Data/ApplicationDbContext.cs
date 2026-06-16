using Microsoft.EntityFrameworkCore;
using taskManager_api_1036.Model;

namespace taskManager_api_1036.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskManagement> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // User -> Assigned Tasks Relationship

            modelBuilder.Entity<TaskManagement>()
                .HasOne(t => t.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.Restrict);



            // User -> Created Tasks Relationship

            modelBuilder.Entity<TaskManagement>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);



            base.OnModelCreating(modelBuilder);

        }

    }
}
