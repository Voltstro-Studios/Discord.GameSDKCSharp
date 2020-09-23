namespace Discord.GameSDK.Store
{
	/// <summary>
	/// The entitlement type
	/// </summary>
	public enum EntitlementType
	{
		/// <summary>
		/// Entitlement was purchased
		/// </summary>
		Purchase = 1,

		/// <summary>
		/// Entitlement for a Discord Nitro subscription
		/// </summary>
		PremiumSubscription,

		/// <summary>
		/// Entitlement was gifted by a developer
		/// </summary>
		DeveloperGift,

		/// <summary>
		/// Entitlement was purchased by a dev in application test mode
		/// </summary>
		TestModePurchase,

		/// <summary>
		/// Entitlement was granted when the SKU was free
		/// </summary>
		FreePurchase,

		/// <summary>
		/// Entitlement was gifted by another user
		/// </summary>
		UserGift,

		/// <summary>
		/// Entitlement was claimed by user for free as a Nitro Subscriber
		/// </summary>
		PremiumPurchase,
	}
}