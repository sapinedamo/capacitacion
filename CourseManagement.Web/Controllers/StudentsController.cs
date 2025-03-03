using CourseManagement.Core.Interfaces;
using CourseManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CourseManagement.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IEnrollmentService _enrollmentService;

        public StudentsController(IStudentService studentService, IEnrollmentService enrollmentService)
        {
            _studentService = studentService;
            _enrollmentService = enrollmentService;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var students = await _studentService.GetAllStudentsAsync();
            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentService.GetStudentWithEnrollmentsAsync(id.Value);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,DateOfBirth,PhoneNumber")] Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.AddStudentAsync(student);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Email,DateOfBirth,PhoneNumber")] Student student)
        {
            if (id != student.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.UpdateStudentAsync(student);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Delete), new { id = id, errorMessage = ex.Message });
            }
        }

        // GET: Students/Enrollments/5
        public async Task<IActionResult> Enrollments(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();

            var enrollments = await _enrollmentService.GetEnrollmentsByStudentAsync(id);
            ViewBag.Student = student;
            return View(enrollments);
        }
    }
}