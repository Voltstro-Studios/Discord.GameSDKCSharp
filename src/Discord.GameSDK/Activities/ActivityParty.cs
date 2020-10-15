using System.Runtime.InteropServices;

namespace Discord.GameSDK.Activities
{
	/// <summary>
	/// Activity party
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct ActivityParty
	{
		/// <summary>
		/// A unique identifier for this party
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Id;

		/// <summary>
		/// Info about the size of the party
		/// </summary>
		public PartySize Size;
	}
}