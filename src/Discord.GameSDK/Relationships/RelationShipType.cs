namespace Discord.GameSDK.Relationships
{
	/// <summary>
	///     The relationship type
	/// </summary>
	public enum RelationshipType
	{
		/// <summary>
		///     User has no intrinsic relationship
		/// </summary>
		None,

		/// <summary>
		///     User is a friend
		/// </summary>
		Friend,

		/// <summary>
		///     User is blocked
		/// </summary>
		Blocked,

		/// <summary>
		///     User has a pending incoming friend request to connected user
		/// </summary>
		PendingIncoming,

		/// <summary>
		///     Current user has a pending outgoing friend request to user
		/// </summary>
		PendingOutgoing,

		/// <summary>
		///     User is not friends, but interacts with current user often (frequency + recency)
		/// </summary>
		Implicit
	}
}