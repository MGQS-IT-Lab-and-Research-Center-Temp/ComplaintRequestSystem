using AspNetCoreHero.ToastNotification.Abstractions;
using ComplaintRequestSystem.Models.Department;
using ComplaintRequestSystem.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintRequestSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly INotyfService _notyf;

        public DepartmentController(IDepartmentService departmentService, INotyfService notyf)
        {
            _departmentService = departmentService;
            _notyf = notyf;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _departmentService.GetAllDepartment();
            ViewData["Message"] = response.Message;
            ViewData["Status"] = response.Status;

            return View(response.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentViewModel request)
        {
            var response = await _departmentService.CreateDepartment(request);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);
                return View(request);
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Department");
        }

        public async Task<IActionResult> GetDepartment(string id)
        {
            var response = await _departmentService.GetDepartment(id);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);
                return RedirectToAction("Index", "Department");
            }

            _notyf.Success(response.Message);

            return View(response.Data);

        }

        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateDepartmentViewModel request)
        {
            var response = await _departmentService.UpdateDepartment(id, request);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);
                return View(request);
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Department");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDepartment([FromRoute] string id)
        {
            var response = await _departmentService.DeleteDepartment(id);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);
                return RedirectToAction("Index", "Department");
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Department");
        }
    }
}
