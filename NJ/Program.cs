#region Using Statements

using System;

#if __IOS__ || __TVOS__
using Foundation;
using UIKit;
#endif

#endregion

namespace NJ
{
#if __IOS__ || __TVOS__
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
#else
	static class Program
#endif
	{
		private static NJGame game;

		internal static void RunGame()
		{
			game = new NJGame();
			game.Run();
#if !__IOS__ && !__TVOS__
			game.Dispose();
#endif
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
#if !__IOS__ && !__TVOS__
		[STAThread]
#endif
		static void Main(string[] args)
		{
#if __IOS__ || __TVOS__
            UIApplication.Main(args, null, "AppDelegate");
#else
			RunGame();
#endif
		}

#if __IOS__ || __TVOS__
        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
#endif
	}
}
