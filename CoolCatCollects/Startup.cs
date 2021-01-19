using Autofac.Integration.Mvc;
using CoolCatCollects.App_Start;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(CoolCatCollects.Startup))]
namespace CoolCatCollects
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);

			var container = DependencyConfig.RegisterServices();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			app.UseAutofacMiddleware(container);
			app.UseAutofacMvc();
		}
	}
}
