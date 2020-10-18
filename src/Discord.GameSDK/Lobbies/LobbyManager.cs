using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Discord.GameSDK.Users;

namespace Discord.GameSDK.Lobbies
{
	/// <summary>
	///     Looking to integrate multiplayer into your game? Lobbies are a great way to organize players into contexts to play
	///     together.
	///     This manager works hand in hand with the networking layer of our SDK to make multiplayer integrations a breeze by:
	///     <list type="bullet">
	///         <item>
	///             <description>Creating, managing, and joining lobbies</description>
	///         </item>
	///         <item>
	///             <description>Matchmaking users based on lobby metadata, like ELO</description>
	///         </item>
	///         <item>
	///             <description>Getting and setting arbitrary metadata on lobbies and lobby members</description>
	///         </item>
	///     </list>
	///     <para>
	///         Lobbies in Discord work in one of two ways.
	///         By using calls from the SDK, lobbies are effectively "owned" by the user who's client creates the lobby.
	///         Someone boots up the game, hits your "Create Lobby" button, and their game client calls
	///         <see cref="CreateLobby" /> from the Discord SDK.
	///     </para>
	/// </summary>
	public sealed class LobbyManager
	{
		public delegate void ConnectLobbyHandler(Result result, ref Lobby lobby);

		public delegate void ConnectLobbyWithActivitySecretHandler(Result result, ref Lobby lobby);

		public delegate void ConnectVoiceHandler(Result result);

		public delegate void CreateLobbyHandler(Result result, ref Lobby lobby);

		public delegate void DeleteLobbyHandler(Result result);

		public delegate void DisconnectLobbyHandler(Result result);

		public delegate void DisconnectVoiceHandler(Result result);

		public delegate void LobbyDeleteHandler(long lobbyId, uint reason);

		public delegate void LobbyMessageHandler(long lobbyId, long userId, byte[] data);

		public delegate void LobbyUpdateHandler(long lobbyId);

		public delegate void MemberConnectHandler(long lobbyId, long userId);

		public delegate void MemberDisconnectHandler(long lobbyId, long userId);

		public delegate void MemberUpdateHandler(long lobbyId, long userId);

		public delegate void NetworkMessageHandler(long lobbyId, long userId, byte channelId, byte[] data);

		public delegate void SearchHandler(Result result);

		public delegate void SendLobbyMessageHandler(Result result);

		public delegate void SpeakingHandler(long lobbyId, long userId, bool speaking);

		public delegate void UpdateLobbyHandler(Result result);

		public delegate void UpdateMemberHandler(Result result);

		private readonly IntPtr methodsPtr;
		private object methodsStructure;

		internal LobbyManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
		{
			if (eventsPtr == IntPtr.Zero)
				throw new ResultException(Result.InternalError);

			InitEvents(eventsPtr, ref events);
			methodsPtr = ptr;

			if (methodsPtr == IntPtr.Zero)
				throw new ResultException(Result.InternalError);
		}

		private FFIMethods Methods
		{
			get
			{
				if (methodsStructure == null) methodsStructure = Marshal.PtrToStructure(methodsPtr, typeof(FFIMethods));
				return (FFIMethods) methodsStructure;
			}
		}

		/// <summary>
		///     Fires when a lobby is updated.
		/// </summary>
		public event LobbyUpdateHandler OnLobbyUpdate;

		/// <summary>
		///     Fired when a lobby is deleted.
		/// </summary>
		public event LobbyDeleteHandler OnLobbyDelete;

		/// <summary>
		///     Fires when a new member joins the lobby.
		/// </summary>
		public event MemberConnectHandler OnMemberConnect;

		/// <summary>
		///     Fires when data for a lobby member is updated.
		/// </summary>
		public event MemberUpdateHandler OnMemberUpdate;

		/// <summary>
		///     Fires when a member leaves the lobby.
		/// </summary>
		public event MemberDisconnectHandler OnMemberDisconnect;

		/// <summary>
		///     Fires when a message is sent to the lobby.
		/// </summary>
		public event LobbyMessageHandler OnLobbyMessage;

		/// <summary>
		///     Fires when a user connected to voice starts or stops speaking.
		/// </summary>
		public event SpeakingHandler OnSpeaking;

