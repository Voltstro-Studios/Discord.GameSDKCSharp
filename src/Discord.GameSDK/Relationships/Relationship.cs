using System.Runtime.InteropServices;
using Discord.GameSDK.Users;

namespace Discord.GameSDK.Relationships
{
	/// <summary>
	///     A relationship with a user
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct Relationship
	{
		/// <summary>
		///     What kind of relationship it is
		/// </summary>
		public RelationshipType Type;

		/// <summary>
		///     The user the relationship is for
		/// </summary>
		public User User;

		/// <summary>
		///     That user's current presence
		/// </summary>
		public Presence Presence;
	}
}