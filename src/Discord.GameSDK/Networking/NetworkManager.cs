using System;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Networking
{
	public sealed class NetworkManager
	{
		public delegate void MessageHandler(ulong peerId, byte channelId, byte[] data);

		public delegate void RouteUpdateHandler(string routeData);

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal NetworkManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
		///     Fires when you receive data from another user. This callback will only fire if you already have an open channel
		///     with the user sending you data.
		///     <para>Make sure you're running <see cref="Discord.RunCallbacks" /> in your game loop, or you'll never get data!</para>
		/// </summary>
		public event MessageHandler OnMessage;

		/// <summary>
		///     Fires when your networking route has changed.
		///     You should broadcast to other users to whom you are connected that this has changed, probably by updating your
		///     lobby member metadata for others to receive.
		/// </summary>
		public event RouteUpdateHandler OnRouteUpdate;

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			events.OnMessage = OnMessageImpl;
			events.OnRouteUpdate = OnRouteUpdateImpl;
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		/// <summary>
		///     Get the networking peer ID for the current user, allowing other users to send packets to them.
		/// </summary>
		public ulong GetPeerId()
		{
			ulong ret = new ulong();
			Methods.GetPeerId(methodsPtr, ref ret);
			return ret;
		}

		/// <summary>
		///     Flushes the network. Run this at the end of your game's loop, once you've finished sending all you need to send. In
		///     Unity, for example, stick this in <c>LateUpdate()</c>.
		/// </summary>
		/// <exception cref="ResultException"></exception>
		public void Flush()
		{
			Result res = Methods.Flush(methodsPtr);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Opens a network connection to another Discord user.
		/// </summary>
		/// <exception cref="ResultException"></exception>
		public void OpenPeer(ulong peerId, string routeData)
		{
			Result res = Methods.OpenPeer(methodsPtr, peerId, routeData);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Updates the network connection to another Discord user.
		///     You'll want to call this when notified that the route for a user to which you are connected has changed, most
		///     likely from a lobby member update event.
		/// </summary>
		/// <exception cref="ResultException"></exception>
		public void UpdatePeer(ulong peerId, string routeData)
		{
			Result res = Methods.UpdatePeer(methodsPtr, peerId, routeData);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Disconnects the network session to another Discord user.
		/// </summary>
		/// <exception cref="ResultException"></exception>
		public void ClosePeer(ulong peerId)
		{
			Result res = Methods.ClosePeer(methodsPtr, peerId);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Opens a channel to a user with their given peer ID on the given channel number.
		///     <para>
		///         Unreliable channels should be used for loss-tolerant data, like player positioning in the world. Reliable
		///         channels should be used for data that must get to the user, like loot drops!
		///     </para>
		/// </summary>
		/// <exception cref="ResultException"></exception>
		public void OpenChannel(ulong peerId, byte channelId, bool reliable)
		{
			Result res = Methods.OpenChannel(methodsPtr, peerId, channelId, reliable);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Close the connection to a given user by peerId on the given channel.
		/// </summary>
		/// <exception cref="ResultException"></exception>
		public void CloseChannel(ulong peerId, byte channelId)
		{
			Result res = Methods.CloseChannel(methodsPtr, peerId, channelId);
			if (res != Result.Ok) throw new ResultException(res);
		}

		/// <summary>
		///     Sends data to a given peer ID through the given channel.
		/// </summary>
		/// <exception cref="ResultException"></exception>
		public void SendMessage(ulong peerId, byte channelId, byte[] data)
		{
			Result res = Methods.SendMessage(methodsPtr, peerId, channelId, data, data.Length);
			if (res != Result.Ok) throw new ResultException(res);
		}

		[MonoPInvokeCallback]
		private static void OnMessageImpl(IntPtr ptr, ulong peerId, byte channelId, IntPtr dataPtr, int dataLen)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.NetworkManagerInstance.OnMessage != null)
			{
				byte[] data = new byte[dataLen];
				Marshal.Copy(dataPtr, data, 0, dataLen);
				d.NetworkManagerInstance.OnMessage.Invoke(peerId, channelId, data);
			}
		}

		[MonoPInvokeCallback]
		private static void OnRouteUpdateImpl(IntPtr ptr, string routeData)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.NetworkManagerInstance.OnRouteUpdate != null)
				d.NetworkManagerInstance.OnRouteUpdate.Invoke(routeData);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MessageHandler(IntPtr ptr, ulong peerId, byte channelId, IntPtr dataPtr,
				int dataLen);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RouteUpdateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string routeData);

			internal MessageHandler OnMessage;

			internal RouteUpdateHandler OnRouteUpdate;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetPeerIdMethod(IntPtr methodsPtr, ref ulong peerId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FlushMethod(IntPtr methodsPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenPeerMethod(IntPtr methodsPtr, ulong peerId,
				[MarshalAs(UnmanagedType.LPStr)] string routeData);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result UpdatePeerMethod(IntPtr methodsPtr, ulong peerId,
				[MarshalAs(UnmanagedType.LPStr)] string routeData);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ClosePeerMethod(IntPtr methodsPtr, ulong peerId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenChannelMethod(IntPtr methodsPtr, ulong peerId, byte channelId, bool reliable);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CloseChannelMethod(IntPtr methodsPtr, ulong peerId, byte channelId);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SendMessageMethod(IntPtr methodsPtr, ulong peerId, byte channelId, byte[] data,
				int dataLen);

			internal GetPeerIdMethod GetPeerId;

			internal FlushMethod Flush;

			internal OpenPeerMethod OpenPeer;

			internal UpdatePeerMethod UpdatePeer;

			internal ClosePeerMethod ClosePeer;

			internal OpenChannelMethod OpenChannel;

			internal CloseChannelMethod CloseChannel;

			internal SendMessageMethod SendMessage;
		}
	}
}