using System;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Core.Models
{
    public enum EnrollmentStatus
    {
        [Display(Name = "Activo")]
        Active,
        [Display(Name = "Cancelado")]
        Canceled
    }

    public class Enrollment
    {
        public int ID { get; set; }
        
        public int StudentID { get; set; }
        public Student Student { get; set; }
        
        public int CourseID { get; set; }
        public Course Course { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inscripción")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        
        [Display(Name = "Estado")]
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    }
}