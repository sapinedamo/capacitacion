using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Core.Models;

namespace CourseManagement.Core.Interfaces.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetCoursesWithTeachersAsync();
        Task<Course> GetCourseWithDetailsAsync(int id);
        Task<IEnumerable<Course>> GetActiveCoursesAsync();
    }
}