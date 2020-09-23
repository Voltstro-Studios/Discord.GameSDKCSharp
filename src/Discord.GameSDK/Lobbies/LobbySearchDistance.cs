namespace Discord.GameSDK.Lobbies
{
	/// <summary>
	/// Search distance for a lobby
	/// </summary>
	public enum LobbySearchDistance
	{
		/// <summary>
		/// Within the same region
		/// </summary>
		Local,

		/// <summary>
		/// Within the same and adjacent regions
		/// </summary>
		Default,

		/// <summary>
		/// Far distances, like US to EU
		/// </summary>
		Extended,

		/// <summary>
		/// All regions
		/// </summary>
		Global,
	}
}