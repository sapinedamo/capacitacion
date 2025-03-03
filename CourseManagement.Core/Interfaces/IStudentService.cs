using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Core.Models;

namespace CourseManagement.Core.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> GetStudentWithEnrollmentsAsync(int id);
        Task<(bool Success, string Message)> CreateStudentAsync(Student student);
        Task<(bool Success, string Message)> UpdateStudentAsync(Student student);
        Task<(bool Success, string Message)> DeleteStudentAsync(int id);
    }
}