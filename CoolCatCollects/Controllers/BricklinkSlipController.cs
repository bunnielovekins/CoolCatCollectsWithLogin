using CoolCatCollects.Bricklink;
using System.Linq;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	[Authorize]
	public class BricklinkSlipController : Controller
	{
		private readonly IBricklinkService _service;

		public BricklinkSlipController(IBricklinkService service)
		{
			_service = service;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List(string status = "PAID")
		{
			var result = _service.GetOrders(status);

			return View(result);
		}

		public ActionResult PartsList(string orderId)
		{
			var order = _service.GetOrderWithItems(orderId);

			order.Messages = _service.GetOrderMessages(orderId);

			return View(order);
		}

		public ActionResult PackingSlip(string orderId)
		{
			var order = _service.GetOrderWithItems(orderId);

			order.Items = order.Items
				.OrderBy(x => x.Colour)
				.ThenBy(x => x.Name);

			order.Items = order.Items.OrderBy(x => x.Colour).ThenBy(x => x.Name);

			return View(order);
		}

		public ActionResult GetOrderInfo(string orderId)
		{
			var order = _service.GetOrderForCsv(orderId);

			return Json(order, JsonRequestBehavior.AllowGet);
		}

		public ActionResult LoadOrder(int id, string orderId)
		{
			var name = _service.LoadOrder(id, orderId);

			return Json(new { name }, JsonRequestBehavior.AllowGet);
		}
	}
}