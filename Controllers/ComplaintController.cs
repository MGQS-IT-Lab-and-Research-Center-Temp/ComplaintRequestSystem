using AspNetCoreHero.ToastNotification.Abstractions;
using ComplaintRequestSystem.Models.Complaint;
using ComplaintRequestSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintRequestSystem.Controllers
{
	public class ComplaintController : Controller
    {
        private readonly IComplaintService _complaintService;
        private readonly IDepartmentService _departmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotyfService _notyf;

        public ComplaintController(
            IComplaintService complaintService,
            IDepartmentService departmentService,
            IHttpContextAccessor httpContextAccessor,
            INotyfService notyf)
        {
            _complaintService = complaintService;
            _departmentService = departmentService;
            _httpContextAccessor = httpContextAccessor;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            var complaints = _complaintService.GetAllComplaint();
            ViewData["Message"] = complaints.Message;
            ViewData["Status"] = complaints.Status;

            return View(complaints.Data);
        }


		
       
		public IActionResult Create()
        {
            ViewBag.Departments = _departmentService.SelectDepartment();
            ViewData["Message"] = "";
            ViewData["Status"] = false;

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateComplaintViewModel request)
        {
            var response = _complaintService.CreateComplaint(request);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);
                return View();
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Complaint");
        }

        public IActionResult GetComplaintByDepartment(string id)
        {
            var response = _complaintService.GetComplaintsByDepartmentId(id);
            ViewData["Message"] = response.Message;
            ViewData["Status"] = response.Status;

            return View(response.Data);
        }

        public IActionResult GetComplaintDetail(string id)
        {
            var response = _complaintService.GetComplaint(id);
            ViewData["Message"] = response.Message;
            ViewData["Status"] = response.Status;

            return View(response.Data);
        }

        public IActionResult Update(string id)
        {
            var response = _complaintService.GetComplaint(id);

            return View(response.Data);
        }

        [HttpPost]
        public IActionResult Update(string id, UpdateComplaintViewModel request)
        {
            var response = _complaintService.UpdateComplaint(id, request);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);

                return RedirectToAction("Index", "Home");
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Complaint");
        }

        [HttpPost]
        public IActionResult DeleteComplaint([FromRoute] string id)
        {
            var response = _complaintService.DeleteComplaint(id);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);
                return View();
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Complaint");
        }
    }
}
