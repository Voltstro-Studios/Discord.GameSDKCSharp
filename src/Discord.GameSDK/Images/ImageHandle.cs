using System.Runtime.InteropServices;

namespace Discord.GameSDK.Images
{
	/// <summary>
	/// Image handle
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public partial struct ImageHandle
	{
		/// <summary>
		/// The source of the image
		/// </summary>
		public ImageType Type;

		/// <summary>
		/// The id of the user whose avatar you want to get
		/// </summary>
		public long Id;

		/// <summary>
		/// The resolution at which you want the image
		/// </summary>
		public uint Size;
	}
}