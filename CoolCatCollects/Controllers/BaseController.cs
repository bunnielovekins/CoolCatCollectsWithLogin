using CoolCatCollects.Data;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
    public class BaseController : Controller
    {
        private EfContext _dbContext;

        public EfContext DbContext
        {
            get
            {
                return _dbContext ?? EfContext.Create();
            }
            private set
            {
                _dbContext = value;
            }
        }

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    _dbContext = HttpContext.GetOwinContext().Get<EfContext>();

        //    base.OnActionExecuting(filterContext);
        //}
    }
}