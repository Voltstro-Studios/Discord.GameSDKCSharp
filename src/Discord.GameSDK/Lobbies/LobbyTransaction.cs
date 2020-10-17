using System;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Lobbies
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct LobbyTransaction
	{
		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetTypeMethod(IntPtr methodsPtr, LobbyType type);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetOwnerMethod(IntPtr methodsPtr, long ownerId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetCapacityMethod(IntPtr methodsPtr, uint capacity);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key,
				[MarshalAs(UnmanagedType.LPStr)] string value);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr,
				[MarshalAs(UnmanagedType.LPStr)] string key);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLockedMethod(IntPtr methodsPtr, bool locked);

			internal SetTypeMethod SetType;

			internal SetOwnerMethod SetOwner;

			internal SetCapacityMethod SetCapacity;

			internal SetMetadataMethod SetMetadata;

			internal DeleteMetadataMethod DeleteMetadata;

			internal SetLockedMethod SetLocked;
		}

		internal IntPtr MethodsPtr;

		internal object MethodsStructure;

		private FFIMethods Methods
		{
			get
			{
				if (MethodsStructure == null) MethodsStructure = Marshal.PtrToStructure(MethodsPtr, typeof(FFIMethods));
				return (FFIMethods) MethodsStructure;
			}
		}

		/// <summary>
		///     Marks a lobby as private or public.
		/// </summary>
		/// <param name="type"></param>
		public void SetType(LobbyType type)
		{
			if (MethodsPtr != IntPtr.Zero)
			{
				Result res = Methods.SetType(MethodsPtr, type);
				if (res != Result.Ok) throw new ResultException(res);
			}
		}

		/// <summary>
		///     Sets a new owner for the lobby.
		///     <para>
		///         This method is only valid for <c>LobbyUpdateTransactions</c> and may cause issues if you set it on a
		///         <c>LobbyCreateTransaction</c>.
		///     </para>
		/// </summary>
		/// <param name="ownerId"></param>
		public void SetOwner(long ownerId)
		{
			if (MethodsPtr != IntPtr.Zero)
			{
				Result res = Methods.SetOwner(MethodsPtr, ownerId);
				if (res != Result.Ok) throw new ResultException(res);
			}
		}

		/// <summary>
		///     Sets a new capacity for the lobby.
		/// </summary>
		/// <param name="capacity"></param>
		public void SetCapacity(uint capacity)
		{
			if (MethodsPtr != IntPtr.Zero)
			{
				Result res = Methods.SetCapacity(MethodsPtr, capacity);
				if (res != Result.Ok) throw new ResultException(res);
			}
		}

		/// <summary>
		///     Sets metadata value under a given key name for the lobby.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void SetMetadata(string key, string value)
		{
			if (MethodsPtr != IntPtr.Zero)
			{
				Result res = Methods.SetMetadata(MethodsPtr, key, value);
				if (res != Result.Ok) throw new ResultException(res);
			}
		}

		/// <summary>
		///     Deletes the lobby metadata for a key.
		/// </summary>
		/// <param name="key"></param>
		public void DeleteMetadata(string key)
		{
			if (MethodsPtr != IntPtr.Zero)
			{
				Result res = Methods.DeleteMetadata(MethodsPtr, key);
				if (res != Result.Ok) throw new ResultException(res);
			}
		}

		/// <summary>
		///     Sets the lobby to locked or unlocked. When locked, new users cannot join the lobby.
		/// </summary>
		/// <param name="locked"></param>
		public void SetLocked(bool locked)
		{
			if (MethodsPtr != IntPtr.Zero)
			{
				Result res = Methods.SetLocked(MethodsPtr, locked);
				if (res != Result.Ok) throw new ResultException(res);
			}
		}
	}
}