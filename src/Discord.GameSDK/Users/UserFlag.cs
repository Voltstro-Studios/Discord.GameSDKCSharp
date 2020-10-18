namespace Discord.GameSDK.Users
{
	/// <summary>
	///     The flags that a user could have
	/// </summary>
	public enum UserFlag
	{
		/// <summary>
		///     Discord Partner
		/// </summary>
		Partner = 2,

		/// <summary>
		///     HypeSquad Events participant
		/// </summary>
		HypeSquadEvents = 4,

		/// <summary>
		///     House Bravery
		/// </summary>
		HypeSquadHouse1 = 64,

		/// <summary>
		///     House Brilliance
		/// </summary>
		HypeSquadHouse2 = 128,

		/// <summary>
		///     House Balance
		/// </summary>
		HypeSquadHouse3 = 256
	}
}