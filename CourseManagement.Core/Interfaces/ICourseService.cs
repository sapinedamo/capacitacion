using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Core.Models;

namespace CourseManagement.Core.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> GetActiveCoursesAsync();
        Task<IEnumerable<Course>> GetCoursesByTeacherAsync(int teacherId);
        Task<(bool Success, string Message)> CreateCourseAsync(Course course);
        Task<(bool Success, string Message)> UpdateCourseAsync(Course course);
        Task<(bool Success, string Message)> DeleteCourseAsync(int id);
        Task<int> GetEnrolledStudentsCountAsync(int courseId);
    }
}