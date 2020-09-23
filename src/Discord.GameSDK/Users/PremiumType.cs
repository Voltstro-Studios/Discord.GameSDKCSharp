namespace Discord.GameSDK.Users
{
	/// <summary>
	/// What Nitro subscription a user has
	/// </summary>
	public enum PremiumType
	{
		/// <summary>
		/// Not a Nitro subscriber
		/// </summary>
		None = 0,

		/// <summary>
		/// Nitro Classic subscriber
		/// </summary>
		Tier1 = 1,

		/// <summary>
		/// Nitro subscriber
		/// </summary>
		Tier2 = 2,
	}
}