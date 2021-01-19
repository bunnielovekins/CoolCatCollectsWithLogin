namespace CoolCatCollects.Services
{
	public interface IWordExportService
	{
		string ExportRemarks(string[] allRemarks, string set, string path, int page);
	}
}