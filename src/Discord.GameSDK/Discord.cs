using System;
using System.Runtime.InteropServices;
using Discord.GameSDK.Achievement;
using Discord.GameSDK.Activities;
using Discord.GameSDK.Images;
using Discord.GameSDK.Lobbies;
using Discord.GameSDK.Networking;
using Discord.GameSDK.Overlay;
using Discord.GameSDK.Storage;
using Discord.GameSDK.Store;
using Discord.GameSDK.Voice;

namespace Discord.GameSDK
{
	/// <summary>
	///     An instance of Discord for the SDK.
	/// </summary>
	public sealed class Discord : IDisposable
	{
		public delegate void SetLogHookHandler(LogLevel level, string message);

		private readonly IntPtr achievementEventsPtr;

		private readonly IntPtr activityEventsPtr;
		private readonly IntPtr applicationEventsPtr;
		private readonly IntPtr eventsPtr;

		private readonly IntPtr imageEventsPtr;

		private readonly IntPtr lobbyEventsPtr;

		private readonly IntPtr methodsPtr;

		private readonly IntPtr networkEventsPtr;

		private readonly IntPtr overlayEventsPtr;

		private readonly IntPtr relationshipEventsPtr;

		private readonly IntPtr storageEventsPtr;

		private readonly IntPtr storeEventsPtr;

		private readonly IntPtr userEventsPtr;

		private readonly IntPtr voiceEventsPtr;
		private AchievementManager.FFIEvents achievementEvents;
		internal AchievementManager AchievementManagerInstance;
		private ActivityManager.FFIEvents activityEvents;
		internal ActivityManager ActivityManagerInstance;

		private ApplicationManager.FFIEvents applicationEvents;
		internal ApplicationManager ApplicationManagerInstance;
		private ImageManager.FFIEvents imageEvents;
		internal ImageManager ImageManagerInstance;
		private LobbyManager.FFIEvents lobbyEvents;
		internal LobbyManager LobbyManagerInstance;
		private object methodsStructure;
		private NetworkManager.FFIEvents networkEvents;
		internal NetworkManager NetworkManagerInstance;
		private OverlayManager.FFIEvents overlayEvents;
		internal OverlayManager OverlayManagerInstance;
		private RelationshipManager.FFIEvents relationshipEvents;
		internal RelationshipManager RelationshipManagerInstance;

		private GCHandle selfHandle;

		private GCHandle? setLogHook;
		private StorageManager.FFIEvents storageEvents;
		internal StorageManager StorageManagerInstance;
		private StoreManager.FFIEvents storeEvents;
		internal StoreManager StoreManagerInstance;
		private UserManager.FFIEvents userEvents;
		internal UserManager UserManagerInstance;
		private VoiceManager.FFIEvents voiceEvents;
		internal VoiceManager VoiceManagerInstance;

		/// <summary>
		///     Creates an instance of Discord to initialize the SDK. This is the overlord of all things Discord. We like to call
		///     her Nelly.
		/// </summary>
		/// <param name="clientId"></param>
		/// <param name="flags"></param>
		/// <exception cref="ResultException"></exception>
		public Discord(long clientId, CreateFlags flags)
		{
			FFICreateParams createParams;
			createParams.ClientId = clientId;
			createParams.Flags = (ulong) flags;

			FFIEvents events = new FFIEvents();
			eventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(events));
			createParams.Events = eventsPtr;

			selfHandle = GCHandle.Alloc(this);
			createParams.EventData = GCHandle.ToIntPtr(selfHandle);

			applicationEvents = new ApplicationManager.FFIEvents();
			applicationEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(applicationEvents));
			createParams.ApplicationEvents = applicationEventsPtr;
			createParams.ApplicationVersion = 1;

