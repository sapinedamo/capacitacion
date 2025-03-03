using CourseManagement.Core.Interfaces;
using CourseManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CourseManagement.Web.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;

        public EnrollmentsController(
            IEnrollmentService enrollmentService,
            ICourseService courseService,
            IStudentService studentService)
        {
            _enrollmentService = enrollmentService;
            _courseService = courseService;
            _studentService = studentService;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return View(enrollments);
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id.Value);
            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public async Task<IActionResult> Create()
        {
            var students = await _studentService.GetAllStudentsAsync();
            var courses = await _courseService.GetActiveCoursesAsync();
            
            ViewBag.Students = new SelectList(students, "ID", "FullName");
            ViewBag.Courses = new SelectList(courses, "ID", "Name");
            
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int studentId, int courseId)
        {
            var result = await _enrollmentService.EnrollStudentAsync(studentId, courseId);

            if (result.success)
            {
                TempData["SuccessMessage"] = result.message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var students = await _studentService.GetAllStudentsAsync();
                var courses = await _courseService.GetActiveCoursesAsync();
                
                ViewBag.Students = new SelectList(students, "ID", "FullName", studentId);
                ViewBag.Courses = new SelectList(courses, "ID", "Name", courseId);
                ViewBag.ErrorMessage = result.message;
                
                return View();
            }
        }

        // GET: Enrollments/Cancel/5
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
                return NotFound();

            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id.Value);
            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        // POST: Enrollments/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var success = await _enrollmentService.CancelEnrollmentAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "La inscripción ha sido cancelada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo cancelar la inscripción.";
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Enrollments/Student/5
        public async Task<IActionResult> Student(int? id)
        {
            if (id == null)
                return NotFound();
                
            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null)
                return NotFound();
                
            var enrollments = await _enrollmentService.GetEnrollmentsByStudentAsync(id.Value);
            ViewBag.StudentName = student.FullName;
            ViewBag.StudentId = student.ID;
            
            return View(enrollments);
        }
        
        // GET: Enrollments/Course/5
        public async Task<IActionResult> Course(int? id)
        {
            if (id == null)
                return NotFound();
                
            var course = await _courseService.GetCourseByIdAsync(id.Value);
            if (course == null)
                return NotFound();
                
            var enrollments = await _enrollmentService.GetEnrollmentsByCourseAsync(id.Value);
            ViewBag.CourseName = course.Name;
            ViewBag.CourseId = course.ID;
            
            return View(enrollments);
        }
    }
}