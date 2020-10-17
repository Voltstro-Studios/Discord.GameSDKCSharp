using System;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Lobbies
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct LobbyMemberTransaction
	{
		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key,
				[MarshalAs(UnmanagedType.LPStr)] string value);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr,
				[MarshalAs(UnmanagedType.LPStr)] string key);

			internal SetMetadataMethod SetMetadata;

			internal DeleteMetadataMethod DeleteMetadata;
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
		///     Sets metadata value under a given key name for the current user.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void SetMetadata(string key, string value)
		{
			if (MethodsPtr == IntPtr.Zero) return;

			Result res = Methods.SetMetadata(MethodsPtr, key, value);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Sets metadata value under a given key name for the current user.
		/// </summary>
		/// <param name="key"></param>
		public void DeleteMetadata(string key)
		{
			if (MethodsPtr == IntPtr.Zero) return;

			Result res = Methods.DeleteMetadata(MethodsPtr, key);
			if (res != Result.Ok) throw new ResultException(res);
		}
	}
}