			userEvents = new UserManager.FFIEvents();
			userEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(userEvents));
			createParams.UserEvents = userEventsPtr;
			createParams.UserVersion = 1;

			imageEvents = new ImageManager.FFIEvents();
			imageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(imageEvents));
			createParams.ImageEvents = imageEventsPtr;
			createParams.ImageVersion = 1;

			activityEvents = new ActivityManager.FFIEvents();
			activityEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(activityEvents));
			createParams.ActivityEvents = activityEventsPtr;
			createParams.ActivityVersion = 1;

			relationshipEvents = new RelationshipManager.FFIEvents();
			relationshipEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(relationshipEvents));
			createParams.RelationshipEvents = relationshipEventsPtr;
			createParams.RelationshipVersion = 1;

			lobbyEvents = new LobbyManager.FFIEvents();
			lobbyEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(lobbyEvents));
			createParams.LobbyEvents = lobbyEventsPtr;
			createParams.LobbyVersion = 1;

			networkEvents = new NetworkManager.FFIEvents();
			networkEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(networkEvents));
			createParams.NetworkEvents = networkEventsPtr;
			createParams.NetworkVersion = 1;

			overlayEvents = new OverlayManager.FFIEvents();
			overlayEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(overlayEvents));
			createParams.OverlayEvents = overlayEventsPtr;
			createParams.OverlayVersion = 1;

			storageEvents = new StorageManager.FFIEvents();
			storageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(storageEvents));
			createParams.StorageEvents = storageEventsPtr;
			createParams.StorageVersion = 1;

			storeEvents = new StoreManager.FFIEvents();
			storeEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(storeEvents));
			createParams.StoreEvents = storeEventsPtr;
			createParams.StoreVersion = 1;

			voiceEvents = new VoiceManager.FFIEvents();
			voiceEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(voiceEvents));
			createParams.VoiceEvents = voiceEventsPtr;
			createParams.VoiceVersion = 1;

			achievementEvents = new AchievementManager.FFIEvents();
			achievementEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(achievementEvents));
			createParams.AchievementEvents = achievementEventsPtr;
			createParams.AchievementVersion = 1;

			InitEvents(eventsPtr, ref events);

			Result result = DiscordCreate(2, ref createParams, out methodsPtr);
			if (result != Result.Ok)
			{
				Dispose();
				throw new ResultException(result);
			}
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
		///     Destroys the instance. Wave goodbye, Nelly! You monster.
		/// </summary>
		public void Dispose()
		{
			if (methodsPtr != IntPtr.Zero) Methods.Destroy(methodsPtr);

			selfHandle.Free();

			Marshal.FreeHGlobal(eventsPtr);
			Marshal.FreeHGlobal(applicationEventsPtr);
			Marshal.FreeHGlobal(userEventsPtr);
			Marshal.FreeHGlobal(imageEventsPtr);
			Marshal.FreeHGlobal(activityEventsPtr);
			Marshal.FreeHGlobal(relationshipEventsPtr);
			Marshal.FreeHGlobal(lobbyEventsPtr);
			Marshal.FreeHGlobal(networkEventsPtr);
			Marshal.FreeHGlobal(overlayEventsPtr);
			Marshal.FreeHGlobal(storageEventsPtr);
			Marshal.FreeHGlobal(storeEventsPtr);
			Marshal.FreeHGlobal(voiceEventsPtr);
			Marshal.FreeHGlobal(achievementEventsPtr);

			setLogHook?.Free();
		}

		[DllImport(Constants.DllName, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
		private static extern Result DiscordCreate(uint version, ref FFICreateParams createParams, out IntPtr manager);

		/// <summary>
		///     Runs all pending SDK callbacks. Put this in your game's main event loop, like <c>Update()</c> in Unity.
		///     That way, the first thing your game does is check for any new info from Discord.
		///     <para>
		///         This function also serves as a way to know that the local Discord client is still connected.
		///         If the user closes Discord while playing your game, <see cref="RunCallbacks" /> will throw
		///         <see cref="Result.NotRunning" />.
		///     </para>
		/// </summary>
		/// <exception cref="ResultException"></exception>
		public void RunCallbacks()
		{
			Result res = Methods.RunCallbacks(methodsPtr);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Registers a logging callback function with the minimum level of message to receive
		/// </summary>
		/// <param name="minLevel"></param>
		/// <param name="callback"></param>
		public void SetLogHook(LogLevel minLevel, SetLogHookHandler callback)
		{
			setLogHook?.Free();
			setLogHook = GCHandle.Alloc(callback);
			Methods.SetLogHook(methodsPtr, minLevel, GCHandle.ToIntPtr(setLogHook.Value), SetLogHookCallbackImpl);
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with applications in the SDK.
		/// </summary>
		/// <returns></returns>
		public ApplicationManager GetApplicationManager()
		{
			return ApplicationManagerInstance ?? (ApplicationManagerInstance = new ApplicationManager(
				Methods.GetApplicationManager(methodsPtr),
				applicationEventsPtr,
				ref applicationEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with users in the SDK.
		/// </summary>
		/// <returns></returns>
		public UserManager GetUserManager()
		{
			return UserManagerInstance ?? (UserManagerInstance = new UserManager(
				Methods.GetUserManager(methodsPtr),
				userEventsPtr,
				ref userEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with images in the SDK.
		/// </summary>
		/// <returns></returns>
		public ImageManager GetImageManager()
		{
			return ImageManagerInstance ?? (ImageManagerInstance = new ImageManager(
				Methods.GetImageManager(methodsPtr),
				imageEventsPtr,
				ref imageEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with activities in the SDK.
		/// </summary>
		/// <returns></returns>
		public ActivityManager GetActivityManager()
		{
			return ActivityManagerInstance ?? (ActivityManagerInstance = new ActivityManager(
				Methods.GetActivityManager(methodsPtr),
				activityEventsPtr,
				ref activityEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with relationships in the SDK.
		/// </summary>
		/// <returns></returns>
		public RelationshipManager GetRelationshipManager()
		{
			return RelationshipManagerInstance ?? (RelationshipManagerInstance = new RelationshipManager(
				Methods.GetRelationshipManager(methodsPtr),
				relationshipEventsPtr,
				ref relationshipEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with lobbies in the SDK.
		/// </summary>
		/// <returns></returns>
		public LobbyManager GetLobbyManager()
		{
			return LobbyManagerInstance ?? (LobbyManagerInstance = new LobbyManager(
				Methods.GetLobbyManager(methodsPtr),
				lobbyEventsPtr,
				ref lobbyEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with networking in the SDK.
		/// </summary>
		/// <returns></returns>
		public NetworkManager GetNetworkManager()
		{
			return NetworkManagerInstance ?? (NetworkManagerInstance = new NetworkManager(
				Methods.GetNetworkManager(methodsPtr),
				networkEventsPtr,
				ref networkEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with the overlay in the SDK.
		/// </summary>
		/// <returns></returns>
		public OverlayManager GetOverlayManager()
		{
			return OverlayManagerInstance ?? (OverlayManagerInstance = new OverlayManager(
				Methods.GetOverlayManager(methodsPtr),
				overlayEventsPtr,
				ref overlayEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with storage in the SDK.
		/// </summary>
		/// <returns></returns>
		public StorageManager GetStorageManager()
		{
			return StorageManagerInstance ?? (StorageManagerInstance = new StorageManager(
				Methods.GetStorageManager(methodsPtr),
				storageEventsPtr,
				ref storageEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with SKUs and Entitlements in the SDK.
		/// </summary>
		/// <returns></returns>
		public StoreManager GetStoreManager()
		{
			return StoreManagerInstance ?? (StoreManagerInstance = new StoreManager(
				Methods.GetStoreManager(methodsPtr),
				storeEventsPtr,
				ref storeEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with voice chat in the SDK.
		/// </summary>
		/// <returns></returns>
		public VoiceManager GetVoiceManager()
		{
			return VoiceManagerInstance ?? (VoiceManagerInstance = new VoiceManager(
				Methods.GetVoiceManager(methodsPtr),
				voiceEventsPtr,
				ref voiceEvents
			));
		}

		/// <summary>
		///     Fetches an instance of the manager for interfacing with achievements in the SDK.
		/// </summary>
		/// <returns></returns>
		public AchievementManager GetAchievementManager()
		{
			return AchievementManagerInstance ?? (AchievementManagerInstance = new AchievementManager(
				Methods.GetAchievementManager(methodsPtr),
				achievementEventsPtr,
				ref achievementEvents
			));
		}

		private void InitEvents(IntPtr ffiEventPtr, ref FFIEvents events)
		{
			Marshal.StructureToPtr(events, ffiEventPtr, false);
		}

		[MonoPInvokeCallback]
		private static void SetLogHookCallbackImpl(IntPtr ptr, LogLevel level, string message)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			SetLogHookHandler callback = (SetLogHookHandler) h.Target;
			callback(level, message);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DestroyHandler(IntPtr methodsPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RunCallbacksMethod(IntPtr methodsPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookCallback(IntPtr ptr, LogLevel level,
				[MarshalAs(UnmanagedType.LPStr)] string message);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookMethod(IntPtr methodsPtr, LogLevel minLevel, IntPtr callbackData,
				SetLogHookCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetApplicationManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetUserManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetImageManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetActivityManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetRelationshipManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetLobbyManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetNetworkManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetOverlayManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStorageManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStoreManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetVoiceManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetAchievementManagerMethod(IntPtr discordPtr);

			internal DestroyHandler Destroy;

			internal RunCallbacksMethod RunCallbacks;
			internal SetLogHookMethod SetLogHook;
			internal GetApplicationManagerMethod GetApplicationManager;
			internal GetUserManagerMethod GetUserManager;
			internal GetImageManagerMethod GetImageManager;
			internal GetActivityManagerMethod GetActivityManager;
			internal GetRelationshipManagerMethod GetRelationshipManager;
			internal GetLobbyManagerMethod GetLobbyManager;
			internal GetNetworkManagerMethod GetNetworkManager;
			internal GetOverlayManagerMethod GetOverlayManager;
			internal GetStorageManagerMethod GetStorageManager;
			internal GetStoreManagerMethod GetStoreManager;
			internal GetVoiceManagerMethod GetVoiceManager;
			internal GetAchievementManagerMethod GetAchievementManager;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFICreateParams
		{
			internal long ClientId;
			internal ulong Flags;

			internal IntPtr Events;
			internal IntPtr EventData;

			internal IntPtr ApplicationEvents;
			internal uint ApplicationVersion;

			internal IntPtr UserEvents;
			internal uint UserVersion;

			internal IntPtr ImageEvents;
			internal uint ImageVersion;

			internal IntPtr ActivityEvents;
			internal uint ActivityVersion;

			internal IntPtr RelationshipEvents;
			internal uint RelationshipVersion;

			internal IntPtr LobbyEvents;
			internal uint LobbyVersion;

			internal IntPtr NetworkEvents;
			internal uint NetworkVersion;

			internal IntPtr OverlayEvents;
			internal uint OverlayVersion;

			internal IntPtr StorageEvents;
			internal uint StorageVersion;

			internal IntPtr StoreEvents;
			internal uint StoreVersion;

			internal IntPtr VoiceEvents;
			internal uint VoiceVersion;

			internal IntPtr AchievementEvents;
			internal uint AchievementVersion;
		}
	}
}