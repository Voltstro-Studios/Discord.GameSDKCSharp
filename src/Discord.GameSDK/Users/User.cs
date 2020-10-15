using System.Runtime.InteropServices;

namespace Discord.GameSDK.Users
{
	/// <summary>
	/// A Discord user
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct User
	{
		/// <summary>
		/// The user's id
		/// </summary>
		public long Id;

		/// <summary>
		/// Their name
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Username;

		/// <summary>
		/// The user's unique discrim
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
		public string Discriminator;

		/// <summary>
		/// The hash of the user's avatar
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Avatar;

		/// <summary>
		/// If the user is a bot user
		/// </summary>
		public bool Bot;
	}
}