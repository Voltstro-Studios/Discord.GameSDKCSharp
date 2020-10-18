namespace Discord.GameSDK.Store
{
	public enum SkuType
	{
		/// <summary>
		///     SKU is a game
		/// </summary>
		Application = 1,

		/// <summary>
		///     SKU is a DLC
		/// </summary>
		DLC,

		/// <summary>
		///     SKU is a consumable (in-app purchase)
		/// </summary>
		Consumable,

		/// <summary>
		///     SKU is a bundle (comprising the other 3 types)
		/// </summary>
		Bundle
	}
}