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

        public async Task<IActionResult> Index()
        {
            var complaints = await _complaintService.GetAllComplaint();
            ViewData["Message"] = complaints.Message;
            ViewData["Status"] = complaints.Status;

            return View(complaints.Data);
        }


		
       
		public async Task<IActionResult> Create()
        {
            ViewBag.Departments =await _departmentService.SelectDepartment();
            ViewData["Message"] = "";
            ViewData["Status"] = false;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateComplaintViewModel request)
        {
            var response = await _complaintService.CreateComplaint(request);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);
                return View();
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Complaint");
        }

        public async Task<IActionResult> GetComplaintByDepartment(string id)
        {
            var response = await _complaintService.GetComplaintsByDepartmentId(id);
            ViewData["Message"] = response.Message;
            ViewData["Status"] = response.Status;

            return View(response.Data);
        }

        public async Task<IActionResult> GetComplaintDetail(string id)
        {
            var response = await _complaintService.GetComplaint(id);
            ViewData["Message"] = response.Message;
            ViewData["Status"] = response.Status;

            return View(response.Data);
        }

        public async Task<IActionResult> Update(string id)
        {
            var response = await _complaintService.GetComplaint(id);

            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateComplaintViewModel request)
        {
            var response = await _complaintService.UpdateComplaint(id, request);

            if (response.Status is false)
            {
                _notyf.Error(response.Message);

                return RedirectToAction("Index", "Home");
            }

            _notyf.Success(response.Message);

            return RedirectToAction("Index", "Complaint");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComplaint([FromRoute] string id)
        {
            var response = await _complaintService.DeleteComplaint(id);

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
