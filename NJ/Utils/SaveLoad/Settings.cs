using KTEngine;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Chip
{
    [Serializable]
    public class Settings
    {
        public int WindowScale = 4;
        public bool VSync = true;
        public bool Rumble = true;
        public int MusicVolume = 10;
        public int SFXVolume = 10;
        public string Language = "en";
        public static Settings Instance;
        public static bool Existed;
        public bool Fullscreen;
        public bool ShowFPS;
        public const string Filename = "settings";

        public Settings()
        {
#if DEBUG && !CONSOLE && !__MOBILE__
            Fullscreen = false;
            ShowFPS = true;
#else
            Fullscreen = true;
#endif
        }

        public int MaxScale
        {
            get
            {
                return Math.Min(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 320,
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 180) - 1;
            }
        }

        public void AfterLoad()
        {
            MusicVolume = Calc.Clamp(MusicVolume, 0, 10);
            SFXVolume = Calc.Clamp(SFXVolume, 0, 10);
            WindowScale = Math.Min(WindowScale, MaxScale);
            WindowScale = Calc.Clamp(WindowScale, 3, 8);
            SetDefaultControls(false);
        }

        public void SetDefaultControls(bool reset)
        {
        }

        public static void Initialize()
        {
            if (UserIO.Open(UserIO.Mode.Read))
            {
                Instance = UserIO.Load<Settings>(Filename);
                UserIO.Close();
            }
            Existed = Instance != null;
            if (Instance == null)
                Instance = new Settings();
            Instance.AfterLoad();
        }
    }
}