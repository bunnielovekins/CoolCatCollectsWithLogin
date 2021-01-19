using Autofac;
using Autofac.Integration.Mvc;
using CoolCatCollects.Bricklink;
using CoolCatCollects.Data;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Ebay;
using CoolCatCollects.Login;
using CoolCatCollects.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web;

namespace CoolCatCollects.App_Start
{
	public static class DependencyConfig
	{
		public static IContainer RegisterServices()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(typeof(MvcApplication).Assembly);
			builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
			builder.RegisterModelBinderProvider();
			builder.RegisterModule<AutofacWebTypesModule>();
			//builder.RegisterSource(new ViewRegistrationSource());
			builder.RegisterFilterProvider();

			// Data
			builder.RegisterType<EfContext>().AsSelf().InstancePerRequest();
			builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
			builder.RegisterType<InfoRepository>().As<IInfoRepository>();
			builder.RegisterType<OrderRepository>().As<IOrderRepository>();
			builder.RegisterType<PartInventoryRepository>().As<IPartInventoryRepository>();

			// Owin
			builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
			builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
			builder.Register(c => new UserStore<ApplicationUser>(c.Resolve<EfContext>())).AsImplementedInterfaces().InstancePerRequest();
			builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();
			builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>
			{
				DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("CoolCatCollects​")
			});

			// Services
			builder.RegisterType<ExpensesService>().As<IExpensesService>();
			builder.RegisterType<LogService>().As<ILogService>();
			builder.RegisterType<NewPurchaseService>().As<INewPurchaseService>();
			builder.RegisterType<UsedPurchaseService>().As<IUsedPurchaseService>();
			builder.RegisterType<WordExportService>().As<IWordExportService>();

			// eBay
			builder.RegisterType<eBayApiService>().As<IeBayApiService>();
			builder.RegisterType<eBayDataService>().As<IeBayDataService>();
			builder.RegisterType<eBayService>().As<IeBayService>();

			// Bricklink
			builder.RegisterType<BricklinkApiService>().As<IBricklinkApiService>();
			builder.RegisterType<BricklinkDataService>().As<IBricklinkDataService>();
			builder.RegisterType<BricklinkInventorySanityCheckService>().As<IBricklinkInventorySanityCheckService>();
			builder.RegisterType<BricklinkService>().As<IBricklinkService>();
			builder.RegisterType<ColourService>().As<IColourService>();

			var container = builder.Build();

			return container;
		}
	}
}