using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Core.Models
{
    public class Course
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "El nombre del curso es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre del curso")]
        public string Name { get; set; }
        
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "La fecha de finalización es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de finalización")]
        public DateTime EndDate { get; set; }
        
        [Required(ErrorMessage = "La capacidad máxima es obligatoria")]
        [Range(1, 100, ErrorMessage = "La capacidad debe estar entre 1 y 100")]
        [Display(Name = "Capacidad máxima")]
        public int Capacity { get; set; }
        
        [Display(Name = "Estado")]
        public bool IsActive { get; set; } = true;
        
        // Navegación
        public int? TeacherID { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        
        // Propiedades calculadas
        public int AvailableSpots => Capacity - (Enrollments?.Count ?? 0);
        public bool IsFull => AvailableSpots <= 0;
        public bool HasEnded => EndDate < DateTime.Now;
    }
}