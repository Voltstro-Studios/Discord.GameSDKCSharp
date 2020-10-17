using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord.GameSDK.Storage
{
	/// <summary>
	///     We've been told that people playing games want to save their progress as they go, allowing them to come back where
	///     they left off and continue their epic journey of power.
	///     <para>Yeah, roguelikes. Even you.</para>
	///     <para>
	///         Discord's storage manager lets you save data mapped to a key for easy reading, writing, and deleting both
	///         synchronously and asynchronously.
	///         It's saved to a super special directory, the Holy Grail of file mappings, that's unique per Discord user — no
	///         need to worry about your little brother overwriting your save file.
	///     </para>
	///     <para>
	///         Creating this manager will also spawn an IO thread for async reads and writes, so unless you really want to
	///         be blocking, you don't need to be!
	///     </para>
	/// </summary>
	public sealed class StorageManager
	{
		public delegate void ReadAsyncHandler(Result result, byte[] data);

		public delegate void ReadAsyncPartialHandler(Result result, byte[] data);

		public delegate void WriteAsyncHandler(Result result);

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal StorageManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		/// <summary>
		///     Reads data synchronously from the game's allocated save file into a buffer. The file is mapped by key value pairs,
		///     and this function will read data that exists for the given key name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public uint Read(string name, byte[] data)
		{
			uint ret = new uint();
			Result res = Methods.Read(methodsPtr, name, data, data.Length, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Reads data asynchronously from the game's allocated save file into a buffer.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="callback"></param>
		public void ReadAsync(string name, ReadAsyncHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.ReadAsync(methodsPtr, name, GCHandle.ToIntPtr(wrapped), ReadAsyncCallbackImpl);
		}

		/// <summary>
		///     Reads data asynchronously from the game's allocated save file into a buffer, starting at a given offset and up to a
		///     given length.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <param name="callback"></param>
		public void ReadAsyncPartial(string name, ulong offset, ulong length, ReadAsyncPartialHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.ReadAsyncPartial(methodsPtr, name, offset, length, GCHandle.ToIntPtr(wrapped),
				ReadAsyncPartialCallbackImpl);
		}

		/// <summary>
		///     Writes data synchronously to disk, under the given key name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="data"></param>
		public void Write(string name, byte[] data)
		{
			Result res = Methods.Write(methodsPtr, name, data, data.Length);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Writes data asynchronously to disk under the given keyname.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="data"></param>
		/// <param name="callback"></param>
		public void WriteAsync(string name, byte[] data, WriteAsyncHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.WriteAsync(methodsPtr, name, data, data.Length, GCHandle.ToIntPtr(wrapped), WriteAsyncCallbackImpl);
		}

		/// <summary>
		///     Deletes written data for the given key name.
		/// </summary>
		/// <param name="name"></param>
		public void Delete(string name)
		{
			Result res = Methods.Delete(methodsPtr, name);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Checks if data exists for a given key name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool Exists(string name)
		{
			bool ret = new bool();
			Result res = Methods.Exists(methodsPtr, name, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Returns file info for the given key name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public FileStat Stat(string name)
		{
			FileStat ret = new FileStat();
			Result res = Methods.Stat(methodsPtr, name, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Returns the count of files, for iteration.
		/// </summary>
		/// <returns></returns>
		public int Count()
		{
			int ret = new int();
			Methods.Count(methodsPtr, ref ret);
			return ret;
		}

		/// <summary>
		///     Gets all files
		/// </summary>
		/// <returns></returns>
		public IEnumerable<FileStat> Files()
		{
			int fileCount = Count();
			List<FileStat> files = new List<FileStat>();
			for (int i = 0; i < fileCount; i++) files.Add(StatAt(i));
			return files;
		}

		/// <summary>
		///     Returns file info for the given index when iterating over files.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public FileStat StatAt(int index)
		{
			FileStat ret = new FileStat();
			Result res = Methods.StatAt(methodsPtr, index, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Returns the filepath to which Discord saves files if you were to use the SDK's storage manager.
		///     Discord has branch-specific, user-specific saves, so you and your little brother will never overwrite each others'
		///     save files.
		///     If your game already has save file writing logic, you can use this method to get that user-specific path and help
		///     users protect their save files.
		///     <para>Value from environment variable <c>DISCORD_STORAGE_PATH</c></para>
		/// </summary>
		/// <returns></returns>
		public string GetPath()
		{
			StringBuilder ret = new StringBuilder(4096);
			Result res = Methods.GetPath(methodsPtr, ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret.ToString();
		}

		[MonoPInvokeCallback]
		private static void ReadAsyncCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			ReadAsyncHandler callback = (ReadAsyncHandler) h.Target;
			h.Free();
			byte[] data = new byte[dataLen];
			Marshal.Copy(dataPtr, data, 0, dataLen);
			callback(result, data);
		}

		[MonoPInvokeCallback]
		private static void ReadAsyncPartialCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			ReadAsyncPartialHandler callback = (ReadAsyncPartialHandler) h.Target;
			h.Free();
			byte[] data = new byte[dataLen];
			Marshal.Copy(dataPtr, data, 0, dataLen);
			callback(result, data);
		}

		[MonoPInvokeCallback]
		private static void WriteAsyncCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			WriteAsyncHandler callback = (WriteAsyncHandler) h.Target;
			h.Free();
			callback(result);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ReadMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name,
				byte[] data, int dataLen, ref uint read);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name,
				IntPtr callbackData, ReadAsyncCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncPartialCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncPartialMethod(IntPtr methodsPtr,
				[MarshalAs(UnmanagedType.LPStr)] string name, ulong offset, ulong length, IntPtr callbackData,
				ReadAsyncPartialCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result WriteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name,
				byte[] data, int dataLen);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void WriteAsyncCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void WriteAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name,
				byte[] data, int dataLen, IntPtr callbackData, WriteAsyncCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ExistsMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name,
				ref bool exists);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountMethod(IntPtr methodsPtr, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result StatMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name,
				ref FileStat stat);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result StatAtMethod(IntPtr methodsPtr, int index, ref FileStat stat);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetPathMethod(IntPtr methodsPtr, StringBuilder path);

			internal ReadMethod Read;

			internal ReadAsyncMethod ReadAsync;

			internal ReadAsyncPartialMethod ReadAsyncPartial;

			internal WriteMethod Write;

			internal WriteAsyncMethod WriteAsync;

			internal DeleteMethod Delete;

			internal ExistsMethod Exists;

			internal CountMethod Count;

			internal StatMethod Stat;

			internal StatAtMethod StatAt;

			internal GetPathMethod GetPath;
		}
	}
}