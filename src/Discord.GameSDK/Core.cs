using System;
using System.Runtime.InteropServices;
using System.Text;
using Discord.GameSDK;
using Discord.GameSDK.Users;
using Discord.GameSDK.Applications;

namespace Discord
{
	public partial class ResultException : Exception
    {
        public readonly Result Result;

        public ResultException(Result result) : base(result.ToString())
        {
        }
    }

    internal partial class MonoPInvokeCallbackAttribute : Attribute
    {

    }

    public partial class ApplicationManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {

        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ValidateOrExitCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ValidateOrExitMethod(IntPtr methodsPtr, IntPtr callbackData, ValidateOrExitCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void GetCurrentLocaleMethod(IntPtr methodsPtr, StringBuilder locale);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void GetCurrentBranchMethod(IntPtr methodsPtr, StringBuilder branch);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void GetOAuth2TokenCallback(IntPtr ptr, Result result, ref OAuth2Token oauth2Token);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void GetOAuth2TokenMethod(IntPtr methodsPtr, IntPtr callbackData, GetOAuth2TokenCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void GetTicketCallback(IntPtr ptr, Result result, [MarshalAs(UnmanagedType.LPStr)]ref string data);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void GetTicketMethod(IntPtr methodsPtr, IntPtr callbackData, GetTicketCallback callback);

            internal ValidateOrExitMethod ValidateOrExit;

            internal GetCurrentLocaleMethod GetCurrentLocale;

            internal GetCurrentBranchMethod GetCurrentBranch;

            internal GetOAuth2TokenMethod GetOAuth2Token;

            internal GetTicketMethod GetTicket;
        }

        public delegate void ValidateOrExitHandler(Result result);

        public delegate void GetOAuth2TokenHandler(Result result, ref OAuth2Token oauth2Token);

        public delegate void GetTicketHandler(Result result, ref string data);

        private IntPtr MethodsPtr;

        private Object MethodsStructure;

        private FFIMethods Methods
        {
            get
            {
                if (MethodsStructure == null)
                {
                    MethodsStructure = Marshal.PtrToStructure(MethodsPtr, typeof(FFIMethods));
                }
                return (FFIMethods)MethodsStructure;
            }

        }

        internal ApplicationManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
        {
            if (eventsPtr == IntPtr.Zero) {
                throw new ResultException(Result.InternalError);
            }
            InitEvents(eventsPtr, ref events);
            MethodsPtr = ptr;
            if (MethodsPtr == IntPtr.Zero) {
                throw new ResultException(Result.InternalError);
            }
        }

        private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
        {
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        [MonoPInvokeCallback]
        private static void ValidateOrExitCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            ValidateOrExitHandler callback = (ValidateOrExitHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void ValidateOrExit(ValidateOrExitHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.ValidateOrExit(MethodsPtr, GCHandle.ToIntPtr(wrapped), ValidateOrExitCallbackImpl);
        }

        public string GetCurrentLocale()
        {
            var ret = new StringBuilder(128);
            Methods.GetCurrentLocale(MethodsPtr, ret);
            return ret.ToString();
        }

        public string GetCurrentBranch()
        {
            var ret = new StringBuilder(4096);
            Methods.GetCurrentBranch(MethodsPtr, ret);
            return ret.ToString();
        }

        [MonoPInvokeCallback]
        private static void GetOAuth2TokenCallbackImpl(IntPtr ptr, Result result, ref OAuth2Token oauth2Token)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GetOAuth2TokenHandler callback = (GetOAuth2TokenHandler)h.Target;
            h.Free();
            callback(result, ref oauth2Token);
        }

        public void GetOAuth2Token(GetOAuth2TokenHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.GetOAuth2Token(MethodsPtr, GCHandle.ToIntPtr(wrapped), GetOAuth2TokenCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void GetTicketCallbackImpl(IntPtr ptr, Result result, ref string data)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GetTicketHandler callback = (GetTicketHandler)h.Target;
            h.Free();
            callback(result, ref data);
        }

        public void GetTicket(GetTicketHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.GetTicket(MethodsPtr, GCHandle.ToIntPtr(wrapped), GetTicketCallbackImpl);
        }
    }

    public partial class UserManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void CurrentUserUpdateHandler(IntPtr ptr);

            internal CurrentUserUpdateHandler OnCurrentUserUpdate;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetCurrentUserMethod(IntPtr methodsPtr, ref User currentUser);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void GetUserCallback(IntPtr ptr, Result result, ref User user);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void GetUserMethod(IntPtr methodsPtr, Int64 userId, IntPtr callbackData, GetUserCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetCurrentUserPremiumTypeMethod(IntPtr methodsPtr, ref PremiumType premiumType);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result CurrentUserHasFlagMethod(IntPtr methodsPtr, UserFlag flag, ref bool hasFlag);

            internal GetCurrentUserMethod GetCurrentUser;

            internal GetUserMethod GetUser;

            internal GetCurrentUserPremiumTypeMethod GetCurrentUserPremiumType;

            internal CurrentUserHasFlagMethod CurrentUserHasFlag;
        }

        public delegate void GetUserHandler(Result result, ref User user);

        public delegate void CurrentUserUpdateHandler();

        private IntPtr MethodsPtr;

        private Object MethodsStructure;

        private FFIMethods Methods
        {
            get
            {
                if (MethodsStructure == null)
                {
                    MethodsStructure = Marshal.PtrToStructure(MethodsPtr, typeof(FFIMethods));
                }
                return (FFIMethods)MethodsStructure;
            }

        }

        public event CurrentUserUpdateHandler OnCurrentUserUpdate;

        internal UserManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
        {
            if (eventsPtr == IntPtr.Zero) {
                throw new ResultException(Result.InternalError);
            }
            InitEvents(eventsPtr, ref events);
            MethodsPtr = ptr;
            if (MethodsPtr == IntPtr.Zero) {
                throw new ResultException(Result.InternalError);
            }
        }

        private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
        {
            events.OnCurrentUserUpdate = OnCurrentUserUpdateImpl;
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        public User GetCurrentUser()
        {
            var ret = new User();
            var res = Methods.GetCurrentUser(MethodsPtr, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void GetUserCallbackImpl(IntPtr ptr, Result result, ref User user)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GetUserHandler callback = (GetUserHandler)h.Target;
            h.Free();
            callback(result, ref user);
        }

        public void GetUser(Int64 userId, GetUserHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.GetUser(MethodsPtr, userId, GCHandle.ToIntPtr(wrapped), GetUserCallbackImpl);
        }

        public PremiumType GetCurrentUserPremiumType()
        {
            var ret = new PremiumType();
            var res = Methods.GetCurrentUserPremiumType(MethodsPtr, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public bool CurrentUserHasFlag(UserFlag flag)
        {
            var ret = new bool();
            var res = Methods.CurrentUserHasFlag(MethodsPtr, flag, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void OnCurrentUserUpdateImpl(IntPtr ptr)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.UserManagerInstance.OnCurrentUserUpdate != null)
            {
                d.UserManagerInstance.OnCurrentUserUpdate.Invoke();
            }
        }
    }
}