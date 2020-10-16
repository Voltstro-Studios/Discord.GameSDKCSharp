using System.Runtime.InteropServices;

namespace Discord.GameSDK.Images
{
	/// <summary>
	/// Image handle
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct ImageHandle
	{
		/// <summary>
		/// Get's a user's <see cref="ImageHandle"/>
		/// </summary>
		/// <param name="id"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static ImageHandle User(long id, uint size = 128)
		{
			return new ImageHandle
			{
				Type = ImageType.User,
				Id = id,
				Size = size,
			};
		}

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