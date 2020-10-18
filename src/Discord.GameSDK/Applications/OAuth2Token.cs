using System.Runtime.InteropServices;

namespace Discord.GameSDK.Applications
{
	/// <summary>
	///     Token for OAuth2
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct OAuth2Token
	{
		/// <summary>
		///     A bearer token for the current user
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string AccessToken;

		/// <summary>
		///     A list of oauth2 scopes as a single string, delineated by spaces like "identify rpc gdm.join"
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
		public string Scopes;

		/// <summary>
		///     The timestamp at which the token expires
		/// </summary>
		public long Expires;
	}
}