using CoolCatCollects.Bricklink;
using CoolCatCollects.Models;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	[Authorize]
	public class InventoryController : Controller
	{
		private readonly IBricklinkService _service;
		private readonly IBricklinkInventorySanityCheckService _sanityService;

		public InventoryController(IBricklinkService service, IBricklinkInventorySanityCheckService sanityService)
		{
			_service = service;
			_sanityService = sanityService;
		}

		// GET: Inventory
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult GetByHistory(string location)
		{
			var locations = _service.GetHistoriesByLocation(location);

			var model = new LocationHistoryModel();
			model.AddRange(locations);
			model.Location = location;

			return View(model);
		}

		public ActionResult SanityChecker()
		{
			return View();
		}

		#region duplicate inv items

		public ActionResult GetDuplicateInventoryItems()
		{
			var duplicateInventories = _sanityService.GetDuplicateInventoryItems();

			return Json(duplicateInventories, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FixDuplicateInventoryItems()
		{
			var results = _sanityService.FixDuplicateInventoryItems();

			return Json(results, JsonRequestBehavior.AllowGet);
		}

		#endregion
		#region duplicate parts

		public ActionResult GetDuplicateParts()
		{
			var duplicateParts = _sanityService.GetDuplicateParts();

			return Json(duplicateParts, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FixDuplicateParts()
		{
			_sanityService.FixDuplicateParts();

			return Json(true, JsonRequestBehavior.AllowGet);
		}

		#endregion
		#region duplicate orders

		public ActionResult GetDuplicateOrders()
		{
			var duplicateParts = _sanityService.GetDuplicateOrders();

			return Json(duplicateParts, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FixDuplicateOrders()
		{
			_sanityService.FixDuplicateOrders();

			return Json(true, JsonRequestBehavior.AllowGet);
		}

		#endregion
		#region duplicate locations

		public ActionResult GetDuplicateInventoryLocations()
		{
			var duplicateLocations = _sanityService.GetDuplicateInventoryLocations();

			return Json(duplicateLocations, JsonRequestBehavior.AllowGet);
		}

		#endregion
		#region old inventory

		public ActionResult GetOldInventory()
		{
			var oldInventory = _sanityService.GetOldInventory();

			return Json(oldInventory, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FixOldInventory()
		{
			var oldInventory = _sanityService.FixOldInventory();

			return Json(oldInventory, JsonRequestBehavior.AllowGet);
		}

		#endregion
	}
}