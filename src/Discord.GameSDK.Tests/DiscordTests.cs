using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Discord.GameSDK.Tests
{
	[TestClass]
	public class DiscordTests
	{
		private const long ClientId = 758184866411315221;
		private static Discord client;

		[ClassInitialize]
		public static void DiscordSetup(TestContext context)
		{
			client = new Discord(ClientId, (ulong)CreateFlags.Default);
			client.RunCallbacks();
		}

		[TestMethod]
		public void DiscordActivityTest()
		{
			client.RunCallbacks();
			client.GetActivityManager().UpdateActivity(new Activity
			{
				Details = "Test",
				State = "Test"
			}, result =>
			{
				Assert.AreEqual(Result.Ok, result);
			});
			client.RunCallbacks();
		}

		[ClassCleanup]
		public static void DiscordCleanup()
		{
			client.Dispose();
		}
	}
}