using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Core.Models;

namespace CourseManagement.Core.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task<Teacher> GetTeacherByIdAsync(int id);
        Task<Teacher> GetTeacherWithCoursesAsync(int id);
        Task<(bool Success, string Message)> CreateTeacherAsync(Teacher teacher);
        Task<(bool Success, string Message)> UpdateTeacherAsync(Teacher teacher);
        Task<(bool Success, string Message)> DeleteTeacherAsync(int id);
    }
}