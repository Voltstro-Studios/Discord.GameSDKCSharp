using System;
using Discord.GameSDK.Activities;
using Discord.GameSDK.Images;

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
					ConsoleKeyInfo keyInfo = Console.ReadKey();
					if (keyInfo.Key == ConsoleKey.Spacebar)
					{
						running = false;
					}

					if (keyInfo.Key == ConsoleKey.Backspace)
					{
						discord.GetActivityManager().ClearActivity(result => Console.WriteLine($"Clear presence: {result}"));
					}

					if (keyInfo.Key == ConsoleKey.Enter)
					{
						discord.GetActivityManager().UpdateActivity(new Activity
						{
							Name = "Test App",
							Details = "Bruh moment"
						}, result => Console.WriteLine($"Update presence: {result}"));
					}

					if (keyInfo.Key == ConsoleKey.I)
					{
						const long userID = 373808840568864768;

						discord.GetImageManager().Fetch(ImageHandle.User(userID), (result, handleResult) =>
						{
							if (result == Result.Ok)
							{
								ImageDimensions dimensions =
									discord.GetImageManager().GetDimensions(ImageHandle.User(userID));

								Console.WriteLine($"Voltstro image is {dimensions.Width} x {dimensions.Height} with a size of {handleResult.Size} and an ID of {handleResult.Id}");
							}
							else
							{
								Console.WriteLine("Some error occurred getting user image!");
							}
						} );
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
