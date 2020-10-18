using System.Runtime.InteropServices;

namespace Discord.GameSDK.Images
{
	/// <summary>
	///     Dimensions of an image
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct ImageDimensions
	{
		/// <summary>
		///     The width of the image
		/// </summary>
		public uint Width;

		/// <summary>
		///     The height of the image
		/// </summary>
		public uint Height;
	}
}