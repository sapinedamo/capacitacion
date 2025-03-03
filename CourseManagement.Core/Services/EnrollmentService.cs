using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagement.Core.Interfaces;
using CourseManagement.Core.Models;
using CourseManagement.Core.Interfaces.Repositories;

namespace CourseManagement.Core.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IRepository<Student> _studentRepository;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepository,
            ICourseRepository courseRepository,
            IRepository<Student> studentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _enrollmentRepository.GetAllWithDetailsAsync();
        }

        public async Task<Enrollment> GetEnrollmentByIdAsync(int id)
        {
            return await _enrollmentRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseAsync(int courseId)
        {
            return await _enrollmentRepository.GetByCourseIdAsync(courseId);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(int studentId)
        {
            return await _enrollmentRepository.GetByStudentIdAsync(studentId);
        }

        public async Task<(bool Success, string Message)> CreateEnrollmentAsync(Enrollment enrollment)
        {
            try
            {
                // Verificar que el curso existe
                var course = await _courseRepository.GetByIdAsync(enrollment.CourseID);
                if (course == null)
                    return (false, "El curso especificado no existe.");

                // Verificar que el estudiante existe
                var student = await _studentRepository.GetByIdAsync(enrollment.StudentID);
                if (student == null)
                    return (false, "El estudiante especificado no existe.");

                // Verificar que el curso está activo y no ha finalizado
                if (!course.IsActive || course.EndDate < DateTime.Now)
                    return (false, "No se puede inscribir en un curso inactivo o finalizado.");

                // Verificar capacidad del curso
                if (await HasCourseReachedCapacityAsync(course.ID))
                    return (false, "El curso ha alcanzado su capacidad máxima.");

                // Verificar que el estudiante no esté ya inscrito
                if (await IsStudentAlreadyEnrolledAsync(enrollment.StudentID, enrollment.CourseID))
                    return (false, "El estudiante ya está inscrito en este curso.");

                // Verificar solapamiento de fechas
                if (await DoCoursesDatesOverlapAsync(enrollment.StudentID, enrollment.CourseID))
                    return (false, "El estudiante ya está inscrito en otro curso que se solapa con las fechas de este curso.");

                enrollment.EnrollmentDate = DateTime.Now;
                enrollment.Status = EnrollmentStatus.Active;
                
                await _enrollmentRepository.AddAsync(enrollment);
                return (true, "Inscripción creada con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear la inscripción: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateEnrollmentAsync(Enrollment enrollment)
        {
            try
            {
                await _enrollmentRepository.UpdateAsync(enrollment);
                return (true, "Inscripción actualizada con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar la inscripción: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteEnrollmentAsync(int id)
        {
            try
            {
                var enrollment = await _enrollmentRepository.GetByIdAsync(id);
                if (enrollment == null)
                    return (false, "La inscripción no existe.");

                await _enrollmentRepository.DeleteAsync(id);
                return (true, "Inscripción eliminada con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar la inscripción: {ex.Message}");
            }
        }

        public async Task<bool> IsStudentAlreadyEnrolledAsync(int studentId, int courseId)
        {
            var enrollments = await _enrollmentRepository.GetByStudentIdAsync(studentId);
            return enrollments.Any(e => e.CourseID == courseId && e.Status == EnrollmentStatus.Active);
        }

        public async Task<bool> HasCourseReachedCapacityAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            var activeEnrollments = await _enrollmentRepository.GetByCourseIdAsync(courseId);
            
            return activeEnrollments.Count(e => e.Status == EnrollmentStatus.Active) >= course.Capacity;
        }

        public async Task<bool> DoCoursesDatesOverlapAsync(int studentId, int newCourseId)
        {
            var newCourse = await _courseRepository.GetByIdAsync(newCourseId);
            var studentEnrollments = await _enrollmentRepository.GetByStudentIdAsync(studentId);
            
            foreach (var enrollment in studentEnrollments.Where(e => e.Status == EnrollmentStatus.Active))
            {
                var existingCourse = await _courseRepository.GetByIdAsync(enrollment.CourseID);
                
                // Verificar si hay solapamiento de fechas
                if ((newCourse.StartDate <= existingCourse.EndDate) && 
                    (newCourse.EndDate >= existingCourse.StartDate))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}