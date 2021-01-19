using CoolCatCollects.Bricklink.Models;
using System.Collections.Generic;

namespace CoolCatCollects.Bricklink
{
	public interface IColourService
	{
		Dictionary<int, ColourModel> Colours { get; }

		IEnumerable<ColourModel> GetAll();
	}
}