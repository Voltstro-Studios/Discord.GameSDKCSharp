using System.Runtime.InteropServices;
using Discord.GameSDK.Activities;

namespace Discord.GameSDK.Relationships
{
	/// <summary>
	///     Presence of a user
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct Presence
	{
		/// <summary>
		///     The user's current online status
		/// </summary>
		public Status Status;

		/// <summary>
		///     The user's current activity
		/// </summary>
		public Activity Activity;
	}
}