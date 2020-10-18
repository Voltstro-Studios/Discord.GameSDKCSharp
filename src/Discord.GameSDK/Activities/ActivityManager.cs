using System;
using System.Runtime.InteropServices;
using Discord.GameSDK.Users;

namespace Discord.GameSDK.Activities
{
	/// <summary>
	///     Looking to integrate Rich Presence into your game? No need to manage multiple SDKs—this one does all that awesome
	///     stuff, too!.
	///     Delight your players with the ability to post game invites in chat and party up directly from Discord.
	///     Surface interesting live game data in their profile and on the Games Tab for their friends, encouraging them to
	///     group up and play together.
	///     <para>
	///         For more detailed information and documentation around the Rich Presence feature set and integration tips,
	///         check out our
	///         <a href="https://discord.com/developers/docs/rich-presence/how-to">Rich Presence Documentation</a>.
	///     </para>
	/// </summary>
	public sealed class ActivityManager
	{
		public delegate void AcceptInviteHandler(Result result);

		public delegate void ActivityInviteHandler(ActivityActionType type, ref User user, ref Activity activity);

		public delegate void ActivityJoinHandler(string secret);

		public delegate void ActivityJoinRequestHandler(ref User user);

		public delegate void ActivitySpectateHandler(string secret);

		public delegate void ClearActivityHandler(Result result);

		public delegate void SendInviteHandler(Result result);

		public delegate void SendRequestReplyHandler(Result result);

		public delegate void UpdateActivityHandler(Result result);

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal ActivityManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
		///     Fires when a user accepts a game chat invite or receives confirmation from Asking to Join.
		/// </summary>
		public event ActivityJoinHandler OnActivityJoin;

		/// <summary>
		///     Fires when a user accepts a spectate chat invite or clicks the Spectate button on a user's profile.
		/// </summary>
		public event ActivitySpectateHandler OnActivitySpectate;

		/// <summary>
		///     Fires when a user asks to join the current user's game.
		/// </summary>
		public event ActivityJoinRequestHandler OnActivityJoinRequest;

		/// <summary>
		///     Fires when the user receives a join or spectate invite.
		/// </summary>
		public event ActivityInviteHandler OnActivityInvite;

