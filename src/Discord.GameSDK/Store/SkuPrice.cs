using System.Runtime.InteropServices;

namespace Discord.GameSDK.Store
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct SkuPrice
	{
		/// <summary>
		///     The amount of money the SKU costs
		/// </summary>
		public uint Amount;

		/// <summary>
		///     The currency the amount is in
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string Currency;
	}
}