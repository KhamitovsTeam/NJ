#region Using Statements

using System;
using System.IO;
using System.Runtime.InteropServices;
using KTEngine;
using Microsoft.Xna.Framework;

#if __IOS__ || __TVOS__
using Foundation;
using UIKit;
#endif

#endregion

namespace Chip
{
#if __IOS__ || __TVOS__
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
#else
    static class Program
#endif
	{
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

        private static NJGame _game;

		internal static void RunGame()
		{
#if !DEBUG
			/*try {
				if (!SteamAPI.Init()) {
					Log.Error("SteamAPI.Init() failed!");
					return;
				}
			}
			catch (DllNotFoundException e) { // We check this here as it will be the first instance of it.
				Log.Error(e);
				return;
			}
			
			if (!Packsize.Test()) {
				Log.Error("You're using the wrong Steamworks.NET Assembly for this platform!");
				return;
			}

			if (!DllCheck.Test()) {
				Log.Error("You're using the wrong dlls for this platform!");
				return;
			}*/
#endif
			
			Settings.Initialize();
		    using(_game = new NJGame())
		    {
		        bool isHighDpi = Environment.GetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI") == "1";
		        if (isHighDpi)
			        Log.Message("HiDPI Enabled");
		        _game.Run();
#if !__IOS__ && !__TVOS__
		        _game.Dispose();
#endif
		    }
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
#if !__IOS__ && !__TVOS__
		[STAThread]
#endif
		static void Main(string[] args)
		{
			/* We recommend setting this before touching anything XNA-related! */
			FNALoggerEXT.LogInfo = (msg) => Log.Message($"FNA INFORMATION {msg}");
			FNALoggerEXT.LogWarn = (msg) => Log.Warning($"FNA WARNING {msg}");
			FNALoggerEXT.LogError = (msg) => Log.Error("FNA ERROR {msg}");
			
		    // https://github.com/FNA-XNA/FNA/wiki/4:-FNA-and-Windows-API#64-bit-support
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetDllDirectory(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    Environment.Is64BitProcess ? "x64" : "x86"
                ));
            }
		    
			// https://github.com/FNA-XNA/FNA/wiki/7:-FNA-Environment-Variables#fna_graphics_enable_highdpi
			// NOTE: from documentation: 
			//       Lastly, when packaging for macOS, be sure this is in your app bundle's Info.plist:
			//           <key>NSHighResolutionCapable</key>
			//           <string>True</string>
			//Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");
			
			// Use scancodes instead keycodes
			Environment.SetEnvironmentVariable("FNA_KEYBOARD_USE_SCANCODES", "1");
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
