using CourseManagement.Core.Interfaces;
using CourseManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace CourseManagement.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ITeacherService _teacherService;
        private readonly IEnrollmentService _enrollmentService;

        public CoursesController(ICourseService courseService, ITeacherService teacherService, IEnrollmentService enrollmentService)
        {
            _courseService = courseService;
            _teacherService = teacherService;
            _enrollmentService = enrollmentService;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchString, CourseStatus? status)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["StatusFilter"] = status;

            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _courseService.GetCourseWithDetailsAsync(id.Value);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            ViewBag.Teachers = new SelectList(teachers, "ID", "FullName");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,StartDate,EndDate,Capacity,Status,TeacherID")] Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseService.AddCourseAsync(course);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            var teachers = await _teacherService.GetAllTeachersAsync();
            ViewBag.Teachers = new SelectList(teachers, "ID", "FullName", course.TeacherID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _courseService.GetCourseByIdAsync(id.Value);
            if (course == null)
                return NotFound();

            var teachers = await _teacherService.GetAllTeachersAsync();
            ViewBag.Teachers = new SelectList(teachers, "ID", "FullName", course.TeacherID);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,StartDate,EndDate,Capacity,Status,TeacherID")] Course course)
        {
            if (id != course.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _courseService.UpdateCourseAsync(course);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            var teachers = await _teacherService.GetAllTeachersAsync();
            ViewBag.Teachers = new SelectList(teachers, "ID", "FullName", course.TeacherID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _courseService.GetCourseByIdAsync(id.Value);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Delete), new { id = id, errorMessage = ex.Message });
            }
        }

        // GET: Courses/Enrollments/5
        public async Task<IActionResult> Enrollments(int id)
        {
            var course = await _courseService.GetCourseWithDetailsAsync(id);
            if (course == null)
                return NotFound();

            var enrollments = await _enrollmentService.GetEnrollmentsByCourseAsync(id);
            ViewBag.Course = course;
            return View(enrollments);
        }
    }
}