using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord.GameSDK.Applications
{
	/// <summary>
	///     Many games run their own backend servers for things like user authentication. If one of those many games is yours,
	///     then we've got something for you! This manager gives you access to a bearer token for the currently connected
	///     Discord user, which you can send off to your server to do user authentication.
	///     <para>
	///         This token is also useful for retrieving information about the connected user's account. Check out our
	///         <a href="https://discord.com/developers/docs/topics/oauth2">OAuth2 documentation</a> for more information.
	///     </para>
	///     <para>
	///         These bearer tokens are good for seven days, after which they will expire. When a user reconnects to your
	///         game, and is online and connected to the internet, they'll receive a new token that you can grab.
	///     </para>
	///     <para>
	///         This manager also includes a couple useful helper functions, like getting the locale in which the user has
	///         chosen to use their Discord client, and knowing which game branch the game is running on. More about branches
	///         in the Dispatch CLI tool section of the documentation.
	///     </para>
	/// </summary>
	public class ApplicationManager
	{
		public delegate void GetOAuth2TokenHandler(Result result, ref OAuth2Token oauth2Token);

		public delegate void GetTicketHandler(Result result, ref string data);

		public delegate void ValidateOrExitHandler(Result result);

		private readonly IntPtr methodsPtr;

		private object methodsStructure;

		internal ApplicationManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
		///     Checks if the current user has the entitlement to run this game.
		/// </summary>
		/// <param name="callback"></param>
		public void ValidateOrExit(ValidateOrExitHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.ValidateOrExit(methodsPtr, GCHandle.ToIntPtr(wrapped), ValidateOrExitCallbackImpl);
		}

		/// <summary>
		///     Get's the locale the current user has Discord set to.
		///     <para>Value from environment variable <c>DISCORD_CURRENT_LOCALE</c></para>
		/// </summary>
		/// <returns></returns>
		public string GetCurrentLocale()
		{
			StringBuilder ret = new StringBuilder(128);
			Methods.GetCurrentLocale(methodsPtr, ret);
			return ret.ToString();
		}

		/// <summary>
		///     Get the name of pushed branch on which the game is running. These are branches that you created and pushed using
		///     Dispatch.
		///     <para>Value from environment variable <c>DISCORD_CURRENT_BRANCH</c></para>
		/// </summary>
		/// <returns></returns>
		public string GetCurrentBranch()
		{
			StringBuilder ret = new StringBuilder(4096);
			Methods.GetCurrentBranch(methodsPtr, ret);
			return ret.ToString();
		}

		/// <summary>
		///     Retrieve an oauth2 bearer token for the current user. If your game was launched from Discord and you call this
		///     function, you will automatically receive the token.
		///     If the game was not launched from Discord and this method is called, Discord will focus itself and prompt the user
		///     for authorization.
		///     <para>
		///         Ensure that you have <c>http://127.0.0.1</c> set as a valid redirect URI for your application in the
		///         Developer Portal, or this method will always return an error.
		///     </para>
		///     <para>Value from environment variable DISCORD_ACCESS_TOKEN</para>
		/// </summary>
		/// <param name="callback"></param>
		public void GetOAuth2Token(GetOAuth2TokenHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.GetOAuth2Token(methodsPtr, GCHandle.ToIntPtr(wrapped), GetOAuth2TokenCallbackImpl);
		}

		/// <summary>
		///     Get the signed app ticket for the current user.
		///     The structure of the ticket is: <c>version.signature.base64encodedjson</c>, so you should split the string by the
		///     <c>.</c> character.
		///     Ensure that the <c>version</c> matches the current version. The <c>signature</c> is used to verify the ticket using
		///     the libsodium library of your choice,
		///     and the <c>base64encodedjson</c> is what you can transform after verification. It contains:
		///     <list type="bullet">
		///         <item>the application id tied to the ticket</item>
		///         <item>the user's user id</item>
		///         <item>a timestamp for the ticket</item>
		///         <item>the list of the user's <see cref="Store.Entitlement" />s for the application id</item>
		///     </list>
		///     These values can be accessed by transforming the string into a
		///     <a href="https://discord.com/developers/docs/game-sdk/applications#data-models-signedappticket-struct">SignedAppTicket</a>
		///     with your application's private key. The ticket is signed using
		///     <a href="https://github.com/jedisct1/libsodium">libsodium</a> which should be available for any programming
		///     language. Here's a
		///     <a href="https://download.libsodium.org/doc/bindings_for_other_languages">list of available libraries</a>.
		///     <para>
		///         Note that both the public key you receive from Discord and the signature within the app ticket from the SDK
		///         are both in hex, and will need to be converted to <see cref="byte" />[] before use with libsodium.
		///     </para>
		/// </summary>
		/// <param name="callback"></param>
		public void GetTicket(GetTicketHandler callback)
		{
			GCHandle wrapped = GCHandle.Alloc(callback);
			Methods.GetTicket(methodsPtr, GCHandle.ToIntPtr(wrapped), GetTicketCallbackImpl);
		}

		[MonoPInvokeCallback]
		private static void ValidateOrExitCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			ValidateOrExitHandler callback = (ValidateOrExitHandler) h.Target;
			h.Free();
			callback(result);
		}

		[MonoPInvokeCallback]
		private static void GetOAuth2TokenCallbackImpl(IntPtr ptr, Result result, ref OAuth2Token oauth2Token)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			GetOAuth2TokenHandler callback = (GetOAuth2TokenHandler) h.Target;
			h.Free();
			callback(result, ref oauth2Token);
		}

		[MonoPInvokeCallback]
		private static void GetTicketCallbackImpl(IntPtr ptr, Result result, ref string data)
		{
			GCHandle h = GCHandle.FromIntPtr(ptr);
			GetTicketHandler callback = (GetTicketHandler) h.Target;
			h.Free();
			callback(result, ref data);
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIEvents
		{
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ValidateOrExitCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ValidateOrExitMethod(IntPtr methodsPtr, IntPtr callbackData,
				ValidateOrExitCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetCurrentLocaleMethod(IntPtr methodsPtr, StringBuilder locale);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetCurrentBranchMethod(IntPtr methodsPtr, StringBuilder branch);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetOAuth2TokenCallback(IntPtr ptr, Result result, ref OAuth2Token oauth2Token);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetOAuth2TokenMethod(IntPtr methodsPtr, IntPtr callbackData,
				GetOAuth2TokenCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetTicketCallback(IntPtr ptr, Result result,
				[MarshalAs(UnmanagedType.LPStr)] ref string data);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetTicketMethod(IntPtr methodsPtr, IntPtr callbackData, GetTicketCallback callback);

			internal ValidateOrExitMethod ValidateOrExit;

			internal GetCurrentLocaleMethod GetCurrentLocale;

			internal GetCurrentBranchMethod GetCurrentBranch;

			internal GetOAuth2TokenMethod GetOAuth2Token;

			internal GetTicketMethod GetTicket;
		}
	}
}