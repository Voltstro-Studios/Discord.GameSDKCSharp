using System;

namespace Discord.GameSDK
{
	public class InitializedException : Exception
	{
		public InitializedException(string message) : base(message)
		{
		}
	}
}