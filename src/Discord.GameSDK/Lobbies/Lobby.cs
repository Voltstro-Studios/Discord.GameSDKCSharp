using System.Runtime.InteropServices;

namespace Discord.GameSDK.Lobbies
{
	/// <summary>
	/// A lobby
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct Lobby
	{
		/// <summary>
		/// The unique id of the lobby
		/// </summary>
		public long Id;

		/// <summary>
		/// If the lobby is public or private
		/// </summary>
		public LobbyType Type;

		/// <summary>
		/// The userId of the lobby owner
		/// </summary>
		public long OwnerId;

		/// <summary>
		/// The password to the lobby
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Secret;

		/// <summary>
		/// The max capacity of the lobby
		/// </summary>
		public uint Capacity;

		/// <summary>
		/// Whether or not the lobby can be joined
		/// </summary>
		public bool Locked;
	}
}