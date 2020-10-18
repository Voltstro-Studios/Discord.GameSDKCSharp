using System;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Achievements
{
	/// <summary>
	///     There's no feeling quite like accomplishing a goal that you've set out to achieve.
	///     Is killing 1000 zombies in a game as great an achievement as climbing Mt. Everest? Of course it is, and I didn't
	///     even have to leave my house.
	///     So get off my back, society.
	///     <para>Anyway—Discord has achievements! Show your players just how successful they are.</para>
	///     <para>
	///         Achievements are managed in the <a href="https://discord.com/developers/applications">Developer Portal</a>.
	///         Head over to your application --> <c>Achievements</c> to create and manage achievements for your game.
	///         You'll give them an icon, a name, and a description; then they'll be assigned an id.
	///     </para>
	/// </summary>
	public sealed class AchievementManager
	{
		public delegate void FetchUserAchievementsHandler(Result result);

		public delegate void SetUserAchievementHandler(Result result);

		public delegate void UserAchievementUpdateHandler(ref UserAchievement userAchievement);

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal AchievementManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
		///     Fires when an achievement is updated for the currently connected user
		/// </summary>
		public event UserAchievementUpdateHandler OnUserAchievementUpdate;

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			events.OnUserAchievementUpdate = OnUserAchievementUpdateImpl;
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		/// <summary>
		///     Updates the current user's status for a given achievement. If <see cref="percentComplete" /> is set to 100, the
		///     UnlockedAt field will be automatically updated with the current timestamp.
		/// </summary>
		/// <param name="achievementId"></param>
		/// <param name="percentComplete"></param>
		/// <param name="callback"></param>
		public void SetUserAchievement(long achievementId, byte percentComplete, SetUserAchievementHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.SetUserAchievement(methodsPtr, achievementId, percentComplete, GCHandle.ToIntPtr(wrapped),
				SetUserAchievementCallbackImpl);
		}

		/// <summary>
		///     Loads a stable list of the current user's achievements to iterate over. If the user has any achievements, do your
		///     iteration within the callback of this function.
		/// </summary>
		/// <param name="callback"></param>
		public void FetchUserAchievements(FetchUserAchievementsHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.FetchUserAchievements(methodsPtr, GCHandle.ToIntPtr(wrapped), FetchUserAchievementsCallbackImpl);
		}

		/// <summary>
		///     Counts the list of a user's achievements for iteration.
		/// </summary>
		/// <returns></returns>
		public int CountUserAchievements()
		{
			int ret = new int();
			Methods.CountUserAchievements(methodsPtr, ref ret);
			return ret;
		}

		/// <summary>
		///     Gets the user achievement for the given achievement id.
		///     If you keep a hardcoded mapping of achievement -- id in your codebase, this will be better than iterating over each
		///     achievement.
		///     Make sure to call <see cref="FetchUserAchievements" /> first still.
		/// </summary>
		/// <param name="userAchievementId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public UserAchievement GetUserAchievement(long userAchievementId)
		{
			UserAchievement ret = new UserAchievement();
			Result res = Methods.GetUserAchievement(methodsPtr, userAchievementId, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Gets the user's achievement at a given index of their list of achievements.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public UserAchievement GetUserAchievementAt(int index)
		{
			UserAchievement ret = new UserAchievement();
			Result res = Methods.GetUserAchievementAt(methodsPtr, index, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		[MonoPInvokeCallback]
		private static void SetUserAchievementCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			SetUserAchievementHandler callback = (SetUserAchievementHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void FetchUserAchievementsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			FetchUserAchievementsHandler callback = (FetchUserAchievementsHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void OnUserAchievementUpdateImpl(IntPtr ptr, ref UserAchievement userAchievement)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.AchievementManagerInstance.OnUserAchievementUpdate != null)
				d.AchievementManagerInstance.OnUserAchievementUpdate.Invoke(ref userAchievement);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UserAchievementUpdateHandler(IntPtr ptr, ref UserAchievement userAchievement);

			internal UserAchievementUpdateHandler OnUserAchievementUpdate;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetUserAchievementCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetUserAchievementMethod(IntPtr methodsPtr, long achievementId, byte percentComplete,
				IntPtr callbackData, SetUserAchievementCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchUserAchievementsCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchUserAchievementsMethod(IntPtr methodsPtr, IntPtr callbackData,
				FetchUserAchievementsCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountUserAchievementsMethod(IntPtr methodsPtr, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetUserAchievementMethod(IntPtr methodsPtr, long userAchievementId,
				ref UserAchievement userAchievement);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetUserAchievementAtMethod(IntPtr methodsPtr, int index,
				ref UserAchievement userAchievement);

			internal SetUserAchievementMethod SetUserAchievement;

			internal FetchUserAchievementsMethod FetchUserAchievements;

			internal CountUserAchievementsMethod CountUserAchievements;

			internal GetUserAchievementMethod GetUserAchievement;

			internal GetUserAchievementAtMethod GetUserAchievementAt;
		}
	}
}