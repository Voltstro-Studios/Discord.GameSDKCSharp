using System;
using System.Runtime.InteropServices;
using System.Text;
using Discord.GameSDK;
using Discord.GameSDK.Achievement;
using Discord.GameSDK.Users;
using Discord.GameSDK.Activities;
using Discord.GameSDK.Relationships;
using Discord.GameSDK.Lobbies;
using Discord.GameSDK.Store;
using Discord.GameSDK.Voice;
using Discord.GameSDK.Applications;
using Discord.GameSDK.Images;
using Discord.GameSDK.Storage;

namespace Discord
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public partial struct LobbyTransaction
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SetTypeMethod(IntPtr methodsPtr, LobbyType type);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SetOwnerMethod(IntPtr methodsPtr, Int64 ownerId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SetCapacityMethod(IntPtr methodsPtr, UInt32 capacity);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string key, [MarshalAs(UnmanagedType.LPStr)]string value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string key);

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

        internal Object MethodsStructure;

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

        public void SetType(LobbyType type)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.SetType(MethodsPtr, type);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }

        public void SetOwner(Int64 ownerId)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.SetOwner(MethodsPtr, ownerId);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }

        public void SetCapacity(UInt32 capacity)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.SetCapacity(MethodsPtr, capacity);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }

        public void SetMetadata(string key, string value)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.SetMetadata(MethodsPtr, key, value);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }

        public void DeleteMetadata(string key)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.DeleteMetadata(MethodsPtr, key);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }

        public void SetLocked(bool locked)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.SetLocked(MethodsPtr, locked);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public partial struct LobbyMemberTransaction
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string key, [MarshalAs(UnmanagedType.LPStr)]string value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string key);

            internal SetMetadataMethod SetMetadata;

            internal DeleteMetadataMethod DeleteMetadata;
        }

        internal IntPtr MethodsPtr;

        internal Object MethodsStructure;

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

        public void SetMetadata(string key, string value)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.SetMetadata(MethodsPtr, key, value);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }

        public void DeleteMetadata(string key)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.DeleteMetadata(MethodsPtr, key);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public partial struct LobbySearchQuery
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result FilterMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string key, LobbySearchComparison comparison, LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)]string value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SortMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string key, LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)]string value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result LimitMethod(IntPtr methodsPtr, UInt32 limit);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result DistanceMethod(IntPtr methodsPtr, LobbySearchDistance distance);

            internal FilterMethod Filter;

            internal SortMethod Sort;

            internal LimitMethod Limit;

            internal DistanceMethod Distance;
        }

        internal IntPtr MethodsPtr;

        internal Object MethodsStructure;

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

        public void Filter(string key, LobbySearchComparison comparison, LobbySearchCast cast, string value)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.Filter(MethodsPtr, key, comparison, cast, value);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }

        public void Sort(string key, LobbySearchCast cast, string value)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.Sort(MethodsPtr, key, cast, value);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }

        public void Limit(UInt32 limit)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.Limit(MethodsPtr, limit);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }

        public void Distance(LobbySearchDistance distance)
        {
            if (MethodsPtr != IntPtr.Zero)
            {
                var res = Methods.Distance(MethodsPtr, distance);
                if (res != Result.Ok)
                {
                    throw new ResultException(res);
                }
            }
        }
    }

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

    public partial class ImageManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {

        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FetchCallback(IntPtr ptr, Result result, ImageHandle handleResult);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FetchMethod(IntPtr methodsPtr, ImageHandle handle, bool refresh, IntPtr callbackData, FetchCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetDimensionsMethod(IntPtr methodsPtr, ImageHandle handle, ref ImageDimensions dimensions);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetDataMethod(IntPtr methodsPtr, ImageHandle handle, byte[] data, Int32 dataLen);

            internal FetchMethod Fetch;

            internal GetDimensionsMethod GetDimensions;

            internal GetDataMethod GetData;
        }

        public delegate void FetchHandler(Result result, ImageHandle handleResult);

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

        internal ImageManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
        private static void FetchCallbackImpl(IntPtr ptr, Result result, ImageHandle handleResult)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            FetchHandler callback = (FetchHandler)h.Target;
            h.Free();
            callback(result, handleResult);
        }

        public void Fetch(ImageHandle handle, bool refresh, FetchHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.Fetch(MethodsPtr, handle, refresh, GCHandle.ToIntPtr(wrapped), FetchCallbackImpl);
        }

        public ImageDimensions GetDimensions(ImageHandle handle)
        {
            var ret = new ImageDimensions();
            var res = Methods.GetDimensions(MethodsPtr, handle, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public void GetData(ImageHandle handle, byte[] data)
        {
            var res = Methods.GetData(MethodsPtr, handle, data, data.Length);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }
    }

    public partial class RelationshipManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void RefreshHandler(IntPtr ptr);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void RelationshipUpdateHandler(IntPtr ptr, ref Relationship relationship);

            internal RefreshHandler OnRefresh;

            internal RelationshipUpdateHandler OnRelationshipUpdate;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool FilterCallback(IntPtr ptr, ref Relationship relationship);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FilterMethod(IntPtr methodsPtr, IntPtr callbackData, FilterCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result CountMethod(IntPtr methodsPtr, ref Int32 count);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetMethod(IntPtr methodsPtr, Int64 userId, ref Relationship relationship);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetAtMethod(IntPtr methodsPtr, UInt32 index, ref Relationship relationship);

            internal FilterMethod Filter;

            internal CountMethod Count;

            internal GetMethod Get;

            internal GetAtMethod GetAt;
        }

        public delegate bool FilterHandler(ref Relationship relationship);

        public delegate void RefreshHandler();

        public delegate void RelationshipUpdateHandler(ref Relationship relationship);

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

        public event RefreshHandler OnRefresh;

        public event RelationshipUpdateHandler OnRelationshipUpdate;

        internal RelationshipManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
            events.OnRefresh = OnRefreshImpl;
            events.OnRelationshipUpdate = OnRelationshipUpdateImpl;
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        [MonoPInvokeCallback]
        private static bool FilterCallbackImpl(IntPtr ptr, ref Relationship relationship)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            FilterHandler callback = (FilterHandler)h.Target;
            return callback(ref relationship);
        }

        public void Filter(FilterHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.Filter(MethodsPtr, GCHandle.ToIntPtr(wrapped), FilterCallbackImpl);
            wrapped.Free();
        }

        public Int32 Count()
        {
            var ret = new Int32();
            var res = Methods.Count(MethodsPtr, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public Relationship Get(Int64 userId)
        {
            var ret = new Relationship();
            var res = Methods.Get(MethodsPtr, userId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public Relationship GetAt(UInt32 index)
        {
            var ret = new Relationship();
            var res = Methods.GetAt(MethodsPtr, index, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void OnRefreshImpl(IntPtr ptr)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.RelationshipManagerInstance.OnRefresh != null)
            {
                d.RelationshipManagerInstance.OnRefresh.Invoke();
            }
        }

        [MonoPInvokeCallback]
        private static void OnRelationshipUpdateImpl(IntPtr ptr, ref Relationship relationship)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.RelationshipManagerInstance.OnRelationshipUpdate != null)
            {
                d.RelationshipManagerInstance.OnRelationshipUpdate.Invoke(ref relationship);
            }
        }
    }

    public partial class LobbyManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void LobbyUpdateHandler(IntPtr ptr, Int64 lobbyId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void LobbyDeleteHandler(IntPtr ptr, Int64 lobbyId, UInt32 reason);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void MemberConnectHandler(IntPtr ptr, Int64 lobbyId, Int64 userId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void MemberUpdateHandler(IntPtr ptr, Int64 lobbyId, Int64 userId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void MemberDisconnectHandler(IntPtr ptr, Int64 lobbyId, Int64 userId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void LobbyMessageHandler(IntPtr ptr, Int64 lobbyId, Int64 userId, IntPtr dataPtr, Int32 dataLen);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SpeakingHandler(IntPtr ptr, Int64 lobbyId, Int64 userId, bool speaking);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void NetworkMessageHandler(IntPtr ptr, Int64 lobbyId, Int64 userId, byte channelId, IntPtr dataPtr, Int32 dataLen);

            internal LobbyUpdateHandler OnLobbyUpdate;

            internal LobbyDeleteHandler OnLobbyDelete;

            internal MemberConnectHandler OnMemberConnect;

            internal MemberUpdateHandler OnMemberUpdate;

            internal MemberDisconnectHandler OnMemberDisconnect;

            internal LobbyMessageHandler OnLobbyMessage;

            internal SpeakingHandler OnSpeaking;

            internal NetworkMessageHandler OnNetworkMessage;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetLobbyCreateTransactionMethod(IntPtr methodsPtr, ref IntPtr transaction);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetLobbyUpdateTransactionMethod(IntPtr methodsPtr, Int64 lobbyId, ref IntPtr transaction);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetMemberUpdateTransactionMethod(IntPtr methodsPtr, Int64 lobbyId, Int64 userId, ref IntPtr transaction);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void CreateLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void CreateLobbyMethod(IntPtr methodsPtr, IntPtr transaction, IntPtr callbackData, CreateLobbyCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void UpdateLobbyCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void UpdateLobbyMethod(IntPtr methodsPtr, Int64 lobbyId, IntPtr transaction, IntPtr callbackData, UpdateLobbyCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void DeleteLobbyCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void DeleteLobbyMethod(IntPtr methodsPtr, Int64 lobbyId, IntPtr callbackData, DeleteLobbyCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ConnectLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ConnectLobbyMethod(IntPtr methodsPtr, Int64 lobbyId, [MarshalAs(UnmanagedType.LPStr)]string secret, IntPtr callbackData, ConnectLobbyCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ConnectLobbyWithActivitySecretCallback(IntPtr ptr, Result result, ref Lobby lobby);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ConnectLobbyWithActivitySecretMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string activitySecret, IntPtr callbackData, ConnectLobbyWithActivitySecretCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void DisconnectLobbyCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void DisconnectLobbyMethod(IntPtr methodsPtr, Int64 lobbyId, IntPtr callbackData, DisconnectLobbyCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetLobbyMethod(IntPtr methodsPtr, Int64 lobbyId, ref Lobby lobby);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetLobbyActivitySecretMethod(IntPtr methodsPtr, Int64 lobbyId, StringBuilder secret);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetLobbyMetadataValueMethod(IntPtr methodsPtr, Int64 lobbyId, [MarshalAs(UnmanagedType.LPStr)]string key, StringBuilder value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetLobbyMetadataKeyMethod(IntPtr methodsPtr, Int64 lobbyId, Int32 index, StringBuilder key);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result LobbyMetadataCountMethod(IntPtr methodsPtr, Int64 lobbyId, ref Int32 count);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result MemberCountMethod(IntPtr methodsPtr, Int64 lobbyId, ref Int32 count);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetMemberUserIdMethod(IntPtr methodsPtr, Int64 lobbyId, Int32 index, ref Int64 userId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetMemberUserMethod(IntPtr methodsPtr, Int64 lobbyId, Int64 userId, ref User user);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetMemberMetadataValueMethod(IntPtr methodsPtr, Int64 lobbyId, Int64 userId, [MarshalAs(UnmanagedType.LPStr)]string key, StringBuilder value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetMemberMetadataKeyMethod(IntPtr methodsPtr, Int64 lobbyId, Int64 userId, Int32 index, StringBuilder key);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result MemberMetadataCountMethod(IntPtr methodsPtr, Int64 lobbyId, Int64 userId, ref Int32 count);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void UpdateMemberCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void UpdateMemberMethod(IntPtr methodsPtr, Int64 lobbyId, Int64 userId, IntPtr transaction, IntPtr callbackData, UpdateMemberCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SendLobbyMessageCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SendLobbyMessageMethod(IntPtr methodsPtr, Int64 lobbyId, byte[] data, Int32 dataLen, IntPtr callbackData, SendLobbyMessageCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetSearchQueryMethod(IntPtr methodsPtr, ref IntPtr query);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SearchCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SearchMethod(IntPtr methodsPtr, IntPtr query, IntPtr callbackData, SearchCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void LobbyCountMethod(IntPtr methodsPtr, ref Int32 count);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetLobbyIdMethod(IntPtr methodsPtr, Int32 index, ref Int64 lobbyId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ConnectVoiceCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ConnectVoiceMethod(IntPtr methodsPtr, Int64 lobbyId, IntPtr callbackData, ConnectVoiceCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void DisconnectVoiceCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void DisconnectVoiceMethod(IntPtr methodsPtr, Int64 lobbyId, IntPtr callbackData, DisconnectVoiceCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result ConnectNetworkMethod(IntPtr methodsPtr, Int64 lobbyId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result DisconnectNetworkMethod(IntPtr methodsPtr, Int64 lobbyId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result FlushNetworkMethod(IntPtr methodsPtr);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result OpenNetworkChannelMethod(IntPtr methodsPtr, Int64 lobbyId, byte channelId, bool reliable);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SendNetworkMessageMethod(IntPtr methodsPtr, Int64 lobbyId, Int64 userId, byte channelId, byte[] data, Int32 dataLen);

            internal GetLobbyCreateTransactionMethod GetLobbyCreateTransaction;

            internal GetLobbyUpdateTransactionMethod GetLobbyUpdateTransaction;

            internal GetMemberUpdateTransactionMethod GetMemberUpdateTransaction;

            internal CreateLobbyMethod CreateLobby;

            internal UpdateLobbyMethod UpdateLobby;

            internal DeleteLobbyMethod DeleteLobby;

            internal ConnectLobbyMethod ConnectLobby;

            internal ConnectLobbyWithActivitySecretMethod ConnectLobbyWithActivitySecret;

            internal DisconnectLobbyMethod DisconnectLobby;

            internal GetLobbyMethod GetLobby;

            internal GetLobbyActivitySecretMethod GetLobbyActivitySecret;

            internal GetLobbyMetadataValueMethod GetLobbyMetadataValue;

            internal GetLobbyMetadataKeyMethod GetLobbyMetadataKey;

            internal LobbyMetadataCountMethod LobbyMetadataCount;

            internal MemberCountMethod MemberCount;

            internal GetMemberUserIdMethod GetMemberUserId;

            internal GetMemberUserMethod GetMemberUser;

            internal GetMemberMetadataValueMethod GetMemberMetadataValue;

            internal GetMemberMetadataKeyMethod GetMemberMetadataKey;

            internal MemberMetadataCountMethod MemberMetadataCount;

            internal UpdateMemberMethod UpdateMember;

            internal SendLobbyMessageMethod SendLobbyMessage;

            internal GetSearchQueryMethod GetSearchQuery;

            internal SearchMethod Search;

            internal LobbyCountMethod LobbyCount;

            internal GetLobbyIdMethod GetLobbyId;

            internal ConnectVoiceMethod ConnectVoice;

            internal DisconnectVoiceMethod DisconnectVoice;

            internal ConnectNetworkMethod ConnectNetwork;

            internal DisconnectNetworkMethod DisconnectNetwork;

            internal FlushNetworkMethod FlushNetwork;

            internal OpenNetworkChannelMethod OpenNetworkChannel;

            internal SendNetworkMessageMethod SendNetworkMessage;
        }

        public delegate void CreateLobbyHandler(Result result, ref Lobby lobby);

        public delegate void UpdateLobbyHandler(Result result);

        public delegate void DeleteLobbyHandler(Result result);

        public delegate void ConnectLobbyHandler(Result result, ref Lobby lobby);

        public delegate void ConnectLobbyWithActivitySecretHandler(Result result, ref Lobby lobby);

        public delegate void DisconnectLobbyHandler(Result result);

        public delegate void UpdateMemberHandler(Result result);

        public delegate void SendLobbyMessageHandler(Result result);

        public delegate void SearchHandler(Result result);

        public delegate void ConnectVoiceHandler(Result result);

        public delegate void DisconnectVoiceHandler(Result result);

        public delegate void LobbyUpdateHandler(Int64 lobbyId);

        public delegate void LobbyDeleteHandler(Int64 lobbyId, UInt32 reason);

        public delegate void MemberConnectHandler(Int64 lobbyId, Int64 userId);

        public delegate void MemberUpdateHandler(Int64 lobbyId, Int64 userId);

        public delegate void MemberDisconnectHandler(Int64 lobbyId, Int64 userId);

        public delegate void LobbyMessageHandler(Int64 lobbyId, Int64 userId, byte[] data);

        public delegate void SpeakingHandler(Int64 lobbyId, Int64 userId, bool speaking);

        public delegate void NetworkMessageHandler(Int64 lobbyId, Int64 userId, byte channelId, byte[] data);

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

        public event LobbyUpdateHandler OnLobbyUpdate;

        public event LobbyDeleteHandler OnLobbyDelete;

        public event MemberConnectHandler OnMemberConnect;

        public event MemberUpdateHandler OnMemberUpdate;

        public event MemberDisconnectHandler OnMemberDisconnect;

        public event LobbyMessageHandler OnLobbyMessage;

        public event SpeakingHandler OnSpeaking;

        public event NetworkMessageHandler OnNetworkMessage;

        internal LobbyManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
            events.OnLobbyUpdate = OnLobbyUpdateImpl;
            events.OnLobbyDelete = OnLobbyDeleteImpl;
            events.OnMemberConnect = OnMemberConnectImpl;
            events.OnMemberUpdate = OnMemberUpdateImpl;
            events.OnMemberDisconnect = OnMemberDisconnectImpl;
            events.OnLobbyMessage = OnLobbyMessageImpl;
            events.OnSpeaking = OnSpeakingImpl;
            events.OnNetworkMessage = OnNetworkMessageImpl;
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        public LobbyTransaction GetLobbyCreateTransaction()
        {
            var ret = new LobbyTransaction();
            var res = Methods.GetLobbyCreateTransaction(MethodsPtr, ref ret.MethodsPtr);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public LobbyTransaction GetLobbyUpdateTransaction(Int64 lobbyId)
        {
            var ret = new LobbyTransaction();
            var res = Methods.GetLobbyUpdateTransaction(MethodsPtr, lobbyId, ref ret.MethodsPtr);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public LobbyMemberTransaction GetMemberUpdateTransaction(Int64 lobbyId, Int64 userId)
        {
            var ret = new LobbyMemberTransaction();
            var res = Methods.GetMemberUpdateTransaction(MethodsPtr, lobbyId, userId, ref ret.MethodsPtr);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void CreateLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            CreateLobbyHandler callback = (CreateLobbyHandler)h.Target;
            h.Free();
            callback(result, ref lobby);
        }

        public void CreateLobby(LobbyTransaction transaction, CreateLobbyHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.CreateLobby(MethodsPtr, transaction.MethodsPtr, GCHandle.ToIntPtr(wrapped), CreateLobbyCallbackImpl);
            transaction.MethodsPtr = IntPtr.Zero;
        }

        [MonoPInvokeCallback]
        private static void UpdateLobbyCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            UpdateLobbyHandler callback = (UpdateLobbyHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void UpdateLobby(Int64 lobbyId, LobbyTransaction transaction, UpdateLobbyHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.UpdateLobby(MethodsPtr, lobbyId, transaction.MethodsPtr, GCHandle.ToIntPtr(wrapped), UpdateLobbyCallbackImpl);
            transaction.MethodsPtr = IntPtr.Zero;
        }

        [MonoPInvokeCallback]
        private static void DeleteLobbyCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            DeleteLobbyHandler callback = (DeleteLobbyHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void DeleteLobby(Int64 lobbyId, DeleteLobbyHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.DeleteLobby(MethodsPtr, lobbyId, GCHandle.ToIntPtr(wrapped), DeleteLobbyCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void ConnectLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            ConnectLobbyHandler callback = (ConnectLobbyHandler)h.Target;
            h.Free();
            callback(result, ref lobby);
        }

        public void ConnectLobby(Int64 lobbyId, string secret, ConnectLobbyHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.ConnectLobby(MethodsPtr, lobbyId, secret, GCHandle.ToIntPtr(wrapped), ConnectLobbyCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void ConnectLobbyWithActivitySecretCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            ConnectLobbyWithActivitySecretHandler callback = (ConnectLobbyWithActivitySecretHandler)h.Target;
            h.Free();
            callback(result, ref lobby);
        }

        public void ConnectLobbyWithActivitySecret(string activitySecret, ConnectLobbyWithActivitySecretHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.ConnectLobbyWithActivitySecret(MethodsPtr, activitySecret, GCHandle.ToIntPtr(wrapped), ConnectLobbyWithActivitySecretCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void DisconnectLobbyCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            DisconnectLobbyHandler callback = (DisconnectLobbyHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void DisconnectLobby(Int64 lobbyId, DisconnectLobbyHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.DisconnectLobby(MethodsPtr, lobbyId, GCHandle.ToIntPtr(wrapped), DisconnectLobbyCallbackImpl);
        }

        public Lobby GetLobby(Int64 lobbyId)
        {
            var ret = new Lobby();
            var res = Methods.GetLobby(MethodsPtr, lobbyId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public string GetLobbyActivitySecret(Int64 lobbyId)
        {
            var ret = new StringBuilder(128);
            var res = Methods.GetLobbyActivitySecret(MethodsPtr, lobbyId, ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret.ToString();
        }

        public string GetLobbyMetadataValue(Int64 lobbyId, string key)
        {
            var ret = new StringBuilder(4096);
            var res = Methods.GetLobbyMetadataValue(MethodsPtr, lobbyId, key, ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret.ToString();
        }

        public string GetLobbyMetadataKey(Int64 lobbyId, Int32 index)
        {
            var ret = new StringBuilder(256);
            var res = Methods.GetLobbyMetadataKey(MethodsPtr, lobbyId, index, ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret.ToString();
        }

        public Int32 LobbyMetadataCount(Int64 lobbyId)
        {
            var ret = new Int32();
            var res = Methods.LobbyMetadataCount(MethodsPtr, lobbyId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public Int32 MemberCount(Int64 lobbyId)
        {
            var ret = new Int32();
            var res = Methods.MemberCount(MethodsPtr, lobbyId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public Int64 GetMemberUserId(Int64 lobbyId, Int32 index)
        {
            var ret = new Int64();
            var res = Methods.GetMemberUserId(MethodsPtr, lobbyId, index, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public User GetMemberUser(Int64 lobbyId, Int64 userId)
        {
            var ret = new User();
            var res = Methods.GetMemberUser(MethodsPtr, lobbyId, userId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public string GetMemberMetadataValue(Int64 lobbyId, Int64 userId, string key)
        {
            var ret = new StringBuilder(4096);
            var res = Methods.GetMemberMetadataValue(MethodsPtr, lobbyId, userId, key, ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret.ToString();
        }

        public string GetMemberMetadataKey(Int64 lobbyId, Int64 userId, Int32 index)
        {
            var ret = new StringBuilder(256);
            var res = Methods.GetMemberMetadataKey(MethodsPtr, lobbyId, userId, index, ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret.ToString();
        }

        public Int32 MemberMetadataCount(Int64 lobbyId, Int64 userId)
        {
            var ret = new Int32();
            var res = Methods.MemberMetadataCount(MethodsPtr, lobbyId, userId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void UpdateMemberCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            UpdateMemberHandler callback = (UpdateMemberHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void UpdateMember(Int64 lobbyId, Int64 userId, LobbyMemberTransaction transaction, UpdateMemberHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.UpdateMember(MethodsPtr, lobbyId, userId, transaction.MethodsPtr, GCHandle.ToIntPtr(wrapped), UpdateMemberCallbackImpl);
            transaction.MethodsPtr = IntPtr.Zero;
        }

        [MonoPInvokeCallback]
        private static void SendLobbyMessageCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            SendLobbyMessageHandler callback = (SendLobbyMessageHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void SendLobbyMessage(Int64 lobbyId, byte[] data, SendLobbyMessageHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.SendLobbyMessage(MethodsPtr, lobbyId, data, data.Length, GCHandle.ToIntPtr(wrapped), SendLobbyMessageCallbackImpl);
        }

        public LobbySearchQuery GetSearchQuery()
        {
            var ret = new LobbySearchQuery();
            var res = Methods.GetSearchQuery(MethodsPtr, ref ret.MethodsPtr);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void SearchCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            SearchHandler callback = (SearchHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void Search(LobbySearchQuery query, SearchHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.Search(MethodsPtr, query.MethodsPtr, GCHandle.ToIntPtr(wrapped), SearchCallbackImpl);
            query.MethodsPtr = IntPtr.Zero;
        }

        public Int32 LobbyCount()
        {
            var ret = new Int32();
            Methods.LobbyCount(MethodsPtr, ref ret);
            return ret;
        }

        public Int64 GetLobbyId(Int32 index)
        {
            var ret = new Int64();
            var res = Methods.GetLobbyId(MethodsPtr, index, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void ConnectVoiceCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            ConnectVoiceHandler callback = (ConnectVoiceHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void ConnectVoice(Int64 lobbyId, ConnectVoiceHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.ConnectVoice(MethodsPtr, lobbyId, GCHandle.ToIntPtr(wrapped), ConnectVoiceCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void DisconnectVoiceCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            DisconnectVoiceHandler callback = (DisconnectVoiceHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void DisconnectVoice(Int64 lobbyId, DisconnectVoiceHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.DisconnectVoice(MethodsPtr, lobbyId, GCHandle.ToIntPtr(wrapped), DisconnectVoiceCallbackImpl);
        }

        public void ConnectNetwork(Int64 lobbyId)
        {
            var res = Methods.ConnectNetwork(MethodsPtr, lobbyId);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        public void DisconnectNetwork(Int64 lobbyId)
        {
            var res = Methods.DisconnectNetwork(MethodsPtr, lobbyId);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        public void FlushNetwork()
        {
            var res = Methods.FlushNetwork(MethodsPtr);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        public void OpenNetworkChannel(Int64 lobbyId, byte channelId, bool reliable)
        {
            var res = Methods.OpenNetworkChannel(MethodsPtr, lobbyId, channelId, reliable);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        public void SendNetworkMessage(Int64 lobbyId, Int64 userId, byte channelId, byte[] data)
        {
            var res = Methods.SendNetworkMessage(MethodsPtr, lobbyId, userId, channelId, data, data.Length);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        [MonoPInvokeCallback]
        private static void OnLobbyUpdateImpl(IntPtr ptr, Int64 lobbyId)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.LobbyManagerInstance.OnLobbyUpdate != null)
            {
                d.LobbyManagerInstance.OnLobbyUpdate.Invoke(lobbyId);
            }
        }

        [MonoPInvokeCallback]
        private static void OnLobbyDeleteImpl(IntPtr ptr, Int64 lobbyId, UInt32 reason)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.LobbyManagerInstance.OnLobbyDelete != null)
            {
                d.LobbyManagerInstance.OnLobbyDelete.Invoke(lobbyId, reason);
            }
        }

        [MonoPInvokeCallback]
        private static void OnMemberConnectImpl(IntPtr ptr, Int64 lobbyId, Int64 userId)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.LobbyManagerInstance.OnMemberConnect != null)
            {
                d.LobbyManagerInstance.OnMemberConnect.Invoke(lobbyId, userId);
            }
        }

        [MonoPInvokeCallback]
        private static void OnMemberUpdateImpl(IntPtr ptr, Int64 lobbyId, Int64 userId)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.LobbyManagerInstance.OnMemberUpdate != null)
            {
                d.LobbyManagerInstance.OnMemberUpdate.Invoke(lobbyId, userId);
            }
        }

        [MonoPInvokeCallback]
        private static void OnMemberDisconnectImpl(IntPtr ptr, Int64 lobbyId, Int64 userId)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.LobbyManagerInstance.OnMemberDisconnect != null)
            {
                d.LobbyManagerInstance.OnMemberDisconnect.Invoke(lobbyId, userId);
            }
        }

        [MonoPInvokeCallback]
        private static void OnLobbyMessageImpl(IntPtr ptr, Int64 lobbyId, Int64 userId, IntPtr dataPtr, Int32 dataLen)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.LobbyManagerInstance.OnLobbyMessage != null)
            {
                byte[] data = new byte[dataLen];
                Marshal.Copy(dataPtr, data, 0, (int)dataLen);
                d.LobbyManagerInstance.OnLobbyMessage.Invoke(lobbyId, userId, data);
            }
        }

        [MonoPInvokeCallback]
        private static void OnSpeakingImpl(IntPtr ptr, Int64 lobbyId, Int64 userId, bool speaking)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.LobbyManagerInstance.OnSpeaking != null)
            {
                d.LobbyManagerInstance.OnSpeaking.Invoke(lobbyId, userId, speaking);
            }
        }

        [MonoPInvokeCallback]
        private static void OnNetworkMessageImpl(IntPtr ptr, Int64 lobbyId, Int64 userId, byte channelId, IntPtr dataPtr, Int32 dataLen)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.LobbyManagerInstance.OnNetworkMessage != null)
            {
                byte[] data = new byte[dataLen];
                Marshal.Copy(dataPtr, data, 0, (int)dataLen);
                d.LobbyManagerInstance.OnNetworkMessage.Invoke(lobbyId, userId, channelId, data);
            }
        }
    }

    public partial class NetworkManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void MessageHandler(IntPtr ptr, UInt64 peerId, byte channelId, IntPtr dataPtr, Int32 dataLen);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void RouteUpdateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)]string routeData);

            internal MessageHandler OnMessage;

            internal RouteUpdateHandler OnRouteUpdate;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void GetPeerIdMethod(IntPtr methodsPtr, ref UInt64 peerId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result FlushMethod(IntPtr methodsPtr);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result OpenPeerMethod(IntPtr methodsPtr, UInt64 peerId, [MarshalAs(UnmanagedType.LPStr)]string routeData);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result UpdatePeerMethod(IntPtr methodsPtr, UInt64 peerId, [MarshalAs(UnmanagedType.LPStr)]string routeData);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result ClosePeerMethod(IntPtr methodsPtr, UInt64 peerId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result OpenChannelMethod(IntPtr methodsPtr, UInt64 peerId, byte channelId, bool reliable);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result CloseChannelMethod(IntPtr methodsPtr, UInt64 peerId, byte channelId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SendMessageMethod(IntPtr methodsPtr, UInt64 peerId, byte channelId, byte[] data, Int32 dataLen);

            internal GetPeerIdMethod GetPeerId;

            internal FlushMethod Flush;

            internal OpenPeerMethod OpenPeer;

            internal UpdatePeerMethod UpdatePeer;

            internal ClosePeerMethod ClosePeer;

            internal OpenChannelMethod OpenChannel;

            internal CloseChannelMethod CloseChannel;

            internal SendMessageMethod SendMessage;
        }

        public delegate void MessageHandler(UInt64 peerId, byte channelId, byte[] data);

        public delegate void RouteUpdateHandler(string routeData);

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

        public event MessageHandler OnMessage;

        public event RouteUpdateHandler OnRouteUpdate;

        internal NetworkManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
            events.OnMessage = OnMessageImpl;
            events.OnRouteUpdate = OnRouteUpdateImpl;
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        /// <summary>
        /// Get the local peer ID for this process.
        /// </summary>
        public UInt64 GetPeerId()
        {
            var ret = new UInt64();
            Methods.GetPeerId(MethodsPtr, ref ret);
            return ret;
        }

        /// <summary>
        /// Send pending network messages.
        /// </summary>
        public void Flush()
        {
            var res = Methods.Flush(MethodsPtr);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        /// <summary>
        /// Open a connection to a remote peer.
        /// </summary>
        public void OpenPeer(UInt64 peerId, string routeData)
        {
            var res = Methods.OpenPeer(MethodsPtr, peerId, routeData);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        /// <summary>
        /// Update the route data for a connected peer.
        /// </summary>
        public void UpdatePeer(UInt64 peerId, string routeData)
        {
            var res = Methods.UpdatePeer(MethodsPtr, peerId, routeData);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        /// <summary>
        /// Close the connection to a remote peer.
        /// </summary>
        public void ClosePeer(UInt64 peerId)
        {
            var res = Methods.ClosePeer(MethodsPtr, peerId);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        /// <summary>
        /// Open a message channel to a connected peer.
        /// </summary>
        public void OpenChannel(UInt64 peerId, byte channelId, bool reliable)
        {
            var res = Methods.OpenChannel(MethodsPtr, peerId, channelId, reliable);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        /// <summary>
        /// Close a message channel to a connected peer.
        /// </summary>
        public void CloseChannel(UInt64 peerId, byte channelId)
        {
            var res = Methods.CloseChannel(MethodsPtr, peerId, channelId);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        /// <summary>
        /// Send a message to a connected peer over an opened message channel.
        /// </summary>
        public void SendMessage(UInt64 peerId, byte channelId, byte[] data)
        {
            var res = Methods.SendMessage(MethodsPtr, peerId, channelId, data, data.Length);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        [MonoPInvokeCallback]
        private static void OnMessageImpl(IntPtr ptr, UInt64 peerId, byte channelId, IntPtr dataPtr, Int32 dataLen)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.NetworkManagerInstance.OnMessage != null)
            {
                byte[] data = new byte[dataLen];
                Marshal.Copy(dataPtr, data, 0, (int)dataLen);
                d.NetworkManagerInstance.OnMessage.Invoke(peerId, channelId, data);
            }
        }

        [MonoPInvokeCallback]
        private static void OnRouteUpdateImpl(IntPtr ptr, string routeData)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.NetworkManagerInstance.OnRouteUpdate != null)
            {
                d.NetworkManagerInstance.OnRouteUpdate.Invoke(routeData);
            }
        }
    }

    public partial class OverlayManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ToggleHandler(IntPtr ptr, bool locked);

            internal ToggleHandler OnToggle;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void IsEnabledMethod(IntPtr methodsPtr, ref bool enabled);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void IsLockedMethod(IntPtr methodsPtr, ref bool locked);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SetLockedCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SetLockedMethod(IntPtr methodsPtr, bool locked, IntPtr callbackData, SetLockedCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void OpenActivityInviteCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void OpenActivityInviteMethod(IntPtr methodsPtr, ActivityActionType type, IntPtr callbackData, OpenActivityInviteCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void OpenGuildInviteCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void OpenGuildInviteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string code, IntPtr callbackData, OpenGuildInviteCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void OpenVoiceSettingsCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void OpenVoiceSettingsMethod(IntPtr methodsPtr, IntPtr callbackData, OpenVoiceSettingsCallback callback);

            internal IsEnabledMethod IsEnabled;

            internal IsLockedMethod IsLocked;

            internal SetLockedMethod SetLocked;

            internal OpenActivityInviteMethod OpenActivityInvite;

            internal OpenGuildInviteMethod OpenGuildInvite;

            internal OpenVoiceSettingsMethod OpenVoiceSettings;
        }

        public delegate void SetLockedHandler(Result result);

        public delegate void OpenActivityInviteHandler(Result result);

        public delegate void OpenGuildInviteHandler(Result result);

        public delegate void OpenVoiceSettingsHandler(Result result);

        public delegate void ToggleHandler(bool locked);

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

        public event ToggleHandler OnToggle;

        internal OverlayManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
            events.OnToggle = OnToggleImpl;
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        public bool IsEnabled()
        {
            var ret = new bool();
            Methods.IsEnabled(MethodsPtr, ref ret);
            return ret;
        }

        public bool IsLocked()
        {
            var ret = new bool();
            Methods.IsLocked(MethodsPtr, ref ret);
            return ret;
        }

        [MonoPInvokeCallback]
        private static void SetLockedCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            SetLockedHandler callback = (SetLockedHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void SetLocked(bool locked, SetLockedHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.SetLocked(MethodsPtr, locked, GCHandle.ToIntPtr(wrapped), SetLockedCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void OpenActivityInviteCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            OpenActivityInviteHandler callback = (OpenActivityInviteHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void OpenActivityInvite(ActivityActionType type, OpenActivityInviteHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.OpenActivityInvite(MethodsPtr, type, GCHandle.ToIntPtr(wrapped), OpenActivityInviteCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void OpenGuildInviteCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            OpenGuildInviteHandler callback = (OpenGuildInviteHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void OpenGuildInvite(string code, OpenGuildInviteHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.OpenGuildInvite(MethodsPtr, code, GCHandle.ToIntPtr(wrapped), OpenGuildInviteCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void OpenVoiceSettingsCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            OpenVoiceSettingsHandler callback = (OpenVoiceSettingsHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void OpenVoiceSettings(OpenVoiceSettingsHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.OpenVoiceSettings(MethodsPtr, GCHandle.ToIntPtr(wrapped), OpenVoiceSettingsCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void OnToggleImpl(IntPtr ptr, bool locked)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.OverlayManagerInstance.OnToggle != null)
            {
                d.OverlayManagerInstance.OnToggle.Invoke(locked);
            }
        }
    }

    public partial class StorageManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {

        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result ReadMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string name, byte[] data, Int32 dataLen, ref UInt32 read);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ReadAsyncCallback(IntPtr ptr, Result result, IntPtr dataPtr, Int32 dataLen);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ReadAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string name, IntPtr callbackData, ReadAsyncCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ReadAsyncPartialCallback(IntPtr ptr, Result result, IntPtr dataPtr, Int32 dataLen);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void ReadAsyncPartialMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string name, UInt64 offset, UInt64 length, IntPtr callbackData, ReadAsyncPartialCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result WriteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string name, byte[] data, Int32 dataLen);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void WriteAsyncCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void WriteAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string name, byte[] data, Int32 dataLen, IntPtr callbackData, WriteAsyncCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result DeleteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string name);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result ExistsMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string name, ref bool exists);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void CountMethod(IntPtr methodsPtr, ref Int32 count);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result StatMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)]string name, ref FileStat stat);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result StatAtMethod(IntPtr methodsPtr, Int32 index, ref FileStat stat);

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

        public delegate void ReadAsyncHandler(Result result, byte[] data);

        public delegate void ReadAsyncPartialHandler(Result result, byte[] data);

        public delegate void WriteAsyncHandler(Result result);

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

        internal StorageManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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

        public UInt32 Read(string name, byte[] data)
        {
            var ret = new UInt32();
            var res = Methods.Read(MethodsPtr, name, data, data.Length, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void ReadAsyncCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, Int32 dataLen)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            ReadAsyncHandler callback = (ReadAsyncHandler)h.Target;
            h.Free();
            byte[] data = new byte[dataLen];
            Marshal.Copy(dataPtr, data, 0, (int)dataLen);
            callback(result, data);
        }

        public void ReadAsync(string name, ReadAsyncHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.ReadAsync(MethodsPtr, name, GCHandle.ToIntPtr(wrapped), ReadAsyncCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void ReadAsyncPartialCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, Int32 dataLen)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            ReadAsyncPartialHandler callback = (ReadAsyncPartialHandler)h.Target;
            h.Free();
            byte[] data = new byte[dataLen];
            Marshal.Copy(dataPtr, data, 0, (int)dataLen);
            callback(result, data);
        }

        public void ReadAsyncPartial(string name, UInt64 offset, UInt64 length, ReadAsyncPartialHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.ReadAsyncPartial(MethodsPtr, name, offset, length, GCHandle.ToIntPtr(wrapped), ReadAsyncPartialCallbackImpl);
        }

        public void Write(string name, byte[] data)
        {
            var res = Methods.Write(MethodsPtr, name, data, data.Length);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        [MonoPInvokeCallback]
        private static void WriteAsyncCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            WriteAsyncHandler callback = (WriteAsyncHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void WriteAsync(string name, byte[] data, WriteAsyncHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.WriteAsync(MethodsPtr, name, data, data.Length, GCHandle.ToIntPtr(wrapped), WriteAsyncCallbackImpl);
        }

        public void Delete(string name)
        {
            var res = Methods.Delete(MethodsPtr, name);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        public bool Exists(string name)
        {
            var ret = new bool();
            var res = Methods.Exists(MethodsPtr, name, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public Int32 Count()
        {
            var ret = new Int32();
            Methods.Count(MethodsPtr, ref ret);
            return ret;
        }

        public FileStat Stat(string name)
        {
            var ret = new FileStat();
            var res = Methods.Stat(MethodsPtr, name, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public FileStat StatAt(Int32 index)
        {
            var ret = new FileStat();
            var res = Methods.StatAt(MethodsPtr, index, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public string GetPath()
        {
            var ret = new StringBuilder(4096);
            var res = Methods.GetPath(MethodsPtr, ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret.ToString();
        }
    }

    public partial class StoreManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void EntitlementCreateHandler(IntPtr ptr, ref Entitlement entitlement);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void EntitlementDeleteHandler(IntPtr ptr, ref Entitlement entitlement);

            internal EntitlementCreateHandler OnEntitlementCreate;

            internal EntitlementDeleteHandler OnEntitlementDelete;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FetchSkusCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FetchSkusMethod(IntPtr methodsPtr, IntPtr callbackData, FetchSkusCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void CountSkusMethod(IntPtr methodsPtr, ref Int32 count);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetSkuMethod(IntPtr methodsPtr, Int64 skuId, ref Sku sku);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetSkuAtMethod(IntPtr methodsPtr, Int32 index, ref Sku sku);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FetchEntitlementsCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FetchEntitlementsMethod(IntPtr methodsPtr, IntPtr callbackData, FetchEntitlementsCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void CountEntitlementsMethod(IntPtr methodsPtr, ref Int32 count);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetEntitlementMethod(IntPtr methodsPtr, Int64 entitlementId, ref Entitlement entitlement);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetEntitlementAtMethod(IntPtr methodsPtr, Int32 index, ref Entitlement entitlement);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result HasSkuEntitlementMethod(IntPtr methodsPtr, Int64 skuId, ref bool hasEntitlement);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void StartPurchaseCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void StartPurchaseMethod(IntPtr methodsPtr, Int64 skuId, IntPtr callbackData, StartPurchaseCallback callback);

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

        public delegate void FetchSkusHandler(Result result);

        public delegate void FetchEntitlementsHandler(Result result);

        public delegate void StartPurchaseHandler(Result result);

        public delegate void EntitlementCreateHandler(ref Entitlement entitlement);

        public delegate void EntitlementDeleteHandler(ref Entitlement entitlement);

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

        public event EntitlementCreateHandler OnEntitlementCreate;

        public event EntitlementDeleteHandler OnEntitlementDelete;

        internal StoreManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
            events.OnEntitlementCreate = OnEntitlementCreateImpl;
            events.OnEntitlementDelete = OnEntitlementDeleteImpl;
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        [MonoPInvokeCallback]
        private static void FetchSkusCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            FetchSkusHandler callback = (FetchSkusHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void FetchSkus(FetchSkusHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.FetchSkus(MethodsPtr, GCHandle.ToIntPtr(wrapped), FetchSkusCallbackImpl);
        }

        public Int32 CountSkus()
        {
            var ret = new Int32();
            Methods.CountSkus(MethodsPtr, ref ret);
            return ret;
        }

        public Sku GetSku(Int64 skuId)
        {
            var ret = new Sku();
            var res = Methods.GetSku(MethodsPtr, skuId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public Sku GetSkuAt(Int32 index)
        {
            var ret = new Sku();
            var res = Methods.GetSkuAt(MethodsPtr, index, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void FetchEntitlementsCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            FetchEntitlementsHandler callback = (FetchEntitlementsHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void FetchEntitlements(FetchEntitlementsHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.FetchEntitlements(MethodsPtr, GCHandle.ToIntPtr(wrapped), FetchEntitlementsCallbackImpl);
        }

        public Int32 CountEntitlements()
        {
            var ret = new Int32();
            Methods.CountEntitlements(MethodsPtr, ref ret);
            return ret;
        }

        public Entitlement GetEntitlement(Int64 entitlementId)
        {
            var ret = new Entitlement();
            var res = Methods.GetEntitlement(MethodsPtr, entitlementId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public Entitlement GetEntitlementAt(Int32 index)
        {
            var ret = new Entitlement();
            var res = Methods.GetEntitlementAt(MethodsPtr, index, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public bool HasSkuEntitlement(Int64 skuId)
        {
            var ret = new bool();
            var res = Methods.HasSkuEntitlement(MethodsPtr, skuId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void StartPurchaseCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            StartPurchaseHandler callback = (StartPurchaseHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void StartPurchase(Int64 skuId, StartPurchaseHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.StartPurchase(MethodsPtr, skuId, GCHandle.ToIntPtr(wrapped), StartPurchaseCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void OnEntitlementCreateImpl(IntPtr ptr, ref Entitlement entitlement)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.StoreManagerInstance.OnEntitlementCreate != null)
            {
                d.StoreManagerInstance.OnEntitlementCreate.Invoke(ref entitlement);
            }
        }

        [MonoPInvokeCallback]
        private static void OnEntitlementDeleteImpl(IntPtr ptr, ref Entitlement entitlement)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.StoreManagerInstance.OnEntitlementDelete != null)
            {
                d.StoreManagerInstance.OnEntitlementDelete.Invoke(ref entitlement);
            }
        }
    }

    public partial class VoiceManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SettingsUpdateHandler(IntPtr ptr);

            internal SettingsUpdateHandler OnSettingsUpdate;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetInputModeMethod(IntPtr methodsPtr, ref InputMode inputMode);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SetInputModeCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SetInputModeMethod(IntPtr methodsPtr, InputMode inputMode, IntPtr callbackData, SetInputModeCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result IsSelfMuteMethod(IntPtr methodsPtr, ref bool mute);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SetSelfMuteMethod(IntPtr methodsPtr, bool mute);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result IsSelfDeafMethod(IntPtr methodsPtr, ref bool deaf);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SetSelfDeafMethod(IntPtr methodsPtr, bool deaf);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result IsLocalMuteMethod(IntPtr methodsPtr, Int64 userId, ref bool mute);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SetLocalMuteMethod(IntPtr methodsPtr, Int64 userId, bool mute);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetLocalVolumeMethod(IntPtr methodsPtr, Int64 userId, ref byte volume);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result SetLocalVolumeMethod(IntPtr methodsPtr, Int64 userId, byte volume);

            internal GetInputModeMethod GetInputMode;

            internal SetInputModeMethod SetInputMode;

            internal IsSelfMuteMethod IsSelfMute;

            internal SetSelfMuteMethod SetSelfMute;

            internal IsSelfDeafMethod IsSelfDeaf;

            internal SetSelfDeafMethod SetSelfDeaf;

            internal IsLocalMuteMethod IsLocalMute;

            internal SetLocalMuteMethod SetLocalMute;

            internal GetLocalVolumeMethod GetLocalVolume;

            internal SetLocalVolumeMethod SetLocalVolume;
        }

        public delegate void SetInputModeHandler(Result result);

        public delegate void SettingsUpdateHandler();

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

        public event SettingsUpdateHandler OnSettingsUpdate;

        internal VoiceManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
            events.OnSettingsUpdate = OnSettingsUpdateImpl;
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        public InputMode GetInputMode()
        {
            var ret = new InputMode();
            var res = Methods.GetInputMode(MethodsPtr, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void SetInputModeCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            SetInputModeHandler callback = (SetInputModeHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void SetInputMode(InputMode inputMode, SetInputModeHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.SetInputMode(MethodsPtr, inputMode, GCHandle.ToIntPtr(wrapped), SetInputModeCallbackImpl);
        }

        public bool IsSelfMute()
        {
            var ret = new bool();
            var res = Methods.IsSelfMute(MethodsPtr, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public void SetSelfMute(bool mute)
        {
            var res = Methods.SetSelfMute(MethodsPtr, mute);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        public bool IsSelfDeaf()
        {
            var ret = new bool();
            var res = Methods.IsSelfDeaf(MethodsPtr, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public void SetSelfDeaf(bool deaf)
        {
            var res = Methods.SetSelfDeaf(MethodsPtr, deaf);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        public bool IsLocalMute(Int64 userId)
        {
            var ret = new bool();
            var res = Methods.IsLocalMute(MethodsPtr, userId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public void SetLocalMute(Int64 userId, bool mute)
        {
            var res = Methods.SetLocalMute(MethodsPtr, userId, mute);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        public byte GetLocalVolume(Int64 userId)
        {
            var ret = new byte();
            var res = Methods.GetLocalVolume(MethodsPtr, userId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public void SetLocalVolume(Int64 userId, byte volume)
        {
            var res = Methods.SetLocalVolume(MethodsPtr, userId, volume);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
        }

        [MonoPInvokeCallback]
        private static void OnSettingsUpdateImpl(IntPtr ptr)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.VoiceManagerInstance.OnSettingsUpdate != null)
            {
                d.VoiceManagerInstance.OnSettingsUpdate.Invoke();
            }
        }
    }

    public partial class AchievementManager
    {
        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIEvents
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void UserAchievementUpdateHandler(IntPtr ptr, ref UserAchievement userAchievement);

            internal UserAchievementUpdateHandler OnUserAchievementUpdate;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SetUserAchievementCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void SetUserAchievementMethod(IntPtr methodsPtr, Int64 achievementId, byte percentComplete, IntPtr callbackData, SetUserAchievementCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FetchUserAchievementsCallback(IntPtr ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FetchUserAchievementsMethod(IntPtr methodsPtr, IntPtr callbackData, FetchUserAchievementsCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void CountUserAchievementsMethod(IntPtr methodsPtr, ref Int32 count);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetUserAchievementMethod(IntPtr methodsPtr, Int64 userAchievementId, ref UserAchievement userAchievement);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Result GetUserAchievementAtMethod(IntPtr methodsPtr, Int32 index, ref UserAchievement userAchievement);

            internal SetUserAchievementMethod SetUserAchievement;

            internal FetchUserAchievementsMethod FetchUserAchievements;

            internal CountUserAchievementsMethod CountUserAchievements;

            internal GetUserAchievementMethod GetUserAchievement;

            internal GetUserAchievementAtMethod GetUserAchievementAt;
        }

        public delegate void SetUserAchievementHandler(Result result);

        public delegate void FetchUserAchievementsHandler(Result result);

        public delegate void UserAchievementUpdateHandler(ref UserAchievement userAchievement);

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

        public event UserAchievementUpdateHandler OnUserAchievementUpdate;

        internal AchievementManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
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
            events.OnUserAchievementUpdate = OnUserAchievementUpdateImpl;
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        [MonoPInvokeCallback]
        private static void SetUserAchievementCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            SetUserAchievementHandler callback = (SetUserAchievementHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void SetUserAchievement(Int64 achievementId, byte percentComplete, SetUserAchievementHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.SetUserAchievement(MethodsPtr, achievementId, percentComplete, GCHandle.ToIntPtr(wrapped), SetUserAchievementCallbackImpl);
        }

        [MonoPInvokeCallback]
        private static void FetchUserAchievementsCallbackImpl(IntPtr ptr, Result result)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            FetchUserAchievementsHandler callback = (FetchUserAchievementsHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void FetchUserAchievements(FetchUserAchievementsHandler callback)
        {
            GCHandle wrapped = GCHandle.Alloc(callback);
            Methods.FetchUserAchievements(MethodsPtr, GCHandle.ToIntPtr(wrapped), FetchUserAchievementsCallbackImpl);
        }

        public Int32 CountUserAchievements()
        {
            var ret = new Int32();
            Methods.CountUserAchievements(MethodsPtr, ref ret);
            return ret;
        }

        public UserAchievement GetUserAchievement(Int64 userAchievementId)
        {
            var ret = new UserAchievement();
            var res = Methods.GetUserAchievement(MethodsPtr, userAchievementId, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        public UserAchievement GetUserAchievementAt(Int32 index)
        {
            var ret = new UserAchievement();
            var res = Methods.GetUserAchievementAt(MethodsPtr, index, ref ret);
            if (res != Result.Ok)
            {
                throw new ResultException(res);
            }
            return ret;
        }

        [MonoPInvokeCallback]
        private static void OnUserAchievementUpdateImpl(IntPtr ptr, ref UserAchievement userAchievement)
        {
            GCHandle h = GCHandle.FromIntPtr(ptr);
            GameSDK.Discord d = (GameSDK.Discord)h.Target;
            if (d.AchievementManagerInstance.OnUserAchievementUpdate != null)
            {
                d.AchievementManagerInstance.OnUserAchievementUpdate.Invoke(ref userAchievement);
            }
        }
    }
}
