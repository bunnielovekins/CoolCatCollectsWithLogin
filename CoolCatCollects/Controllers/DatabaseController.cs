using CoolCatCollects.Bricklink;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	[Authorize]
	public class DatabaseController : Controller
	{
		private readonly IInfoRepository _infoRepository;
		private readonly IColourService _colourService;
		private readonly IBricklinkService _service;

		public DatabaseController(IInfoRepository infoRepository, IColourService colourService, IBricklinkService service)
		{
			_infoRepository = infoRepository;
			_colourService = colourService;
			_service = service;
		}

		//GET: Database
		public ActionResult Index()
		{
			ViewBag.Title = "Database Page";

			return View(new DatabaseUpdateModel(_infoRepository.GetInfo(), _colourService.GetAll()));
		}

		public ActionResult UpdateInventory(int colourId)
		{
			var errors = _service.UpdateInventoryForColour(colourId);

			return Json(new { success = errors.Any(), errors = errors.Any() ? errors.Aggregate((current, next) => current + ", " + next) : "" }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult UpdateInventoryDone()
		{
			var info = _infoRepository.GetInfo();

			info.InventoryLastUpdated = DateTime.Now;

			_infoRepository.Update(info);

			return Content("");
		}

		public ActionResult GetAllOrders()
		{
			var orders = _service.GetOrdersNotInDb();

			return Json(orders, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AddOrder(string orderId)
		{
			try
			{
				_service.GetOrderWithItems(orderId);

				return Json(new { success = true, error = "" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult UpdateOrdersDone()
		{
			var info = _infoRepository.GetInfo();

			info.OrdersLastUpdated = DateTime.Now;

			_infoRepository.Update(info);

			return Content("");
		}
	}
}