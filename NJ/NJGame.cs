using KTEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Chip
{
    public class NJGame : Engine
    {
        public static VirtualRenderTarget HudTarget;

        public NJGame()
            : base(Constants.TargetWidth, Constants.TargetHeight, Constants.GameWidth, Constants.GameHeight, "NokiaJam")
        {
            Version = new Version(
                typeof(NJGame).Assembly.GetName().Version.Major,
                typeof(NJGame).Assembly.GetName().Version.Minor,
                typeof(NJGame).Assembly.GetName().Version.Build
            );
            //ShowFPS = Settings.Instance.ShowFPS;
            ClearColor = Color.Black;
#if __IOS__ || __TVOS__ || __ANDROID__// || XBOXONE
            Screen.Fullscreen = true;
#else
            //Engine.Instance.Screen.SnapScaleToPixels = false;
            Screen.SetScale(8);//Settings.Instance.WindowScale
            Screen.Fullscreen = Settings.Instance.Fullscreen;
            Graphics.SynchronizeWithVerticalRetrace = Settings.Instance.VSync;
            Graphics.ApplyChanges();
#endif
            Log.Message("NJ: " + Version);
#if DEBUG && !CONSOLE && !__MOBILE__
            Instance.IsMouseVisible = true;
#endif
        }

        public override void Load()
        {
            Log.Message("[nj] Begin");

            HudTarget = VirtualContent.CreateRenderTarget("hud-target", 1922, 1082);

            Log.Message("[nj] Bind Input");
            Controls.Load();
#if DEBUG && !CONSOLE && !__MOBILE__
            Input.Bind("debug", Keys.D);

            // Off music
            // Music.Toggle();
#endif
            //
            Log.Message("[nj] Load Misc Textures");
            GFX.LoadMisc();
            //
            Log.Message("[nj] Load Gui Textures");
            GFX.LoadGui();
            //
            Log.Message("[nj] Load Gameplay Textures");
            GFX.LoadGame();
            //
            Log.Message("[nj] Load Sprites");
            GFX.LoadSprites();

            Log.Message("[nj] Load Music");
            OST.Load();

            Log.Message("[nj] Load Sounds");
            SFX.Load();

            Log.Message("[nj] Load Fonts");
            Fonts.Load();

            Log.Message("[nj] Load Texts");
            Localization.SetLang("en");
            Texts.Load("en");

            Log.Message("[nj] Init Game Particles");
            ParticlePresets.Init();

#if DEBUG && !CONSOLE && !__MOBILE__
            DBG.Load();
#endif
            //SaveData.InitializeDebugMode(true);
            Log.Message("[nj] Init Scene");
            //Scene = new Title();
            //Scene = new Test();
            Scene = new MainMenu();
            //Scene = new Emulator();
            //Scene = new WorldSelect();

#if __MOBILE__
            VirtualGamepad.AddButton(320, 180, 100, 100, "up");//, GFX.Gui[ButtonUI.Id("cancel")]
            VirtualGamepad.SetActive("up", true);
#endif
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Input.Down(Keys.LeftAlt) && Input.Pressed(Keys.Enter))
            {
                Screen.Fullscreen = !Screen.Fullscreen;
                return;
            }
#if DEBUG && !CONSOLE && !__MOBILE__
            DBG.Update();
#endif
        }
    }
}
