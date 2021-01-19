namespace CoolCatCollects.Ebay
{
	public interface IeBayApiService
	{
		void Authorise();
		string GetRequest(string url);
	}
}