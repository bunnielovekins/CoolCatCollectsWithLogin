using System;
using System.ComponentModel.DataAnnotations;

namespace CoolCatCollects.Models
{
	public class LogModel
	{
		public LogModel()
		{
			Date = DateTime.Now;
		}

		public int Id { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime Date { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		[DataType(DataType.MultilineText)]
		public string Note { get; set; }
		[DataType(DataType.MultilineText)]
		public string FurtherNote { get; set; }
		[Required]
		public string Category { get; set; }

		public static LogModel MakeLog(string title, string note, string furtherNote = "")
		{
			return new LogModel
			{
				Category = "Other",
				Date = DateTime.Now,
				Title = title,
				Note = note,
				FurtherNote = furtherNote
			};
		}
	}
}
