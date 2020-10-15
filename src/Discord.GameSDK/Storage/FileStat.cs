using System.Runtime.InteropServices;

namespace Discord.GameSDK.Storage
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct FileStat
	{
		/// <summary>
		/// The name of the file
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string Filename;

		/// <summary>
		/// The size of the file
		/// </summary>
		public ulong Size;

		/// <summary>
		/// Timestamp of when the file was last modified
		/// </summary>
		public ulong LastModified;
	}
}