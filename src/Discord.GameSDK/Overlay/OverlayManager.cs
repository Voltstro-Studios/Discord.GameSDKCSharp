using System;
using System.Runtime.InteropServices;
using Discord.GameSDK.Activities;

namespace Discord.GameSDK.Overlay
{
	/// <summary>
	///     Discord comes with an awesome built-in overlay, and you may want to make use of it for your game. This manager will
	///     help you do just that! It:
	///     <list type="bullet">
	///         <item>Gives you the current state of the overlay for the user: Locked, enabled, unlocked, open, closed, etc.</item>
	///         <item>Allows you to change that state</item>
	///     </list>
	/// </summary>
	public sealed class OverlayManager
	{
		public delegate void OpenActivityInviteHandler(Result result);

		public delegate void OpenGuildInviteHandler(Result result);

		public delegate void OpenVoiceSettingsHandler(Result result);

		public delegate void SetLockedHandler(Result result);

		public delegate void ToggleHandler(bool locked);

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal OverlayManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
		{
			if (eventsPtr == IntPtr.Zero) throw new ResultException(Result.InternalError);
			InitEvents(eventsPtr, ref events);
			methodsPtr = ptr;
			if (methodsPtr == IntPtr.Zero) throw new ResultException(Result.InternalError);
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
		///     Fires when the overlay is locked or unlocked (a.k.a. opened or closed)
		/// </summary>
		public event ToggleHandler OnToggle;

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			events.OnToggle = OnToggleImpl;
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		/// <summary>
		///     Check whether the user has the overlay enabled or disabled. If the overlay is disabled, all the functionality in
		///     this manager will still work.
		///     The calls will instead focus the Discord client and show the modal there instead.
		/// </summary>
		/// <returns></returns>
		public bool IsEnabled()
		{
			bool ret = new bool();
			Methods.IsEnabled(methodsPtr, ref ret);
			return ret;
		}

		/// <summary>
		///     Check if the overlay is currently locked or unlocked
		/// </summary>
		/// <returns></returns>
		public bool IsLocked()
		{
			bool ret = new bool();
			Methods.IsLocked(methodsPtr, ref ret);
			return ret;
		}

		/// <summary>
		///     Locks or unlocks input in the overlay. Calling SetLocked(true); will also close any modals in the overlay or in-app
		///     from things like IAP purchase flows and disallow input.
		/// </summary>
		/// <param name="locked"></param>
		/// <param name="callback"></param>
		public void SetLocked(bool locked, SetLockedHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.SetLocked(methodsPtr, locked, GCHandle.ToIntPtr(wrapped), SetLockedCallbackImpl);
		}

		/// <summary>
		///     Opens the overlay modal for sending game invitations to users, channels, and servers. If you do not have a valid
		///     activity with all the required fields, this call will error.
		///     See
		///     <a href="https://discord.com/developers/docs/game-sdk/activities#activity-action-field-requirements">
		///         Activity Action Field Requirements
		///     </a>
		///     for the fields required to have join and spectate invites function properly.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public void OpenActivityInvite(ActivityActionType type, OpenActivityInviteHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.OpenActivityInvite(methodsPtr, type, GCHandle.ToIntPtr(wrapped), OpenActivityInviteCallbackImpl);
		}

		/// <summary>
		///     Opens the overlay modal for joining a Discord guild, given its invite code.
		///     An invite code for a server may look something like <c>fortnite</c> for a verified server—the full invite being
		///     <c>discord.gg/fortnite</c>—or something like
		///     <c>rjEeUJq</c> for a non-verified server, the full invite being <c>discord.gg/rjEeUJq</c>.
		///     <para>
		///         Note that a successful <see cref="Result" /> response does not necessarily mean that the user has joined the
		///         guild.
		///         If you want more granular control over and knowledge about users joining your guild, you may want to look into
		///         implementing the
		///         <a href="https://discord.com/developers/docs/topics/oauth2#authorization-code-grant">
		///             <c>guilds.join</c> OAuth2
		///             scope in an authorization code grant
		///         </a>
		///         in conjunction with the
		///         <a href="https://discord.com/developers/docs/resources/guild#add-guild-member">Add Guild Members endpoint</a>.
		///     </para>
		/// </summary>
		/// <param name="code"></param>
		/// <param name="callback"></param>
		public void OpenGuildInvite(string code, OpenGuildInviteHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.OpenGuildInvite(methodsPtr, code, GCHandle.ToIntPtr(wrapped), OpenGuildInviteCallbackImpl);
		}

		/// <summary>
		///     Opens the overlay widget for voice settings for the currently connected application. These settings are unique to
		///     each user within the context of your application.
		///     That means that a user can have different favorite voice settings for each of their games!
		/// </summary>
		/// <param name="callback"></param>
		public void OpenVoiceSettings(OpenVoiceSettingsHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.OpenVoiceSettings(methodsPtr, GCHandle.ToIntPtr(wrapped), OpenVoiceSettingsCallbackImpl);
		}

		[MonoPInvokeCallback]
		private static void SetLockedCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			SetLockedHandler callback = (SetLockedHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void OpenActivityInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			OpenActivityInviteHandler callback = (OpenActivityInviteHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void OpenGuildInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			OpenGuildInviteHandler callback = (OpenGuildInviteHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void OpenVoiceSettingsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			OpenVoiceSettingsHandler callback = (OpenVoiceSettingsHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void OnToggleImpl(IntPtr ptr, bool locked)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.OverlayManagerInstance.OnToggle != null) d.OverlayManagerInstance.OnToggle.Invoke(locked);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ToggleHandler(IntPtr ptr, bool locked);

			internal ToggleHandler OnToggle;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void IsEnabledMethod(IntPtr methodsPtr, ref bool enabled);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void IsLockedMethod(IntPtr methodsPtr, ref bool locked);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLockedCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLockedMethod(IntPtr methodsPtr, bool locked, IntPtr callbackData,
				SetLockedCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenActivityInviteCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenActivityInviteMethod(IntPtr methodsPtr, ActivityActionType type,
				IntPtr callbackData, OpenActivityInviteCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenGuildInviteCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenGuildInviteMethod(IntPtr methodsPtr,
				[MarshalAs(UnmanagedType.LPStr)] string code, IntPtr callbackData, OpenGuildInviteCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenVoiceSettingsCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenVoiceSettingsMethod(IntPtr methodsPtr, IntPtr callbackData,
				OpenVoiceSettingsCallback callback);

			internal IsEnabledMethod IsEnabled;

			internal IsLockedMethod IsLocked;

			internal SetLockedMethod SetLocked;

			internal OpenActivityInviteMethod OpenActivityInvite;

			internal OpenGuildInviteMethod OpenGuildInvite;

			internal OpenVoiceSettingsMethod OpenVoiceSettings;
		}
	}
}