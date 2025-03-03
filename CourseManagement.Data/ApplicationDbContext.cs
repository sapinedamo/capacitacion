using Microsoft.EntityFrameworkCore;
using CourseManagement.Core.Models;

namespace CourseManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Course
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherID)
                .OnDelete(DeleteBehavior.SetNull);

            // Configuración de Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            // Datos de prueba
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Teachers
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { ID = 1, Name = "Juan Pérez", Email = "juan.perez@example.com", Specialty = "Programación", Experience = "10 años en desarrollo de software" },
                new Teacher { ID = 2, Name = "María López", Email = "maria.lopez@example.com", Specialty = "Base de datos", Experience = "8 años como DBA" }
            );

            // Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { 
                    ID = 1, 
                    Name = "Introducción a C#", 
                    Description = "Curso básico de programación en C#", 
                    StartDate = System.DateTime.Now.AddDays(7), 
                    EndDate = System.DateTime.Now.AddDays(37), 
                    Capacity = 20, 
                    IsActive = true, 
                    TeacherID = 1 
                },
                new Course { 
                    ID = 2, 
                    Name = "SQL Avanzado", 
                    Description = "Curso avanzado de SQL Server", 
                    StartDate = System.DateTime.Now.AddDays(14), 
                    EndDate = System.DateTime.Now.AddDays(44), 
                    Capacity = 15, 
                    IsActive = true, 
                    TeacherID = 2 
                }
            );

            // Seed Students
            modelBuilder.Entity<Student>().HasData(
                new Student { 
                    ID = 1, 
                    Name = "Carlos Rodríguez", 
                    Email = "carlos@example.com", 
                    BirthDate = new System.DateTime(1995, 5, 10), 
                    PhoneNumber = "555-123-4567" 
                },
                new Student { 
                    ID = 2, 
                    Name = "Ana Martínez", 
                    Email = "ana@example.com", 
                    BirthDate = new System.DateTime(1998, 8, 15), 
                    PhoneNumber = "555-987-6543" 
                }
            );
        }
    }
}