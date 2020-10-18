using System;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Voice
{
	/// <summary>
	///     Discord's pride and joy is its voice chat. Well, ok, also its memes, but mostly the voice chat. Text and video chat
	///     are pretty great, too. And have you seen that store? Anyway.
	///     <para>
	///         If you want people playing your game to be able to talk with each other, this <see cref="VoiceManager" /> can
	///         help you out!
	///         Note that the main functionality for voice in this SDK is not only in this manager.
	///         Connecting players to a voice chat happens with <see cref="Lobbies.LobbyManager.ConnectVoice" />, and robust
	///         voice settings work through <see cref="Overlay.OverlayManager.OpenVoiceSettings"/>.
	///         The Voice manager handles a few fine-grain details like self muting/deafening, swapping between VAD/PTT voice
	///         modes, and setting a PTT key.
	///         It's a subset of the robust settings from the overlay call for those of you that prefer to build UI and control
	///         things from your own game.
	///     </para>
	/// </summary>
	public sealed class VoiceManager
	{
		public delegate void SetInputModeHandler(Result result);

		public delegate void SettingsUpdateHandler();

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal VoiceManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
		///     [No Docs, but I am gonna assume it invoked when settings are updated]
		/// </summary>
		public event SettingsUpdateHandler OnSettingsUpdate;

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			events.OnSettingsUpdate = OnSettingsUpdateImpl;
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		/// <summary>
		///     Get the current voice input mode for the user.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public InputMode GetInputMode()
		{
			InputMode ret = new InputMode();
			Result res = Methods.GetInputMode(methodsPtr, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Sets a new voice input mode for the user. Refer to
		///     <a href="https://discord.com/developers/docs/game-sdk/discord-voice#data-models-shortcut-keys">Shortcut Keys</a>
		///     for a table of valid values for shortcuts.
		/// </summary>
		/// <param name="inputMode"></param>
		/// <param name="callback"></param>
		public void SetInputMode(InputMode inputMode, SetInputModeHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.SetInputMode(methodsPtr, inputMode, GCHandle.ToIntPtr(wrapped), SetInputModeCallbackImpl);
		}

		/// <summary>
		///     Whether the connected user is currently muted.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public bool IsSelfMute()
		{
			bool ret = new bool();
			Result res = Methods.IsSelfMute(methodsPtr, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Mutes or unmutes the currently connected user.
		/// </summary>
		/// <param name="mute"></param>
		/// <exception cref="ResultException"></exception>
		public void SetSelfMute(bool mute)
		{
			Result res = Methods.SetSelfMute(methodsPtr, mute);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Whether the connected user is currently deafened.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public bool IsSelfDeaf()
		{
			bool ret = new bool();
			Result res = Methods.IsSelfDeaf(methodsPtr, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Deafens or undefeans the currently connected user.
		/// </summary>
		/// <param name="deaf"></param>
		/// <exception cref="ResultException"></exception>
		public void SetSelfDeaf(bool deaf)
		{
			Result res = Methods.SetSelfDeaf(methodsPtr, deaf);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Whether the given user is currently muted by the connected user.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public bool IsLocalMute(long userId)
		{
			bool ret = new bool();
			Result res = Methods.IsLocalMute(methodsPtr, userId, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Mutes or unmutes the given user for the currently connected user.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="mute"></param>
		/// <exception cref="ResultException"></exception>
		public void SetLocalMute(long userId, bool mute)
		{
			Result res = Methods.SetLocalMute(methodsPtr, userId, mute);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Gets the local volume for a given user. This is the volume level at which the currently connected users hears the
		///     given user speak.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public byte GetLocalVolume(long userId)
		{
			byte ret = new byte();
			Result res = Methods.GetLocalVolume(methodsPtr, userId, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Sets the local volume for a given user. This is the volume level at which the currently connected users hears the
		///     given user speak.
		///     Valid volume values are from 0 to 200, with 100 being the default. Lower than 100 will be a reduced volume level
		///     from default, whereas over 100 will be a boosted volume level from default.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="volume"></param>
		/// <exception cref="ResultException"></exception>
		public void SetLocalVolume(long userId, byte volume)
		{
			Result res = Methods.SetLocalVolume(methodsPtr, userId, volume);
			if (res != Result.Ok) throw new ResultException(res);
		}

		[MonoPInvokeCallback]
		private static void SetInputModeCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			SetInputModeHandler callback = (SetInputModeHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void OnSettingsUpdateImpl(IntPtr ptr)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.VoiceManagerInstance.OnSettingsUpdate != null) d.VoiceManagerInstance.OnSettingsUpdate.Invoke();
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SettingsUpdateHandler(IntPtr ptr);

			internal SettingsUpdateHandler OnSettingsUpdate;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetInputModeMethod(IntPtr methodsPtr, ref InputMode inputMode);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetInputModeCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetInputModeMethod(IntPtr methodsPtr, InputMode inputMode, IntPtr callbackData,
				SetInputModeCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsSelfMuteMethod(IntPtr methodsPtr, ref bool mute);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetSelfMuteMethod(IntPtr methodsPtr, bool mute);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsSelfDeafMethod(IntPtr methodsPtr, ref bool deaf);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetSelfDeafMethod(IntPtr methodsPtr, bool deaf);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsLocalMuteMethod(IntPtr methodsPtr, long userId, ref bool mute);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLocalMuteMethod(IntPtr methodsPtr, long userId, bool mute);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLocalVolumeMethod(IntPtr methodsPtr, long userId, ref byte volume);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLocalVolumeMethod(IntPtr methodsPtr, long userId, byte volume);

			internal GetInputModeMethod GetInputMode;

			internal SetInputModeMethod SetInputMode;

			internal IsSelfMuteMethod IsSelfMute;

			internal SetSelfMuteMethod SetSelfMute;

			internal IsSelfDeafMethod IsSelfDeaf;

			internal SetSelfDeafMethod SetSelfDeaf;

			internal IsLocalMuteMethod IsLocalMute;

			internal SetLocalMuteMethod SetLocalMute;

			internal GetLocalVolumeMethod GetLocalVolume;

			internal SetLocalVolumeMethod SetLocalVolume;
		}
	}
}