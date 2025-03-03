using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Core.Models;

namespace CourseManagement.Core.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
        Task<Enrollment> GetEnrollmentByIdAsync(int id);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseAsync(int courseId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(int studentId);
        Task<(bool Success, string Message)> CreateEnrollmentAsync(Enrollment enrollment);
        Task<(bool Success, string Message)> UpdateEnrollmentAsync(Enrollment enrollment);
        Task<(bool Success, string Message)> DeleteEnrollmentAsync(int id);
        Task<bool> IsStudentAlreadyEnrolledAsync(int studentId, int courseId);
        Task<bool> HasCourseReachedCapacityAsync(int courseId);
        Task<bool> DoCoursesDatesOverlapAsync(int studentId, int newCourseId);
    }
}