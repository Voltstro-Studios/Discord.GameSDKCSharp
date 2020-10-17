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
}