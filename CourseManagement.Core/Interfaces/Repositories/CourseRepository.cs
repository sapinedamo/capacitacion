using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Core.Interfaces.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetCoursesWithTeachersAsync()
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .ToListAsync();
        }

        public async Task<Course> GetCourseWithDetailsAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
        {
            return await _context.Courses
                .Where(c => c.Status == CourseStatus.Active)
                .ToListAsync();
        }
    }
}