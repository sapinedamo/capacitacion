using CourseManagement.Core.Interfaces;
using CourseManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CourseManagement.Web.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var teachers = await _teacherService.GetAllTeachersAsync();
            return View(teachers);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var teacher = await _teacherService.GetTeacherWithCoursesAsync(id.Value);
            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Specialty,Experience")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _teacherService.AddTeacherAsync(teacher);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var teacher = await _teacherService.GetTeacherByIdAsync(id.Value);
            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Email,Specialty,Experience")] Teacher teacher)
        {
            if (id != teacher.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _teacherService.UpdateTeacherAsync(teacher);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var teacher = await _teacherService.GetTeacherByIdAsync(id.Value);
            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _teacherService.DeleteTeacherAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Delete), new { id = id, errorMessage = ex.Message });
            }
        }
    }
}