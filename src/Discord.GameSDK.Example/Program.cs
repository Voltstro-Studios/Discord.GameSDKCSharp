using System;
using Discord.GameSDK.Activities;

namespace Discord.GameSDK.Example
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Starting discord example...");

			try
			{
				Discord discord = new Discord(758184866411315221, CreateFlags.Default);
				discord.SetLogHook(LogLevel.Debug, (level, message) => Console.WriteLine(message));

				bool running = true;
				while (running)
				{
					ConsoleKeyInfo keyInfo = System.Console.ReadKey();
					if (keyInfo.Key == ConsoleKey.Spacebar)
					{
						running = false;
					}

					if (keyInfo.Key == ConsoleKey.Enter)
					{
						discord.GetActivityManager().UpdateActivity(new Activity
						{
							Name = "Test App",
							Details = "Bruh moment"
						}, result => Console.WriteLine($"Update presence: {result}"));
					}

					discord.RunCallbacks();
				}

				Console.WriteLine("Stopping...");
				discord.Dispose();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
