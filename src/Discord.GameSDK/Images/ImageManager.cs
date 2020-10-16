using System;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Images
{
	/// <summary>
	///     Discord is like a book; it's better with pictures. The <see cref="ImageManager" /> helps you fetch image data for
	///     images in Discord, including user's avatars.
	///     They worked hard to pick out those photos and gifs. Show them you care, too.
	/// </summary>
	public sealed class ImageManager
	{
		public delegate void FetchHandler(Result result, ImageHandle handleResult);

		private readonly IntPtr methodsPtr;
		private object methodsStructure;

		internal ImageManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
		{
			if (eventsPtr == IntPtr.Zero)
				throw new ResultException(Result.InternalError);

			InitEvents(eventsPtr, ref events);

			methodsPtr = ptr;
			if (methodsPtr == IntPtr.Zero)
				throw new ResultException(Result.InternalError);
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
		///     Prepares an image to later retrieve data about it.
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="callback"></param>
		public void Fetch(ImageHandle handle, FetchHandler callback)
		{
			Fetch(handle, false, callback);
		}

		/// <summary>
		///     Prepares an image to later retrieve data about it.
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="refresh"></param>
		/// <param name="callback"></param>
		public void Fetch(ImageHandle handle, bool refresh, FetchHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.Fetch(methodsPtr, handle, refresh, GCHandle.ToIntPtr(wrapped), FetchCallbackImpl);
		}

		/// <summary>
		///     Get's the dimensions for the given user's avatar's source image.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public ImageDimensions GetDimensions(ImageHandle handle)
		{
			ImageDimensions ret = new ImageDimensions();
			Result res = Methods.GetDimensions(methodsPtr, handle, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Gets the image data for a given user's avatar.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public byte[] GetData(ImageHandle handle)
		{
			ImageDimensions dimensions = GetDimensions(handle);
			byte[] data = new byte[dimensions.Width * dimensions.Height * 4];
			GetData(handle, data);
			return data;
		}

		/// <summary>
		///     Gets the image data for a given user's avatar.
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="data"></param>
		public void GetData(ImageHandle handle, byte[] data)
		{
			Result res = Methods.GetData(methodsPtr, handle, data, data.Length);
			if (res != Result.Ok) throw new ResultException(res);
		}

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		[MonoPInvokeCallback]
		private static void FetchCallbackImpl(IntPtr ptr, Result result, ImageHandle handleResult)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			FetchHandler callback = (FetchHandler) h.Target;
			h.Free();
			callback(result, handleResult);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchCallback(IntPtr ptr, Result result, ImageHandle handleResult);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchMethod(IntPtr methodsPtr, ImageHandle handle, bool refresh, IntPtr callbackData,
				FetchCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetDimensionsMethod(IntPtr methodsPtr, ImageHandle handle,
				ref ImageDimensions dimensions);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetDataMethod(IntPtr methodsPtr, ImageHandle handle, byte[] data, int dataLen);

			internal FetchMethod Fetch;
			internal GetDimensionsMethod GetDimensions;
			internal GetDataMethod GetData;
		}
	}
}