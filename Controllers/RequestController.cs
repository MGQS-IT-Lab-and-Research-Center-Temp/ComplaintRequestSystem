using AspNetCoreHero.ToastNotification.Abstractions;
using ComplaintRequestSystem.Models.Complaint;
using ComplaintRequestSystem.Models.Request;
using ComplaintRequestSystem.Service.Implementations;
using ComplaintRequestSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintRequestSystem.Controllers
{
	public class RequestController : Controller
	{
		private readonly IComplaintService _complaintService;
		private readonly IRequestService _requestService;
		private readonly IDepartmentService _departmentService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly INotyfService _notyf;

		public RequestController(
			IComplaintService complaintService,
			IDepartmentService departmentService,
			IHttpContextAccessor httpContextAccessor,
			IRequestService requestService,
			INotyfService notyf)
		{
			_complaintService = complaintService;
			_departmentService = departmentService;
			_requestService = requestService;
			_httpContextAccessor = httpContextAccessor;
			_notyf = notyf;
		}

		public IActionResult Index()
		{
			var requests = _requestService.GetAllRequest();
			ViewData["Message"] = requests.Message;
			ViewData["Status"] = requests.Status;

			return View(requests.Data);
		}




		public IActionResult Create()
		{
			ViewBag.Departments = _departmentService.SelectDepartment();
			ViewData["Message"] = "";
			ViewData["Status"] = false;

			return View();
		}

		[HttpPost]
		public IActionResult Create(CreateRequestViewModel request)
		{
			var response = _requestService.CreateRequest(request);

			if (response.Status is false)
			{
				_notyf.Error(response.Message);
				return View();
			}

			_notyf.Success(response.Message);

			return RedirectToAction("Index", "Request");
		}

		public IActionResult GetRequestsByDepartment(string id)
		{
			var response = _requestService.GetRequestsByDepartmentId(id);
			ViewData["Message"] = response.Message;
			ViewData["Status"] = response.Status;

			return View(response.Data);
		}

		public IActionResult GetRequestDetail(string id)
		{
			var response = _requestService.GetRequest(id);
			ViewData["Message"] = response.Message;
			ViewData["Status"] = response.Status;

			return View(response.Data);
		}

		public IActionResult Update(string id)
		{
			var response = _requestService.GetRequest(id);

			return View(response.Data);
		}

		[HttpPost]
		public IActionResult Update(string id, UpdateRequestViewModel request)
		{
			var response = _requestService.UpdateRequest(id, request);

			if (response.Status is false)
			{
				_notyf.Error(response.Message);

				return RedirectToAction("Index", "Home");
			}

			_notyf.Success(response.Message);

			return RedirectToAction("Index", "Request");
		}

		[HttpPost]
		public IActionResult DeleteRequest([FromRoute] string id)
		{
			var response = _requestService.DeleteRequest(id);

			if (response.Status is false)
			{
				_notyf.Error(response.Message);
				return View();
			}

			_notyf.Success(response.Message);

			return RedirectToAction("Index", "Request");
		}
	}
}

