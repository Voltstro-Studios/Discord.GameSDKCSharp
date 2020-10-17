namespace Discord.GameSDK
{
	// ReSharper disable UnusedMember.Global

	/// <summary>
	/// The result of a request
	/// </summary>
	public enum Result
	{
		/// <summary>
		/// Everything is good
		/// </summary>
		Ok = 0,

		/// <summary>
		/// Discord isn't working
		/// </summary>
		ServiceUnavailable = 1,

		/// <summary>
		/// The SDK version may be outdated
		/// </summary>
		InvalidVersion = 2,

		/// <summary>
		/// An internal error on transactional operations
		/// </summary>
		LockFailed = 3,

		/// <summary>
		/// Something on our side went wrong
		/// </summary>
		InternalError = 4,

		/// <summary>
		/// The data you sent didn't match what we expect
		/// </summary>
		InvalidPayload = 5,

		/// <summary>
		/// That's not a thing you can do
		/// </summary>
		InvalidCommand = 6,

		/// <summary>
		/// You aren't authorized to do that
		/// </summary>
		InvalidPermissions = 7,

		/// <summary>
		/// Couldn't fetch what you wanted
		/// </summary>
		NotFetched = 8,

		/// <summary>
		/// That you're looking for doesn't exist
		/// </summary>
		NotFound = 9,

		/// <summary>
		/// User already has a network connection open on that channel
		/// </summary>
		Conflict = 10,

		/// <summary>
		/// Activity secrets must be unique and not match party id
		/// </summary>
		InvalidSecret = 11,

		/// <summary>
		/// Join request for that user does not exist
		/// </summary>
		InvalidJoinSecret = 12,

		/// <summary>
		/// You accidentally set an <see cref="Activities.Activity.ApplicationId"/> in your <see cref="Activities.ActivityManager.UpdateActivity"/> payload
		/// </summary>
		NoEligibleActivity = 13,

		/// <summary>
		/// Your game invite is no longer valid
		/// </summary>
		InvalidInvite = 14,

		/// <summary>
		/// The internal auth call failed for the user, and you can't do this
		/// </summary>
		NotAuthenticated = 15,

		/// <summary>
		/// The user's bearer token is invalid
		/// </summary>
		InvalidAccessToken = 16,

		/// <summary>
		/// Access token belongs to another application
		/// </summary>
		ApplicationMismatch = 17,

		/// <summary>
		/// Something internally went wrong fetching image data
		/// </summary>
		InvalidDataUrl = 18,

		/// <summary>
		/// Not valid Base64 data
		/// </summary>
		InvalidBase64 = 19,

		/// <summary>
		/// You're trying to access the list before creating a stable list with Filter()
		/// </summary>
		NotFiltered = 20,

		/// <summary>
		/// The lobby is full
		/// </summary>
		LobbyFull = 21,

		/// <summary>
		/// The secret you're using to connect is wrong
		/// </summary>
		InvalidLobbySecret = 22,

		/// <summary>
		/// File name is too long
		/// </summary>
		InvalidFilename = 23,

		/// <summary>
		/// File is too large
		/// </summary>
		InvalidFileSize = 24,

		/// <summary>
		/// The user does not have the right entitlement for this game
		/// </summary>
		InvalidEntitlement = 25,

		/// <summary>
		/// Discord is not installed
		/// </summary>
		NotInstalled = 26,

		/// <summary>
		/// Discord is not running
		/// </summary>
		NotRunning = 27,

		/// <summary>
		/// Insufficient buffer space when trying to write
		/// </summary>
		InsufficientBuffer = 28,

		/// <summary>
		/// User cancelled the purchase flow
		/// </summary>
		PurchaseCanceled = 29,

		/// <summary>
		/// Discord guild does not exist
		/// </summary>
		InvalidGuild = 30,

		/// <summary>
		/// The event you're trying to subscribe to does not exist
		/// </summary>
		InvalidEvent = 31,

		/// <summary>
		/// Discord channel does not exist
		/// </summary>
		InvalidChannel = 32,

		/// <summary>
		/// The origin header on the socket does not match what you've registered (you should not see this)
		/// </summary>
		InvalidOrigin = 33,

		/// <summary>
		/// You are calling that method too quickly
		/// </summary>
		RateLimited = 34,

		/// <summary>
		/// The OAuth2 process failed at some point
		/// </summary>
		OAuth2Error = 35,

		/// <summary>
		/// The user took too long selecting a channel for an invite
		/// </summary>
		SelectChannelTimeout = 36,

		/// <summary>
		/// Took too long trying to fetch the guild
		/// </summary>
		GetGuildTimeout = 37,

		/// <summary>
		/// Push to talk is required for this channel
		/// </summary>
		SelectVoiceForceRequired = 38,

		/// <summary>
		/// That push to talk shortcut is already registered
		/// </summary>
		CaptureShortcutAlreadyListening = 39,

		/// <summary>
		/// Your application cannot update this achievement
		/// </summary>
		UnauthorizedForAchievement = 40,

		/// <summary>
		/// The gift code is not valid
		/// </summary>
		InvalidGiftCode = 41,

		/// <summary>
		/// Something went wrong during the purchase flow
		/// </summary>
		PurchaseError = 42,

		/// <summary>
		/// Purchase flow aborted because the SDK is being torn down
		/// </summary>
		TransactionAborted = 43,
	}

	// ReSharper restore UnusedMember.Global
}