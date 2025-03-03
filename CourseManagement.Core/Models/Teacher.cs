using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Core.Models
{
    public class Teacher
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
        
        [Display(Name = "Especialidad")]
        public string Specialty { get; set; }
        
        [Display(Name = "Experiencia")]
        public string Experience { get; set; }
        
        // Navegación
        public ICollection<Course> Courses { get; set; }
    }
}