using System;
using System.Diagnostics;
using Activity = Discord.GameSDK.Activities.Activity;

namespace Discord.GameSDK.Example
{
	public static class Program
	{
		private const long ClientId = 758184866411315221;
		private static Discord discord;

		public static void Main()
		{
			Console.WriteLine("Starting discord example...");

			try
			{
				discord = new Discord(ClientId, CreateFlags.Default);
				try
				{
					discord.Init();
					Console.WriteLine("Discord init was a success!");
				}
				catch (ResultException e)
				{
					Console.WriteLine(e);
					Debug.Assert(false, e.Message);
					return;
				}

				discord.SetLogHook(LogLevel.Debug, (level, message) => Console.WriteLine(message));

				Console.WriteLine("Press 'T' to update the activity, press 'C' to clear it, or press the space bar to exit.");

				while (true)
				{
					ConsoleKey key = Console.ReadKey().Key;
					if (key == ConsoleKey.Spacebar)
					{
						break;
					}

					if (key == ConsoleKey.T)
					{
						Console.Write("\b");

						discord.GetActivityManager().UpdateActivity(new Activity
						{
							Details = "Hello",
							Name = "Bruh moment"
						}, result => Console.WriteLine($"Update presence: {result}"));
					}
					else if (key == ConsoleKey.C)
					{
						Console.Write("\b");

						discord.GetActivityManager().ClearActivity(result => Console.WriteLine($"Clear presence: {result}"));
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