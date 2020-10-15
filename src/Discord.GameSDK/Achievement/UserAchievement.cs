using System.Runtime.InteropServices;

namespace Discord.GameSDK.Achievement
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct UserAchievement
	{
		/// <summary>
		/// The unique id of the user working on the achievement
		/// </summary>
		public long UserId;

		/// <summary>
		/// The unique id of the achievement
		/// </summary>
		public long AchievementId;

		/// <summary>
		/// How far along the user is to completing the achievement (0-100)
		/// </summary>
		public byte PercentComplete;

		/// <summary>
		/// The timestamp at which the user completed the achievement (PercentComplete was set to 100)
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string UnlockedAt;
	}
}