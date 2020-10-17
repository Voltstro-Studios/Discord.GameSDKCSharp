using System;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Relationships
{
	/// <summary>
	///     This manager helps you access the relationships your players have made on Discord. Unfortunately, it won't help
	///     them make relationships IRL. They're on their own for that. It lets you:
	///     <list type="bullet">
	///         <item>Access a user's relationships</item>
	///         <item>Filter those relationships based on a given criteria</item>
	///         <item>Build a user's friends list</item>
	///     </list>
	/// </summary>
	public class RelationshipManager
	{
		public delegate bool FilterHandler(ref Relationship relationship);

		public delegate void RefreshHandler();

		public delegate void RelationshipUpdateHandler(ref Relationship relationship);

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal RelationshipManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
		///     Fires at initialization when Discord has cached a snapshot of the current status of all your relationships. Wait
		///     for this to fire before calling Filter within its callback.
		/// </summary>
		public event RefreshHandler OnRefresh;

		/// <summary>
		///     Fires when a relationship in the filtered list changes, like an updated presence or user attribute.
		/// </summary>
		public event RelationshipUpdateHandler OnRelationshipUpdate;

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			events.OnRefresh = OnRefreshImpl;
			events.OnRelationshipUpdate = OnRelationshipUpdateImpl;
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		/// <summary>
		///     Filters a user's relationship list by a boolean condition.
		/// </summary>
		/// <param name="callback"></param>
		public void Filter(FilterHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.Filter(methodsPtr, GCHandle.ToIntPtr(wrapped), FilterCallbackImpl);
			wrapped.Free();
		}

		/// <summary>
		///     Get the number of relationships that match your <see cref="Filter" />.
		/// </summary>
		/// <returns></returns>
		public int Count()
		{
			int ret = new int();
			Result res = Methods.Count(methodsPtr, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Get the relationship between the current user and a given user by id.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public Relationship Get(long userId)
		{
			Relationship ret = new Relationship();
			Result res = Methods.Get(methodsPtr, userId, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Get the relationship at a given index when iterating over a list of relationships.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Relationship GetAt(uint index)
		{
			Relationship ret = new Relationship();
			Result res = Methods.GetAt(methodsPtr, index, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		[MonoPInvokeCallback]
		private static bool FilterCallbackImpl(IntPtr ptr, ref Relationship relationship)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			FilterHandler callback = (FilterHandler) h.Target;
			return callback(ref relationship);
		}

		[MonoPInvokeCallback]
		private static void OnRefreshImpl(IntPtr ptr)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.RelationshipManagerInstance.OnRefresh != null) d.RelationshipManagerInstance.OnRefresh.Invoke();
		}

		[MonoPInvokeCallback]
		private static void OnRelationshipUpdateImpl(IntPtr ptr, ref Relationship relationship)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.RelationshipManagerInstance.OnRelationshipUpdate != null)
				d.RelationshipManagerInstance.OnRelationshipUpdate.Invoke(ref relationship);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RefreshHandler(IntPtr ptr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RelationshipUpdateHandler(IntPtr ptr, ref Relationship relationship);

			internal RefreshHandler OnRefresh;

			internal RelationshipUpdateHandler OnRelationshipUpdate;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate bool FilterCallback(IntPtr ptr, ref Relationship relationship);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FilterMethod(IntPtr methodsPtr, IntPtr callbackData, FilterCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CountMethod(IntPtr methodsPtr, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMethod(IntPtr methodsPtr, long userId, ref Relationship relationship);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetAtMethod(IntPtr methodsPtr, uint index, ref Relationship relationship);

			internal FilterMethod Filter;

			internal CountMethod Count;

			internal GetMethod Get;

			internal GetAtMethod GetAt;
		}
	}
}