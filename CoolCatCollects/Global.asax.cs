using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CoolCatCollects
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			CultureInfo.CurrentUICulture = new CultureInfo("en-GB", false);
			CultureInfo.CurrentCulture = new CultureInfo("en-GB", false);
			if (HttpContext.Current.Session != null)
			{
				Session.Timeout = 1440;
			}

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}
