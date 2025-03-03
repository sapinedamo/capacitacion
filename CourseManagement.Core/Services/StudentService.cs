using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagement.Core.Interfaces;
using CourseManagement.Core.Models;
using CourseManagement.Core.Interfaces.Repositories;

namespace CourseManagement.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public StudentService(IRepository<Student> studentRepository, IEnrollmentRepository enrollmentRepository)
        {
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetByIdAsync(id);
        }

        public async Task<Student> GetStudentWithEnrollmentsAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student != null)
            {
                student.Enrollments = await _enrollmentRepository.GetByStudentIdAsync(id);
            }
            return student;
        }

        public async Task<(bool Success, string Message)> CreateStudentAsync(Student student)
        {
            try
            {
                await _studentRepository.AddAsync(student);
                return (true, "Estudiante creado con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear el estudiante: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateStudentAsync(Student student)
        {
            try
            {
                await _studentRepository.UpdateAsync(student);
                return (true, "Estudiante actualizado con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar el estudiante: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteStudentAsync(int id)
        {
            try
            {
                // Verificar que el estudiante existe
                var student = await _studentRepository.GetByIdAsync(id);
                if (student == null)
                {
                    return (false, "El estudiante no existe.");
                }

                // Verificar si tiene inscripciones
                var enrollments = await _enrollmentRepository.GetByStudentIdAsync(id);
                if (enrollments.Count > 0)
                {
                    return (false, "No se puede eliminar un estudiante con inscripciones.");
                }

                await _studentRepository.DeleteAsync(id);
                return (true, "Estudiante eliminado con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar el estudiante: {ex.Message}");
            }
        }
    }
}