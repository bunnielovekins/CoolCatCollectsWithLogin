using System.Globalization;
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

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}
