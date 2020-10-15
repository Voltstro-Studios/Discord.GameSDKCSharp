using System.Runtime.InteropServices;

namespace Discord.GameSDK.Activities
{
	/// <summary>
	/// Timestamps for an activity
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct ActivityTimestamps
	{
		/// <summary>
		/// Unix timestamp - send this to have an "elapsed" timer
		/// </summary>
		public long Start;

		/// <summary>
		/// Unix timestamp - send this to have a "remaining" timer
		/// </summary>
		public long End;
	}
}