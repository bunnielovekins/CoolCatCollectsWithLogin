using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Bricklink.Models.Responses;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink
{
	public class ColourService : IColourService
	{
		private static Dictionary<int, ColourModel> _colours;
		private readonly IBaseRepository<Colour> _repo;

		public ColourService(IBaseRepository<Colour> repo)
		{
			_repo = repo;
		}

		/// <summary>
		/// Gets all Bricklink colours. If they aren't in the DB, get them from the API.
		/// </summary>
		/// <returns>Every colour in a model</returns>
		public IEnumerable<ColourModel> GetAll()
		{
			var colours = _repo.FindAll();

			if (!colours.Any())
			{
				var apiService = new BricklinkApiService(this);
				var result = apiService.GetRequest<GetColoursResponse>("/colors");

				colours = result.data.Select(x => _repo.Add(new Colour
				{
					ColourId = x.color_id,
					ColourCode = x.color_code,
					ColourType = x.color_type,
					Name = x.color_name
				}));
			}

			return colours.Select(x => new ColourModel(x));
		}

		public Dictionary<int, ColourModel> Colours
		{
			get
			{
				if (_colours != null && _colours.Any())
				{
					return _colours;
				}

				var lst = GetAll().ToList();
				lst.Add(new ColourModel
				{
					Id = 0,
					ColourCode = "#FFFFFF",
					ColourType = "",
					Name = ""
				});

				_colours = lst.ToDictionary(x => x.Id);

				return _colours;
			}
		}
	}
}