		/// <summary>
		///     Registers a command by which Discord can launch your game.
		///     This might be a custom protocol, like <c>my-awesome-game://</c>, or a path to an executable.
		///     It also supports any launch parameters that may be needed, like <c>game.exe --full-screen --no-hax</c>.
		///     <para>
		///         On macOS, due to the way Discord registers executables, your game needs to be bundled for this command to
		///         work. That means it should be a <c>.app</c>.
		///     </para>
		/// </summary>
		/// <param name="command"></param>
		/// <exception cref="ResultException"></exception>
		public void RegisterCommand(string command)
		{
			Result res = Methods.RegisterCommand(methodsPtr, command);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Used if you are distributing this SDK on Steam. Registers your game's Steam app id for the protocol
		///     <c>steam://run-game-id/id</c>.
		/// </summary>
		/// <param name="steamId"></param>
		/// <exception cref="ResultException"></exception>
		public void RegisterSteam(uint steamId)
		{
			Result res = Methods.RegisterSteam(methodsPtr, steamId);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Sets a user's presence in Discord to a new activity. This has a rate limit of 5 updates per 20 seconds.
		///     <para>
		///         It is possible for users to hide their presence on Discord (User Settings -> Game Activity). Presence set
		///         through this SDK may not be visible when this setting is toggled off.
		///     </para>
		/// </summary>
		/// <param name="activity"></param>
		/// <param name="callback"></param>
		public void UpdateActivity(Activity activity, UpdateActivityHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.UpdateActivity(methodsPtr, ref activity, GCHandle.ToIntPtr(wrapped), UpdateActivityCallbackImpl);
		}

		/// <summary>
		///     Clear's a user's presence in Discord to make it show nothing.
		/// </summary>
		/// <param name="callback"></param>
		public void ClearActivity(ClearActivityHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.ClearActivity(methodsPtr, GCHandle.ToIntPtr(wrapped), ClearActivityCallbackImpl);
		}

		/// <summary>
		///     Sends a reply to an Ask to Join request.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="reply"></param>
		/// <param name="callback"></param>
		public void SendRequestReply(long userId, ActivityJoinRequestReply reply, SendRequestReplyHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.SendRequestReply(methodsPtr, userId, reply, GCHandle.ToIntPtr(wrapped),
				SendRequestReplyCallbackImpl);
		}

		/// <summary>
		///     Sends a game invite to a given user. If you do not have a valid activity with all the required fields, this call
		///     will error.
		///     See
		///     <a href="https://discord.com/developers/docs/game-sdk/activities#activity-action-field-requirements">
		///         Activity
		///         Action Field Requirement
		///     </a>
		///     for the fields required to have join and spectate invites function properly.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="type"></param>
		/// <param name="content"></param>
		/// <param name="callback"></param>
		public void SendInvite(long userId, ActivityActionType type, string content, SendInviteHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.SendInvite(methodsPtr, userId, type, content, GCHandle.ToIntPtr(wrapped), SendInviteCallbackImpl);
		}

		/// <summary>
		///     Accepts a game invitation from a given userId.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="callback"></param>
		public void AcceptInvite(long userId, AcceptInviteHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.AcceptInvite(methodsPtr, userId, GCHandle.ToIntPtr(wrapped), AcceptInviteCallbackImpl);
		}

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			events.OnActivityJoin = OnActivityJoinImpl;
			events.OnActivitySpectate = OnActivitySpectateImpl;
			events.OnActivityJoinRequest = OnActivityJoinRequestImpl;
			events.OnActivityInvite = OnActivityInviteImpl;
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		[MonoPInvokeCallback]
		private static void UpdateActivityCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			UpdateActivityHandler callback = (UpdateActivityHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void ClearActivityCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			ClearActivityHandler callback = (ClearActivityHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void SendRequestReplyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			SendRequestReplyHandler callback = (SendRequestReplyHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void SendInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			SendInviteHandler callback = (SendInviteHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void AcceptInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			AcceptInviteHandler callback = (AcceptInviteHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void OnActivityJoinImpl(IntPtr ptr, string secret)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.ActivityManagerInstance.OnActivityJoin?.Invoke(secret);
		}

		[MonoPInvokeCallback]
		private static void OnActivitySpectateImpl(IntPtr ptr, string secret)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.ActivityManagerInstance.OnActivitySpectate?.Invoke(secret);
		}

		[MonoPInvokeCallback]
		private static void OnActivityJoinRequestImpl(IntPtr ptr, ref User user)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.ActivityManagerInstance.OnActivityJoinRequest?.Invoke(ref user);
		}

		[MonoPInvokeCallback]
		private static void OnActivityInviteImpl(IntPtr ptr, ActivityActionType type, ref User user,
			ref Activity activity)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			d.ActivityManagerInstance.OnActivityInvite?.Invoke(type, ref user, ref activity);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityJoinHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivitySpectateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityJoinRequestHandler(IntPtr ptr, ref User user);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityInviteHandler(IntPtr ptr, ActivityActionType type, ref User user,
				ref Activity activity);

			internal ActivityJoinHandler OnActivityJoin;
			internal ActivitySpectateHandler OnActivitySpectate;
			internal ActivityJoinRequestHandler OnActivityJoinRequest;
			internal ActivityInviteHandler OnActivityInvite;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RegisterCommandMethod(IntPtr methodsPtr,
				[MarshalAs(UnmanagedType.LPStr)] string command);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RegisterSteamMethod(IntPtr methodsPtr, uint steamId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateActivityCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateActivityMethod(IntPtr methodsPtr, ref Activity activity, IntPtr callbackData,
				UpdateActivityCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ClearActivityCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ClearActivityMethod(IntPtr methodsPtr, IntPtr callbackData,
				ClearActivityCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendRequestReplyCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendRequestReplyMethod(IntPtr methodsPtr, long userId,
				ActivityJoinRequestReply reply, IntPtr callbackData, SendRequestReplyCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendInviteCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendInviteMethod(IntPtr methodsPtr, long userId, ActivityActionType type,
				[MarshalAs(UnmanagedType.LPStr)] string content, IntPtr callbackData, SendInviteCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void AcceptInviteCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void AcceptInviteMethod(IntPtr methodsPtr, long userId, IntPtr callbackData,
				AcceptInviteCallback callback);

			internal RegisterCommandMethod RegisterCommand;
			internal RegisterSteamMethod RegisterSteam;
			internal UpdateActivityMethod UpdateActivity;
			internal ClearActivityMethod ClearActivity;
			internal SendRequestReplyMethod SendRequestReply;
			internal SendInviteMethod SendInvite;
			internal AcceptInviteMethod AcceptInvite;
		}
	}
}