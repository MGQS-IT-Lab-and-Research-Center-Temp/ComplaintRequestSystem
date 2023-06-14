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
        public IActionResult Index()
        {
            var response = _departmentService.GetAllDepartment();
            ViewData["Message"] = response.Message;
            ViewData["Status"] = response.Status;

            return View(response.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentViewModel request)
        {
            var response = _departmentService.CreateDepartment(request);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);
                return View(request);
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Department");
        }

        public IActionResult GetDepartment(string id)
        {
            var response = _departmentService.GetDepartment(id);

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
        public IActionResult Update(string id, UpdateDepartmentViewModel request)
        {
            var response = _departmentService.UpdateDepartment(id, request);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);
                return View(request);
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Department");
        }

        [HttpPost]
        public IActionResult DeleteDepartment([FromRoute] string id)
        {
            var response = _departmentService.DeleteDepartment(id);

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
