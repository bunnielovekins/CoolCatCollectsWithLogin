using CoolCatCollects.Bricklink;
using CoolCatCollects.Data.Repositories;
using System;
using System.Linq;

namespace CoolCatCollects.DatabaseUpdater
{
	public class DatabaseUpdater
	{
		private readonly IInfoRepository _infoRepository;
		private readonly IColourService _colourService;
		private readonly IBricklinkService _service;

		public DatabaseUpdater(IInfoRepository infoRepository, IColourService colourService, IBricklinkService service)
		{
			_infoRepository = infoRepository;
			_colourService = colourService;
			_service = service;
		}

		public int Run()
		{
			var colours = _colourService.Colours;
			var hasErrors = false;

			foreach(var colour in colours)
			{
				Console.WriteLine("Starting on colour: " + colour.Value.Name);

				var errors = _service.UpdateInventoryForColour(colour.Value.Id);

				if (errors.Any())
				{
					hasErrors = true;
				}
				foreach(var err in errors)
				{
					Console.WriteLine(err);
				}
			}

			var info = _infoRepository.GetInfo();

			info.InventoryLastUpdated = DateTime.Now;

			_infoRepository.Update(info);

			Console.WriteLine("Done!");

			return hasErrors ? 1 : 0;
		}
	}
}
