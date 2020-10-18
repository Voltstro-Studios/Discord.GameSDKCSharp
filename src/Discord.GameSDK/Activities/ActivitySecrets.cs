using System.Runtime.InteropServices;

namespace Discord.GameSDK.Activities
{
	/// <summary>
	///     Secrets related to an activity
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct ActivitySecrets
	{
		/// <summary>
		///     Unique hash for the given match context
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Match;

		/// <summary>
		///     Unique hash for chat invites and Ask to Join
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Join;

		/// <summary>
		///     Unique hash for Spectate button
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Spectate;
	}
}