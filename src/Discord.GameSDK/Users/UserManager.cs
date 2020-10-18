using System;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Users
{
	/// <summary>
	///     This manager helps retrieve basic user information for any user on Discord.
	/// </summary>
	public sealed class UserManager
	{
		public delegate void CurrentUserUpdateHandler();

		public delegate void GetUserHandler(Result result, ref User user);

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal UserManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
		///     Fires when the <see cref="User" /> of the currently connected user changes. They may have changed their avatar,
		///     username, or something else.
		/// </summary>
		public event CurrentUserUpdateHandler OnCurrentUserUpdate;

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			events.OnCurrentUserUpdate = OnCurrentUserUpdateImpl;
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		/// <summary>
		///     Fetch information about the currently connected user account.
		///     <para>
		///         Before calling this function, you'll need to wait for the <see cref="OnCurrentUserUpdate" /> callback to fire
		///         after instantiating the User manager.
		///     </para>
		///     <para>
		///         If you're interested in getting more detailed information about a user—for example, their email—check out our
		///         <a href="https://discord.com/developers/">GetCurrentUser</a> API endpoint.
		///         You'll want to call this with an authorization header of Bearer <c>token</c>, where <c>token</c> is the token
		///         retrieved from a standard
		///         <a href="https://discord.com/developers/docs/topics/oauth2#authorization-code-grant">
		///             OAuth2 Authorization Code
		///             Grant flow
		///         </a>
		///         .
		///     </para>
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public User GetCurrentUser()
		{
			User ret = new User();
			Result res = Methods.GetCurrentUser(methodsPtr, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Get user information for a given id.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="callback"></param>
		public void GetUser(long userId, GetUserHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.GetUser(methodsPtr, userId, GCHandle.ToIntPtr(wrapped), GetUserCallbackImpl);
		}

		/// <summary>
		///     Get the <see cref="PremiumType" /> for the currently connected user.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public PremiumType GetCurrentUserPremiumType()
		{
			PremiumType ret = new PremiumType();
			Result res = Methods.GetCurrentUserPremiumType(methodsPtr, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     See whether or not the current user has a certain <see cref="UserFlag" /> on their account.
		/// </summary>
		/// <param name="flag"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public bool CurrentUserHasFlag(UserFlag flag)
		{
			bool ret = new bool();
			Result res = Methods.CurrentUserHasFlag(methodsPtr, flag, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		[MonoPInvokeCallback]
		private static void GetUserCallbackImpl(IntPtr ptr, Result result, ref User user)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			GetUserHandler callback = (GetUserHandler) h.Target;
			h.Free();
			callback(result, ref user);
		}

		[MonoPInvokeCallback]
		private static void OnCurrentUserUpdateImpl(IntPtr ptr)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.UserManagerInstance.OnCurrentUserUpdate != null) d.UserManagerInstance.OnCurrentUserUpdate.Invoke();
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CurrentUserUpdateHandler(IntPtr ptr);

			internal CurrentUserUpdateHandler OnCurrentUserUpdate;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetCurrentUserMethod(IntPtr methodsPtr, ref User currentUser);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetUserCallback(IntPtr ptr, Result result, ref User user);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetUserMethod(IntPtr methodsPtr, long userId, IntPtr callbackData,
				GetUserCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetCurrentUserPremiumTypeMethod(IntPtr methodsPtr, ref PremiumType premiumType);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CurrentUserHasFlagMethod(IntPtr methodsPtr, UserFlag flag, ref bool hasFlag);

			internal GetCurrentUserMethod GetCurrentUser;

			internal GetUserMethod GetUser;

			internal GetCurrentUserPremiumTypeMethod GetCurrentUserPremiumType;

			internal CurrentUserHasFlagMethod CurrentUserHasFlag;
		}
	}
}