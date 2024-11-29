namespace MagicVilla_VillaAPI.Logging
{
	public class Logging : ILogging
	{
		public void Log(string Message, string Type)
		{
			if (Type == "error")
			{
				Console.WriteLine($"Error - {Message}");
			}
			else
			{
				Console.WriteLine(Message);
			}
		}
	}
}