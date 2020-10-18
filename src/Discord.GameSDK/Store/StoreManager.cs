using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Discord.GameSDK.Store
{
	/// <summary>
	///     If your game has DLC or offers in-app purchases, this manager is for you!
	///     The Store Manager allows you to fetch a users' entitlements, as well as being notified when a user is granted an
	///     entitlement from a purchase flow for your game.
	/// </summary>
	public sealed class StoreManager
	{
		public delegate void EntitlementCreateHandler(ref Entitlement entitlement);

		public delegate void EntitlementDeleteHandler(ref Entitlement entitlement);

		public delegate void FetchEntitlementsHandler(Result result);

		public delegate void FetchSkusHandler(Result result);

		public delegate void StartPurchaseHandler(Result result);

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal StoreManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
		///     Fires when the connected user receives a new entitlement, either through purchase or through a developer grant.
		/// </summary>
		public event EntitlementCreateHandler OnEntitlementCreate;

		/// <summary>
		///     Fires when the connected user loses an entitlement, either by expiration, revocation, or consumption in the case of
		///     consumable entitlements.
		/// </summary>
		public event EntitlementDeleteHandler OnEntitlementDelete;

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			events.OnEntitlementCreate = OnEntitlementCreateImpl;
			events.OnEntitlementDelete = OnEntitlementDeleteImpl;
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		/// <summary>
		///     Fetches the list of SKUs for the connected application, readying them for iteration.
		///     <para>
		///         Only SKUs that have a price set will be fetched. If you aren't seeing any SKUs being returned, make sure they
		///         have a price set!
		///     </para>
		/// </summary>
		/// <param name="callback"></param>
		public void FetchSkus(FetchSkusHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.FetchSkus(methodsPtr, GCHandle.ToIntPtr(wrapped), FetchSkusCallbackImpl);
		}

		/// <summary>
		///     Gets all <see cref="Sku" />s. You must call <see cref="FetchSkus" /> first before being able to access SKUs in this
		///     way.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Sku> GetSkus()
		{
			int count = CountSkus();
			List<Sku> skus = new List<Sku>();
			for (int i = 0; i < count; i++) skus.Add(GetSkuAt(i));
			return skus;
		}

		/// <summary>
		///     Get the number of SKUs readied by <see cref="FetchSkus" />.
		/// </summary>
		/// <returns></returns>
		public int CountSkus()
		{
			int ret = new int();
			Methods.CountSkus(methodsPtr, ref ret);
			return ret;
		}

		/// <summary>
		///     Gets a SKU by its ID. You must call <see cref="FetchSkus" /> first before being able to access SKUs in this way.
		/// </summary>
		/// <param name="skuId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public Sku GetSku(long skuId)
		{
			Sku ret = new Sku();
			Result res = Methods.GetSku(methodsPtr, skuId, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Gets a SKU by index when iterating over SKUs. You must call <see cref="FetchSkus" /> first before being able to
		///     access SKUs in this way.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public Sku GetSkuAt(int index)
		{
			Sku ret = new Sku();
			Result res = Methods.GetSkuAt(methodsPtr, index, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Fetches a list of entitlements to which the user is entitled. Applications, DLC, and Bundles will always be
		///     returned.
		///     Consumables will be returned until they are consumed by the application via the HTTP endpoint.
		/// </summary>
		/// <param name="callback"></param>
		public void FetchEntitlements(FetchEntitlementsHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.FetchEntitlements(methodsPtr, GCHandle.ToIntPtr(wrapped), FetchEntitlementsCallbackImpl);
		}

		/// <summary>
		///     Gets all <see cref="Entitlement" />s. You must call <see cref="FetchEntitlements" /> first before being able to
		///     access SKUs in this way.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Entitlement> GetEntitlements()
		{
			int count = CountEntitlements();
			List<Entitlement> entitlements = new List<Entitlement>();
			for (int i = 0; i < count; i++) entitlements.Add(GetEntitlementAt(i));
			return entitlements;
		}

		/// <summary>
		///     Get the number of entitlements readied by <see cref="FetchEntitlements" />. You must call
		///     <see cref="FetchEntitlements" /> first before being able to access SKUs in this way.
		/// </summary>
		/// <returns></returns>
		public int CountEntitlements()
		{
			int ret = new int();
			Methods.CountEntitlements(methodsPtr, ref ret);
			return ret;
		}

		/// <summary>
		///     Gets an entitlement by its id. You must call <see cref="FetchEntitlements" /> first before being able to access
		///     SKUs in this way.
		/// </summary>
		/// <param name="entitlementId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public Entitlement GetEntitlement(long entitlementId)
		{
			Entitlement ret = new Entitlement();
			Result res = Methods.GetEntitlement(methodsPtr, entitlementId, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Gets an entitlement by index when iterating over a user's entitlements. You must call
		///     <see cref="FetchEntitlements" /> first before being able to access SKUs in this way.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public Entitlement GetEntitlementAt(int index)
		{
			Entitlement ret = new Entitlement();
			Result res = Methods.GetEntitlementAt(methodsPtr, index, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Returns whether or not the user is entitled to the given SKU ID. You must call <see cref="FetchEntitlements" />
		///     first before being able to access SKUs in this way.
		/// </summary>
		/// <param name="skuId"></param>
		/// <returns></returns>
		/// <exception cref="ResultException"></exception>
		public bool HasSkuEntitlement(long skuId)
		{
			bool ret = new bool();
			Result res = Methods.HasSkuEntitlement(methodsPtr, skuId, ref ret);
			if (res != Result.Ok) throw new ResultException(res);
			return ret;
		}

		/// <summary>
		///     Opens the overlay to begin the in-app purchase dialogue for the given SKU ID. You must call
		///     <see cref="FetchSkus" /> first before being able to access SKUs in this way.
		///     If the user has enabled the overlay for your game, a purchase modal will appear in the overlay. Otherwise, the
		///     Discord client will be auto-focused with a purchase modal.
		/// </summary>
		/// <param name="skuId"></param>
		/// <param name="callback"></param>
		public void StartPurchase(long skuId, StartPurchaseHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.StartPurchase(methodsPtr, skuId, GCHandle.ToIntPtr(wrapped), StartPurchaseCallbackImpl);
		}

		[MonoPInvokeCallback]
		private static void FetchSkusCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			FetchSkusHandler callback = (FetchSkusHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void FetchEntitlementsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			FetchEntitlementsHandler callback = (FetchEntitlementsHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void StartPurchaseCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			StartPurchaseHandler callback = (StartPurchaseHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void OnEntitlementCreateImpl(IntPtr ptr, ref Entitlement entitlement)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.StoreManagerInstance.OnEntitlementCreate != null)
				d.StoreManagerInstance.OnEntitlementCreate.Invoke(ref entitlement);
		}

		[MonoPInvokeCallback]
		private static void OnEntitlementDeleteImpl(IntPtr ptr, ref Entitlement entitlement)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			Discord d = (Discord) h.Target;
			if (d.StoreManagerInstance.OnEntitlementDelete != null)
				d.StoreManagerInstance.OnEntitlementDelete.Invoke(ref entitlement);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void EntitlementCreateHandler(IntPtr ptr, ref Entitlement entitlement);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void EntitlementDeleteHandler(IntPtr ptr, ref Entitlement entitlement);

			internal EntitlementCreateHandler OnEntitlementCreate;

			internal EntitlementDeleteHandler OnEntitlementDelete;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchSkusCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchSkusMethod(IntPtr methodsPtr, IntPtr callbackData, FetchSkusCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountSkusMethod(IntPtr methodsPtr, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSkuMethod(IntPtr methodsPtr, long skuId, ref Sku sku);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSkuAtMethod(IntPtr methodsPtr, int index, ref Sku sku);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchEntitlementsCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchEntitlementsMethod(IntPtr methodsPtr, IntPtr callbackData,
				FetchEntitlementsCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountEntitlementsMethod(IntPtr methodsPtr, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetEntitlementMethod(IntPtr methodsPtr, long entitlementId,
				ref Entitlement entitlement);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetEntitlementAtMethod(IntPtr methodsPtr, int index, ref Entitlement entitlement);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result HasSkuEntitlementMethod(IntPtr methodsPtr, long skuId, ref bool hasEntitlement);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void StartPurchaseCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void StartPurchaseMethod(IntPtr methodsPtr, long skuId, IntPtr callbackData,
				StartPurchaseCallback callback);

			internal FetchSkusMethod FetchSkus;

			internal CountSkusMethod CountSkus;

			internal GetSkuMethod GetSku;

			internal GetSkuAtMethod GetSkuAt;

			internal FetchEntitlementsMethod FetchEntitlements;

			internal CountEntitlementsMethod CountEntitlements;

			internal GetEntitlementMethod GetEntitlement;

			internal GetEntitlementAtMethod GetEntitlementAt;

			internal HasSkuEntitlementMethod HasSkuEntitlement;

			internal StartPurchaseMethod StartPurchase;
		}
	}
}