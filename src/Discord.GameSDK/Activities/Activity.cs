using System.Runtime.InteropServices;

namespace Discord.GameSDK.Activities
{
	/// <summary>
	/// Discord Activity to show on the user's profile
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct Activity
	{
		/// <summary>
		/// [No Docs]
		/// </summary>
		public ActivityType Type;

		/// <summary>
		/// Your application id - this is a read-only field
		/// </summary>
		public long ApplicationId;

		/// <summary>
		/// Name of the application - this is a read-only field
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Name;

		/// <summary>
		/// The player's current party status
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string State;

		/// <summary>
		/// What the player is currently doing
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Details;

		/// <summary>
		/// Helps create elapsed/remaining timestamps on a player's profile
		/// </summary>
		public ActivityTimestamps Timestamps;

		/// <summary>
		/// Assets to display on the player's profile
		/// </summary>
		public ActivityAssets Assets;

		/// <summary>
		/// Information about the player's party
		/// </summary>
		public ActivityParty Party;

		/// <summary>
		/// Secret passwords for joining and spectating the player's game
		/// </summary>
		public ActivitySecrets Secrets;

		/// <summary>
		/// Whether this activity is an instanced context, like a match
		/// </summary>
		public bool Instance;
	}
}