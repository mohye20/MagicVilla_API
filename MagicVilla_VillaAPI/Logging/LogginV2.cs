namespace MagicVilla_VillaAPI.Logging
{
	public class LogginV2 : ILogging
	{
		public void Log(string Message, string Type)
		{
			if(Type == "error")
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error - {Message}");
				Console.BackgroundColor = ConsoleColor.Black;
			}

			else
			{
				if(Type == "warning")
				{

					Console.BackgroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine($"Error {Message}");
					Console.BackgroundColor = ConsoleColor.Black;
				}

				else
				{
					Console.WriteLine(Message);
				}
			}
		}
	}
}
