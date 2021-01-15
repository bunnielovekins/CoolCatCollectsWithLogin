using CoolCatCollects.Bricklink;
using CoolCatCollects.Models;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace CoolCatCollects.Controllers
{
	public class InventoryController : BaseController
	{
		// GET: Inventory
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult GetByHistory(string location)
		{
			var service = new BricklinkService(DbContext);

			var locations = service.GetHistoriesByLocation(location);

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
			var service = new BricklinkInventorySanityCheckService(DbContext);

			var duplicateInventories = service.GetDuplicateInventoryItems();

			return Json(duplicateInventories, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FixDuplicateInventoryItems()
		{
			var service = new BricklinkInventorySanityCheckService(DbContext);

			var results = service.FixDuplicateInventoryItems();

			return Json(results, JsonRequestBehavior.AllowGet);
		}

		#endregion
		#region duplicate parts

		public ActionResult GetDuplicateParts()
		{
			var service = new BricklinkInventorySanityCheckService(DbContext);

			var duplicateParts = service.GetDuplicateParts();

			return Json(duplicateParts, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FixDuplicateParts()
		{
			var service = new BricklinkInventorySanityCheckService(DbContext);

			service.FixDuplicateParts();

			return Json(true, JsonRequestBehavior.AllowGet);
		}

		#endregion
		#region duplicate orders

		public ActionResult GetDuplicateOrders()
		{
			var service = new BricklinkInventorySanityCheckService(DbContext);

			var duplicateParts = service.GetDuplicateOrders();

			return Json(duplicateParts, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FixDuplicateOrders()
		{
			var service = new BricklinkInventorySanityCheckService(DbContext);

			service.FixDuplicateOrders();

			return Json(true, JsonRequestBehavior.AllowGet);
		}

		#endregion
		#region duplicate locations

		public ActionResult GetDuplicateInventoryLocations()
		{
			var service = new BricklinkInventorySanityCheckService(DbContext);

			var duplicateLocations = service.GetDuplicateInventoryLocations();

			return Json(duplicateLocations, JsonRequestBehavior.AllowGet);
		}

		#endregion
		#region old inventory

		public ActionResult GetOldInventory()
		{
			var service = new BricklinkInventorySanityCheckService(DbContext);

			var oldInventory = service.GetOldInventory();

			return Json(oldInventory, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FixOldInventory()
		{
			var service = new BricklinkInventorySanityCheckService(DbContext);

			var oldInventory = service.FixOldInventory();

			return Json(oldInventory, JsonRequestBehavior.AllowGet);
		}

		#endregion
	}
}