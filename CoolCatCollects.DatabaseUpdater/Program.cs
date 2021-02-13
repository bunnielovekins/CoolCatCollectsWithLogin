using Autofac;
using CoolCatCollects.Bricklink;
using CoolCatCollects.Data;
using CoolCatCollects.Data.Repositories;

namespace CoolCatCollects.DatabaseUpdater
{
	public class Program
	{
		public static IContainer RegisterServices()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<DatabaseUpdater>().AsSelf().InstancePerLifetimeScope();

			// Data
			builder.RegisterType<EfContext>().AsSelf().InstancePerLifetimeScope();
			builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
			builder.RegisterType<InfoRepository>().As<IInfoRepository>();
			builder.RegisterType<OrderRepository>().As<IOrderRepository>();
			builder.RegisterType<PartInventoryRepository>().As<IPartInventoryRepository>();

			// Bricklink
			builder.RegisterType<BricklinkApiService>().As<IBricklinkApiService>();
			builder.RegisterType<BricklinkDataService>().As<IBricklinkDataService>();
			builder.RegisterType<BricklinkInventorySanityCheckService>().As<IBricklinkInventorySanityCheckService>();
			builder.RegisterType<BricklinkService>().As<IBricklinkService>();
			builder.RegisterType<ColourService>().As<IColourService>();

			var container = builder.Build();

			return container;
		}

		public static int Main(string[] args)
		{
			var container = RegisterServices();

			using (var scope = container.BeginLifetimeScope())
			{
				var updater = scope.Resolve<DatabaseUpdater>();
				return updater.Run();
			}
		}
	}
}
