using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Core.Interfaces;
using CourseManagement.Core.Models;
using CourseManagement.Core.Interfaces.Repositories;

namespace CourseManagement.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public CourseService(ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository)
        {
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllWithDetailsAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
        {
            return await _courseRepository.GetActiveCoursesAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByTeacherAsync(int teacherId)
        {
            return await _courseRepository.GetByTeacherIdAsync(teacherId);
        }

        public async Task<(bool Success, string Message)> CreateCourseAsync(Course course)
        {
            try
            {
                if (course.StartDate > course.EndDate)
                {
                    return (false, "La fecha de inicio no puede ser posterior a la fecha de fin.");
                }

                await _courseRepository.AddAsync(course);
                return (true, "Curso creado con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear el curso: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateCourseAsync(Course course)
        {
            try
            {
                if (course.StartDate > course.EndDate)
                {
                    return (false, "La fecha de inicio no puede ser posterior a la fecha de fin.");
                }

                await _courseRepository.UpdateAsync(course);
                return (true, "Curso actualizado con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar el curso: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteCourseAsync(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdWithDetailsAsync(id);
                if (course == null)
                    return (false, "El curso no existe.");

                // Verificar si hay inscripciones activas
                var enrollments = await _enrollmentRepository.GetByCourseIdAsync(id);
                if (enrollments.Count > 0)
                {
                    return (false, "No se puede eliminar un curso con inscripciones activas.");
                }

                await _courseRepository.DeleteAsync(id);
                return (true, "Curso eliminado con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar el curso: {ex.Message}");
            }
        }

        public async Task<int> GetEnrolledStudentsCountAsync(int courseId)
        {
            var enrollments = await _enrollmentRepository.GetByCourseIdAsync(courseId);
            return enrollments.Count;
        }
    }
}