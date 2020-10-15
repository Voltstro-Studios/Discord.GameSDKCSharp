using System.Runtime.InteropServices;

namespace Discord.GameSDK.Store
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct Entitlement
	{
		/// <summary>
		/// The unique ID of the entitlement
		/// </summary>
		public long Id;

		/// <summary>
		/// The kind of entitlement it is
		/// </summary>
		public EntitlementType Type;

		/// <summary>
		/// The ID of the SKU to which the user is entitled
		/// </summary>
		public long SkuId;
	}
}