namespace Discord.GameSDK.Lobbies
{
	/// <summary>
	///     Lobby search comparison
	/// </summary>
	public enum LobbySearchComparison
	{
		/// <summary>
		///     Less that or equal
		/// </summary>
		LessThanOrEqual = -2,

		/// <summary>
		///     Less than
		/// </summary>
		LessThan,

		/// <summary>
		///     Equal
		/// </summary>
		Equal,

		/// <summary>
		///     Greater than
		/// </summary>
		GreaterThan,

		/// <summary>
		///     Greater than or equal
		/// </summary>
		GreaterThanOrEqual,

		/// <summary>
		///     Not equal
		/// </summary>
		NotEqual
	}
}