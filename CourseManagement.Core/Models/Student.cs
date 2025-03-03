using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Core.Models
{
    public class Student
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre y apellido")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime BirthDate { get; set; }
        
        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "El formato de teléfono no es válido")]
        public string PhoneNumber { get; set; }
        
        // Navegación
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}