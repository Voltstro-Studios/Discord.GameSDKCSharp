namespace Discord.GameSDK
{
	/// <summary>
	/// The logging level
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Log only errors
		/// </summary>
		Error = 1,

		/// <summary>
		/// Log warnings and errors
		/// </summary>
		Warn,

		/// <summary>
		/// Log info, warnings, and errors
		/// </summary>
		Info,

		/// <summary>
		/// Log all the things!
		/// </summary>
		Debug
	}
}