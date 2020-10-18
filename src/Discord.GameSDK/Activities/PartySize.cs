using System.Runtime.InteropServices;

namespace Discord.GameSDK.Activities
{
	/// <summary>
	///     The size of a part
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct PartySize
	{
		/// <summary>
		///     The current size of the party
		/// </summary>
		public int CurrentSize;

		/// <summary>
		///     The max possible size of the party
		/// </summary>
		public int MaxSize;
	}
}