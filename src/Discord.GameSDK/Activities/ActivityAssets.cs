using System.Runtime.InteropServices;

namespace Discord.GameSDK.Activities
{
	/// <summary>
	///     Assets to be used by an activity
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct ActivityAssets
	{
		/// <summary>
		///     Keyname of an asset to display
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string LargeImage;

		/// <summary>
		///     Hover text for the large image
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string LargeText;

		/// <summary>
		///     Keyname of an asset to display
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SmallImage;

		/// <summary>
		///     Hover text for the small image
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SmallText;
	}
}