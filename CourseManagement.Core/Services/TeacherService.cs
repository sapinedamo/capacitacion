using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Core.Interfaces;
using CourseManagement.Core.Models;
using CourseManagement.Core.Interfaces.Repositories;

namespace CourseManagement.Core.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly ICourseRepository _courseRepository;

        public TeacherService(IRepository<Teacher> teacherRepository, ICourseRepository courseRepository)
        {
            _teacherRepository = teacherRepository;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _teacherRepository.GetAllAsync();
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            return await _teacherRepository.GetByIdAsync(id);
        }

        public async Task<Teacher> GetTeacherWithCoursesAsync(int id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            if (teacher != null)
            {
                teacher.Courses = await _courseRepository.GetByTeacherIdAsync(id);
            }
            return teacher;
        }

        public async Task<(bool Success, string Message)> CreateTeacherAsync(Teacher teacher)
        {
            try
            {
                await _teacherRepository.AddAsync(teacher);
                return (true, "Profesor creado con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear el profesor: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateTeacherAsync(Teacher teacher)
        {
            try
            {
                await _teacherRepository.UpdateAsync(teacher);
                return (true, "Profesor actualizado con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar el profesor: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteTeacherAsync(int id)
        {
            try
            {
                // Verificar que el profesor existe
                var teacher = await _teacherRepository.GetByIdAsync(id);
                if (teacher == null)
                {
                    return (false, "El profesor no existe.");
                }

                // Verificar si tiene cursos asignados
                var courses = await _courseRepository.GetByTeacherIdAsync(id);
                if (courses.Count > 0)
                {
                    return (false, "No se puede eliminar un profesor con cursos asignados.");
                }

                await _teacherRepository.DeleteAsync(id);
                return (true, "Profesor eliminado con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar el profesor: {ex.Message}");
            }
        }
    }
}