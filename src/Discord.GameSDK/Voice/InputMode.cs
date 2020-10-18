using System.Runtime.InteropServices;

namespace Discord.GameSDK.Voice
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct InputMode
	{
		/// <summary>
		///     Set either VAD or PTT as the voice input mode
		/// </summary>
		public InputModeType Type;

		/// <summary>
		///     The PTT hotkey for the user
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Shortcut;
	}
}