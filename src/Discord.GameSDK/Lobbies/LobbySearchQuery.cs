using System;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Lobbies
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct LobbySearchQuery
	{
		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FilterMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key,
				LobbySearchComparison comparison, LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)] string value);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SortMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key,
				LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)] string value);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result LimitMethod(IntPtr methodsPtr, uint limit);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DistanceMethod(IntPtr methodsPtr, LobbySearchDistance distance);

			internal FilterMethod Filter;

			internal SortMethod Sort;

			internal LimitMethod Limit;

			internal DistanceMethod Distance;
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
		///     Filters lobbies based on metadata comparison. Available filter values are <c>owner_id</c>, <c>capacity</c>,
		///     <c>slots</c>, and <c>metadata</c>.
		///     If you are filtering based on metadata, make sure you prepend your key with "<c>metadata.</c>". For example,
		///     filtering on matchmaking rating would be "<c>metadata.matchmaking_rating</c>".
		/// </summary>
		/// <param name="key"></param>
		/// <param name="comparison"></param>
		/// <param name="cast"></param>
		/// <param name="value"></param>
		public void Filter(string key, LobbySearchComparison comparison, LobbySearchCast cast, string value)
		{
			if (MethodsPtr == IntPtr.Zero) return;

			Result res = Methods.Filter(MethodsPtr, key, comparison, cast, value);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Sorts the filtered lobbies based on "near-ness" to a given value.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="cast"></param>
		/// <param name="value"></param>
		public void Sort(string key, LobbySearchCast cast, string value)
		{
			if (MethodsPtr == IntPtr.Zero) return;

			Result res = Methods.Sort(MethodsPtr, key, cast, value);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Limits the number of lobbies returned in a search.
		/// </summary>
		/// <param name="limit"></param>
		public void Limit(uint limit)
		{
			if (MethodsPtr == IntPtr.Zero) return;

			Result res = Methods.Limit(MethodsPtr, limit);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Filters lobby results to within certain regions relative to the user's location.
		/// </summary>
		/// <param name="distance"></param>
		public void Distance(LobbySearchDistance distance)
		{
			if (MethodsPtr == IntPtr.Zero) return;

			Result res = Methods.Distance(MethodsPtr, distance);
			if (res != Result.Ok) throw new ResultException(res);
		}
	}
}