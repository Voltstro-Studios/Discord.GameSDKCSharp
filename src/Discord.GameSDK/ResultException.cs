using System;

namespace Discord.GameSDK
{
	public class ResultException : Exception
	{
		public readonly Result Result;

		public ResultException(Result result) : base(result.ToString())
		{
		}
	}
}