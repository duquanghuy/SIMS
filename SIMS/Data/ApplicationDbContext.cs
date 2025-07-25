using Microsoft.EntityFrameworkCore;
using SIMS.Models;

namespace SIMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<ClassSchedule> ClassSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Relationships
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<User>()
                .HasOne<Student>()
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId);

            modelBuilder.Entity<User>()
                .HasOne<Teacher>()
                .WithOne(t => t.User)
                .HasForeignKey<Teacher>(t => t.UserId);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Subject)
                .WithMany()
                .HasForeignKey(c => c.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subject>()
               .HasMany(s => s.Classes)
               .WithOne(c => c.Subject)
               .HasForeignKey(c => c.SubjectId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                   .HasOne(e => e.Student)
                   .WithMany(s => s.Enrollments)
                   .HasForeignKey(e => e.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Enrollment>()
            //       .HasOne(e => e.Class)
            //       .WithMany(c => c.ClassSchedules)
            //       .HasForeignKey(e => e.ClassId)
            //       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSchedule>()
                    .HasOne(cs => cs.Class)
                    .WithMany(c => c.ClassSchedules)
                    .HasForeignKey(cs => cs.ClassId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
