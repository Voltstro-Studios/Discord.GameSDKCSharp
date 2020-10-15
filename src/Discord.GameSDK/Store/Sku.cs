using System.Runtime.InteropServices;

namespace Discord.GameSDK.Store
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct Sku
	{
		/// <summary>
		/// The unique ID of the SKU
		/// </summary>
		public long Id;

		/// <summary>
		/// What sort of SKU it is
		/// </summary>
		public SkuType Type;

		/// <summary>
		/// The name of the SKU
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Name;

		/// <summary>
		/// The price of the SKU
		/// </summary>
		public SkuPrice Price;
	}
}