		/// <summary>
		///     Fires when the user receives a message from the lobby's networking layer.
		/// </summary>
		public event NetworkMessageHandler OnNetworkMessage;

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			events.OnLobbyUpdate = OnLobbyUpdateImpl;
			events.OnLobbyDelete = OnLobbyDeleteImpl;
			events.OnMemberConnect = OnMemberConnectImpl;
			events.OnMemberUpdate = OnMemberUpdateImpl;
			events.OnMemberDisconnect = OnMemberDisconnectImpl;
			events.OnLobbyMessage = OnLobbyMessageImpl;
			events.OnSpeaking = OnSpeakingImpl;
			events.OnNetworkMessage = OnNetworkMessageImpl;
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		/// <summary>
		///     Gets a Lobby transaction used for creating a new lobby
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public LobbyTransaction GetLobbyCreateTransaction()
		{
			LobbyTransaction ret = new LobbyTransaction();
			Result res = Methods.GetLobbyCreateTransaction(methodsPtr, ref ret.MethodsPtr);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Gets a lobby transaction used for updating an existing lobby.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public LobbyTransaction GetLobbyUpdateTransaction(long lobbyId)
		{
			LobbyTransaction ret = new LobbyTransaction();
			Result res = Methods.GetLobbyUpdateTransaction(methodsPtr, lobbyId, ref ret.MethodsPtr);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Gets a new member transaction for a lobby member in a given lobby.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public LobbyMemberTransaction GetMemberUpdateTransaction(long lobbyId, long userId)
		{
			LobbyMemberTransaction ret = new LobbyMemberTransaction();
			Result res = Methods.GetMemberUpdateTransaction(methodsPtr, lobbyId, userId, ref ret.MethodsPtr);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Creates a lobby. Creating a lobby auto-joins the connected user to it. Do not call SetOwner() in the transaction
		///     for this method.
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="callback"></param>
		public void CreateLobby(LobbyTransaction transaction, CreateLobbyHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.CreateLobby(methodsPtr, transaction.MethodsPtr, GCHandle.ToIntPtr(wrapped),
				CreateLobbyCallbackImpl);
			transaction.MethodsPtr = IntPtr.Zero;
		}

		/// <summary>
		///     Updates a lobby with data from the given transaction. You can call SetOwner() in this transaction.
		///     <para>
		///         This call has a rate limit of 10 updates per 5 seconds. If you fear you might hit that, it may be a good idea
		///         to batch your lobby updates into transactions.
		///     </para>
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="transaction"></param>
		/// <param name="callback"></param>
		public void UpdateLobby(long lobbyId, LobbyTransaction transaction, UpdateLobbyHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.UpdateLobby(methodsPtr, lobbyId, transaction.MethodsPtr, GCHandle.ToIntPtr(wrapped),
				UpdateLobbyCallbackImpl);
			transaction.MethodsPtr = IntPtr.Zero;
		}

		/// <summary>
		///     Deletes a given lobby.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="callback"></param>
		public void DeleteLobby(long lobbyId, DeleteLobbyHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.DeleteLobby(methodsPtr, lobbyId, GCHandle.ToIntPtr(wrapped), DeleteLobbyCallbackImpl);
		}

		/// <summary>
		///     Connects the current user to a given lobby. You can be connected to up to five lobbies at a time.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="secret"></param>
		/// <param name="callback"></param>
		public void ConnectLobby(long lobbyId, string secret, ConnectLobbyHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.ConnectLobby(methodsPtr, lobbyId, secret, GCHandle.ToIntPtr(wrapped), ConnectLobbyCallbackImpl);
		}

		/// <summary>
		///     Connects the current user to a lobby; requires the special activity secret from the lobby which is a concatenated
		///     lobbyId and secret.
		/// </summary>
		/// <param name="activitySecret"></param>
		/// <param name="callback"></param>
		public void ConnectLobbyWithActivitySecret(string activitySecret,
			ConnectLobbyWithActivitySecretHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.ConnectLobbyWithActivitySecret(methodsPtr, activitySecret, GCHandle.ToIntPtr(wrapped),
				ConnectLobbyWithActivitySecretCallbackImpl);
		}

		/// <summary>
		///     Disconnects the current user from a lobby.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="callback"></param>
		public void DisconnectLobby(long lobbyId, DisconnectLobbyHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.DisconnectLobby(methodsPtr, lobbyId, GCHandle.ToIntPtr(wrapped), DisconnectLobbyCallbackImpl);
		}

		/// <summary>
		///     Gets the lobby object for a given lobby id.
		///     Because of the way that the SDK is architected, you must first call <see cref="Search" /> to build a stable list of
		///     lobbies.
		///     This function will then query those lobbies for ones with a matching id.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public Lobby GetLobby(long lobbyId)
		{
			Lobby ret = new Lobby();
			Result res = Methods.GetLobby(methodsPtr, lobbyId, ref ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Gets the special activity secret for a given lobby. If you are creating lobbies from game clients, use this to
		///     easily interact with Rich Presence invites.
		///     Set the returned secret to your activity's <c>JoinSecret</c>.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public string GetLobbyActivitySecret(long lobbyId)
		{
			StringBuilder ret = new StringBuilder(128);
			Result res = Methods.GetLobbyActivitySecret(methodsPtr, lobbyId, ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret.ToString();
		}

		/// <summary>
		///     Returns lobby metadata value for a given key and id. Can be used with iteration, or direct access by keyname.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public string GetLobbyMetadataValue(long lobbyId, string key)
		{
			StringBuilder ret = new StringBuilder(4096);
			Result res = Methods.GetLobbyMetadataValue(methodsPtr, lobbyId, key, ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret.ToString();
		}

		/// <summary>
		///     Returns the key for the lobby metadata at the given index.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public string GetLobbyMetadataKey(long lobbyId, int index)
		{
			StringBuilder ret = new StringBuilder(256);
			Result res = Methods.GetLobbyMetadataKey(methodsPtr, lobbyId, index, ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret.ToString();
		}

		/// <summary>
		///     Returns the number of metadata key/value pairs on a given lobby. Used for accessing metadata by iterating over the
		///     list.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public int LobbyMetadataCount(long lobbyId)
		{
			int ret = new int();
			Result res = Methods.LobbyMetadataCount(methodsPtr, lobbyId, ref ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Get the number of members in a lobby.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public int MemberCount(long lobbyId)
		{
			int ret = new int();
			Result res = Methods.MemberCount(methodsPtr, lobbyId, ref ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Gets the user id of the lobby member at the given index.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public long GetMemberUserId(long lobbyId, int index)
		{
			long ret = new long();
			Result res = Methods.GetMemberUserId(methodsPtr, lobbyId, index, ref ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Gets the user object for a given user id.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public User GetMemberUser(long lobbyId, long userId)
		{
			User ret = new User();
			Result res = Methods.GetMemberUser(methodsPtr, lobbyId, userId, ref ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Gets all members in a lobby
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <returns></returns>
		public IEnumerable<User> GetMemberUsers(long lobbyId)
		{
			int memberCount = MemberCount(lobbyId);
			List<User> members = new List<User>();
			for (int i = 0; i < memberCount; i++) members.Add(GetMemberUser(lobbyId, GetMemberUserId(lobbyId, i)));
			return members;
		}

		/// <summary>
		///     Returns user metadata for a given key. Can be used in conjunction with the count and get key functions if you're
		///     iterating over metadata.
		///     Or you can access the metadata directly by keyname.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="userId"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public string GetMemberMetadataValue(long lobbyId, long userId, string key)
		{
			StringBuilder ret = new StringBuilder(4096);
			Result res = Methods.GetMemberMetadataValue(methodsPtr, lobbyId, userId, key, ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret.ToString();
		}

		/// <summary>
		///     Gets the key for the lobby metadata at the given index on a lobby member.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="userId"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public string GetMemberMetadataKey(long lobbyId, long userId, int index)
		{
			StringBuilder ret = new StringBuilder(256);
			Result res = Methods.GetMemberMetadataKey(methodsPtr, lobbyId, userId, index, ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret.ToString();
		}

		/// <summary>
		///     Gets the number of metadata key/value pairs for the given lobby member. Used for accessing metadata by iterating
		///     over a list.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public int MemberMetadataCount(long lobbyId, long userId)
		{
			int ret = new int();
			Result res = Methods.MemberMetadataCount(methodsPtr, lobbyId, userId, ref ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Updates lobby member info for a given member of the lobby.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="userId"></param>
		/// <param name="transaction"></param>
		/// <param name="callback"></param>
		public void UpdateMember(long lobbyId, long userId, LobbyMemberTransaction transaction,
			UpdateMemberHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.UpdateMember(methodsPtr, lobbyId, userId, transaction.MethodsPtr, GCHandle.ToIntPtr(wrapped),
				UpdateMemberCallbackImpl);
			transaction.MethodsPtr = IntPtr.Zero;
		}

		/// <summary>
		///     Sends a message to the lobby on behalf of the current user. You must be connected to the lobby you are messaging.
		///     You should use this function for message sending if you are not using the built in networking layer for the lobby.
		///     If you are, you should use <see cref="SendNetworkMessage" /> instead.
		///     <para>This method has a rate limit of 10 messages per 5 seconds.</para>
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="data"></param>
		/// <param name="handler"></param>
		public void SendLobbyMessage(long lobbyId, string data, SendLobbyMessageHandler handler)
		{
			SendLobbyMessage(lobbyId, Encoding.UTF8.GetBytes(data), handler);
		}

		/// <summary>
		///     Sends a message to the lobby on behalf of the current user. You must be connected to the lobby you are messaging.
		///     You should use this function for message sending if you are not using the built in networking layer for the lobby.
		///     If you are, you should use <see cref="SendNetworkMessage" /> instead.
		///     <para>This method has a rate limit of 10 messages per 5 seconds.</para>
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="data"></param>
		/// <param name="callback"></param>
		public void SendLobbyMessage(long lobbyId, byte[] data, SendLobbyMessageHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.SendLobbyMessage(methodsPtr, lobbyId, data, data.Length, GCHandle.ToIntPtr(wrapped),
				SendLobbyMessageCallbackImpl);
		}

		/// <summary>
		///     Creates a search object to search available lobbies.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public LobbySearchQuery GetSearchQuery()
		{
			LobbySearchQuery ret = new LobbySearchQuery();
			Result res = Methods.GetSearchQuery(methodsPtr, ref ret.MethodsPtr);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Searches available lobbies based on the search criteria chosen in the <see cref="LobbySearchQuery" /> member
		///     functions.
		///     Lobbies that meet the criteria are then globally filtered, and can be accessed via iteration with
		///     <see cref="LobbyCount" /> and <see cref="GetLobbyId" />.
		///     The callback fires when the list of lobbies is stable and ready for iteration.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="callback"></param>
		public void Search(LobbySearchQuery query, SearchHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.Search(methodsPtr, query.MethodsPtr, GCHandle.ToIntPtr(wrapped), SearchCallbackImpl);
			query.MethodsPtr = IntPtr.Zero;
		}

		/// <summary>
		///     Get the number of lobbies that match the search.
		/// </summary>
		/// <returns></returns>
		public int LobbyCount()
		{
			int ret = new int();
			Methods.LobbyCount(methodsPtr, ref ret);
			return ret;
		}

		/// <summary>
		///     Returns the id for the lobby at the given index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public long GetLobbyId(int index)
		{
			long ret = new long();
			Result res = Methods.GetLobbyId(methodsPtr, index, ref ret);
			if (res != Result.Ok)
				throw new ResultException(res);

			return ret;
		}

		/// <summary>
		///     Connects to the voice channel of the current lobby. When connected to voice,
		///     the user can open their Discord overlay to see a list of other users with whom they are in voice,
		///     allowing them to mute/deafen themselves as well as mute/adjust the volume of other lobby members.
		///     <para>
		///         You can also allow users to adjust voice settings for your game with
		///         <a href="https://discord.com/developers/docs/game-sdk/overlay#openvoicesettings">Overlay OpenVoiceSettings</a>.
		///     </para>
		///     <para>
		///         When integrating lobby voice into your game, be thoughtful about the user's experience. Auto-joining to voice
		///         can be jarring for users who may not be expecting it.
		///         We recommend voice always being opt-in, or at least that you provide an option for a player to choose whether
		///         or not to auto-join the voice channel of lobbies they join.
		///     </para>
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="callback"></param>
		public void ConnectVoice(long lobbyId, ConnectVoiceHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.ConnectVoice(methodsPtr, lobbyId, GCHandle.ToIntPtr(wrapped), ConnectVoiceCallbackImpl);
		}

		/// <summary>
		///     Disconnects from the voice channel of a given lobby.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="callback"></param>
		public void DisconnectVoice(long lobbyId, DisconnectVoiceHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.DisconnectVoice(methodsPtr, lobbyId, GCHandle.ToIntPtr(wrapped), DisconnectVoiceCallbackImpl);
		}

		/// <summary>
		///     Connects to the networking layer for the given lobby ID. Call this when connecting to the lobby.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <exception cref="ResultException"></exception>
		public void ConnectNetwork(long lobbyId)
		{
			Result res = Methods.ConnectNetwork(methodsPtr, lobbyId);

			if (res != Result.Ok)
				throw new ResultException(res);
		}

		/// <summary>
		///     Disconnects from the networking layer for the given lobby ID.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <exception cref="ResultException"></exception>
		public void DisconnectNetwork(long lobbyId)
		{
			Result res = Methods.DisconnectNetwork(methodsPtr, lobbyId);

			if (res != Result.Ok)
				throw new ResultException(res);
		}

		/// <summary>
		///     Flushes the network. Call this when you're done sending messages. In Unity, this should be in <c>LateUpdate()</c>.
		/// </summary>
		/// <exception cref="ResultException"></exception>
		public void FlushNetwork()
		{
			Result res = Methods.FlushNetwork(methodsPtr);

			if (res != Result.Ok)
				throw new ResultException(res);
		}

		/// <summary>
		///     Opens a network channel to all users in a lobby on the given channel number. No need to iterate over everyone!
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="channelId"></param>
		/// <param name="reliable"></param>
		/// <exception cref="ResultException"></exception>
		public void OpenNetworkChannel(long lobbyId, byte channelId, bool reliable)
		{
			Result res = Methods.OpenNetworkChannel(methodsPtr, lobbyId, channelId, reliable);

			if (res != Result.Ok)
				throw new ResultException(res);
		}

		/// <summary>
		///     Sends a network message to the given user ID that is a member of the given lobby ID over the given channel ID.
		/// </summary>
		/// <param name="lobbyId"></param>
		/// <param name="userId"></param>
		/// <param name="channelId"></param>
		/// <param name="data"></param>
		/// <exception cref="ResultException"></exception>
		public void SendNetworkMessage(long lobbyId, long userId, byte channelId, byte[] data)
		{
			Result res = Methods.SendNetworkMessage(methodsPtr, lobbyId, userId, channelId, data, data.Length);

			if (res != Result.Ok)
				throw new ResultException(res);
		}

		[MonoPInvokeCallback]
		private static void CreateLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			CreateLobbyHandler callback = (CreateLobbyHandler) h.Target;
			h.Free();
			callback(result, ref lobby);
		}

		[MonoPInvokeCallback]
		private static void UpdateLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			UpdateLobbyHandler callback = (UpdateLobbyHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void DeleteLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			DeleteLobbyHandler callback = (DeleteLobbyHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void ConnectLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			ConnectLobbyHandler callback = (ConnectLobbyHandler) h.Target;
			h.Free();
			callback(result, ref lobby);
		}

		[MonoPInvokeCallback]
		private static void ConnectLobbyWithActivitySecretCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			ConnectLobbyWithActivitySecretHandler callback = (ConnectLobbyWithActivitySecretHandler) h.Target;
			h.Free();
			callback(result, ref lobby);
		}

		[MonoPInvokeCallback]
		private static void DisconnectLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			DisconnectLobbyHandler callback = (DisconnectLobbyHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void UpdateMemberCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			UpdateMemberHandler callback = (UpdateMemberHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void SendLobbyMessageCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			SendLobbyMessageHandler callback = (SendLobbyMessageHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void ConnectVoiceCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			ConnectVoiceHandler callback = (ConnectVoiceHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void SearchCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			SearchHandler callback = (SearchHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void DisconnectVoiceCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			DisconnectVoiceHandler callback = (DisconnectVoiceHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void OnLobbyUpdateImpl(IntPtr ptr, long lobbyId)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.LobbyManagerInstance.OnLobbyUpdate?.Invoke(lobbyId);
		}

		[MonoPInvokeCallback]
		private static void OnLobbyDeleteImpl(IntPtr ptr, long lobbyId, uint reason)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.LobbyManagerInstance.OnLobbyDelete?.Invoke(lobbyId, reason);
		}

		[MonoPInvokeCallback]
		private static void OnMemberConnectImpl(IntPtr ptr, long lobbyId, long userId)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.LobbyManagerInstance.OnMemberConnect?.Invoke(lobbyId, userId);
		}

		[MonoPInvokeCallback]
		private static void OnMemberUpdateImpl(IntPtr ptr, long lobbyId, long userId)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.LobbyManagerInstance.OnMemberUpdate?.Invoke(lobbyId, userId);
		}

		[MonoPInvokeCallback]
		private static void OnMemberDisconnectImpl(IntPtr ptr, long lobbyId, long userId)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.LobbyManagerInstance.OnMemberDisconnect?.Invoke(lobbyId, userId);
		}

		[MonoPInvokeCallback]
		private static void OnLobbyMessageImpl(IntPtr ptr, long lobbyId, long userId, IntPtr dataPtr, int dataLen)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.LobbyManagerInstance.OnLobbyMessage == null) return;

			byte[] data = new byte[dataLen];
			Marshal.Copy(dataPtr, data, 0, dataLen);
			d.LobbyManagerInstance.OnLobbyMessage.Invoke(lobbyId, userId, data);
		}

		[MonoPInvokeCallback]
		private static void OnSpeakingImpl(IntPtr ptr, long lobbyId, long userId, bool speaking)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.LobbyManagerInstance.OnSpeaking?.Invoke(lobbyId, userId, speaking);
		}

		[MonoPInvokeCallback]
		private static void OnNetworkMessageImpl(IntPtr ptr, long lobbyId, long userId, byte channelId, IntPtr dataPtr,
			int dataLen)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.LobbyManagerInstance.OnNetworkMessage == null) return;

			byte[] data = new byte[dataLen];
			Marshal.Copy(dataPtr, data, 0, dataLen);
			d.LobbyManagerInstance.OnNetworkMessage.Invoke(lobbyId, userId, channelId, data);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyUpdateHandler(IntPtr ptr, long lobbyId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyDeleteHandler(IntPtr ptr, long lobbyId, uint reason);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberConnectHandler(IntPtr ptr, long lobbyId, long userId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberUpdateHandler(IntPtr ptr, long lobbyId, long userId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberDisconnectHandler(IntPtr ptr, long lobbyId, long userId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyMessageHandler(IntPtr ptr, long lobbyId, long userId, IntPtr dataPtr,
				int dataLen);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SpeakingHandler(IntPtr ptr, long lobbyId, long userId, bool speaking);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void NetworkMessageHandler(IntPtr ptr, long lobbyId, long userId, byte channelId,
				IntPtr dataPtr, int dataLen);

			internal LobbyUpdateHandler OnLobbyUpdate;
			internal LobbyDeleteHandler OnLobbyDelete;
			internal MemberConnectHandler OnMemberConnect;
			internal MemberUpdateHandler OnMemberUpdate;
			internal MemberDisconnectHandler OnMemberDisconnect;
			internal LobbyMessageHandler OnLobbyMessage;
			internal SpeakingHandler OnSpeaking;
			internal NetworkMessageHandler OnNetworkMessage;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyCreateTransactionMethod(IntPtr methodsPtr, ref IntPtr transaction);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyUpdateTransactionMethod(IntPtr methodsPtr, long lobbyId,
				ref IntPtr transaction);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUpdateTransactionMethod(IntPtr methodsPtr, long lobbyId, long userId,
				ref IntPtr transaction);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CreateLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CreateLobbyMethod(IntPtr methodsPtr, IntPtr transaction, IntPtr callbackData,
				CreateLobbyCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateLobbyCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr transaction,
				IntPtr callbackData, UpdateLobbyCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DeleteLobbyCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DeleteLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData,
				DeleteLobbyCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyMethod(IntPtr methodsPtr, long lobbyId,
				[MarshalAs(UnmanagedType.LPStr)] string secret, IntPtr callbackData, ConnectLobbyCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyWithActivitySecretCallback(IntPtr ptr, Result result, ref Lobby lobby);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyWithActivitySecretMethod(IntPtr methodsPtr,
				[MarshalAs(UnmanagedType.LPStr)] string activitySecret, IntPtr callbackData,
				ConnectLobbyWithActivitySecretCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectLobbyCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData,
				DisconnectLobbyCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMethod(IntPtr methodsPtr, long lobbyId, ref Lobby lobby);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result
				GetLobbyActivitySecretMethod(IntPtr methodsPtr, long lobbyId, StringBuilder secret);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMetadataValueMethod(IntPtr methodsPtr, long lobbyId,
				[MarshalAs(UnmanagedType.LPStr)] string key, StringBuilder value);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMetadataKeyMethod(IntPtr methodsPtr, long lobbyId, int index,
				StringBuilder key);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result LobbyMetadataCountMethod(IntPtr methodsPtr, long lobbyId, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result MemberCountMethod(IntPtr methodsPtr, long lobbyId, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUserIdMethod(IntPtr methodsPtr, long lobbyId, int index, ref long userId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUserMethod(IntPtr methodsPtr, long lobbyId, long userId, ref User user);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberMetadataValueMethod(IntPtr methodsPtr, long lobbyId, long userId,
				[MarshalAs(UnmanagedType.LPStr)] string key, StringBuilder value);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberMetadataKeyMethod(IntPtr methodsPtr, long lobbyId, long userId, int index,
				StringBuilder key);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result MemberMetadataCountMethod(IntPtr methodsPtr, long lobbyId, long userId,
				ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateMemberCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateMemberMethod(IntPtr methodsPtr, long lobbyId, long userId, IntPtr transaction,
				IntPtr callbackData, UpdateMemberCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendLobbyMessageCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendLobbyMessageMethod(IntPtr methodsPtr, long lobbyId, byte[] data, int dataLen,
				IntPtr callbackData, SendLobbyMessageCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSearchQueryMethod(IntPtr methodsPtr, ref IntPtr query);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SearchCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SearchMethod(IntPtr methodsPtr, IntPtr query, IntPtr callbackData,
				SearchCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyCountMethod(IntPtr methodsPtr, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyIdMethod(IntPtr methodsPtr, int index, ref long lobbyId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectVoiceCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectVoiceMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData,
				ConnectVoiceCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectVoiceCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectVoiceMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData,
				DisconnectVoiceCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ConnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DisconnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FlushNetworkMethod(IntPtr methodsPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenNetworkChannelMethod(IntPtr methodsPtr, long lobbyId, byte channelId,
				bool reliable);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SendNetworkMessageMethod(IntPtr methodsPtr, long lobbyId, long userId,
				byte channelId, byte[] data, int dataLen);

			internal GetLobbyCreateTransactionMethod GetLobbyCreateTransaction;
			internal GetLobbyUpdateTransactionMethod GetLobbyUpdateTransaction;
			internal GetMemberUpdateTransactionMethod GetMemberUpdateTransaction;
			internal CreateLobbyMethod CreateLobby;
			internal UpdateLobbyMethod UpdateLobby;
			internal DeleteLobbyMethod DeleteLobby;
			internal ConnectLobbyMethod ConnectLobby;
			internal ConnectLobbyWithActivitySecretMethod ConnectLobbyWithActivitySecret;
			internal DisconnectLobbyMethod DisconnectLobby;
			internal GetLobbyMethod GetLobby;
			internal GetLobbyActivitySecretMethod GetLobbyActivitySecret;
			internal GetLobbyMetadataValueMethod GetLobbyMetadataValue;
			internal GetLobbyMetadataKeyMethod GetLobbyMetadataKey;
			internal LobbyMetadataCountMethod LobbyMetadataCount;
			internal MemberCountMethod MemberCount;
			internal GetMemberUserIdMethod GetMemberUserId;
			internal GetMemberUserMethod GetMemberUser;
			internal GetMemberMetadataValueMethod GetMemberMetadataValue;
			internal GetMemberMetadataKeyMethod GetMemberMetadataKey;
			internal MemberMetadataCountMethod MemberMetadataCount;
			internal UpdateMemberMethod UpdateMember;
			internal SendLobbyMessageMethod SendLobbyMessage;
			internal GetSearchQueryMethod GetSearchQuery;
			internal SearchMethod Search;
			internal LobbyCountMethod LobbyCount;
			internal GetLobbyIdMethod GetLobbyId;
			internal ConnectVoiceMethod ConnectVoice;
			internal DisconnectVoiceMethod DisconnectVoice;
			internal ConnectNetworkMethod ConnectNetwork;
			internal DisconnectNetworkMethod DisconnectNetwork;
			internal FlushNetworkMethod FlushNetwork;
			internal OpenNetworkChannelMethod OpenNetworkChannel;
			internal SendNetworkMessageMethod SendNetworkMessage;
		}
	}
}