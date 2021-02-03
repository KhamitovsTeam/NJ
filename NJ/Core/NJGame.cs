using KTEngine;

namespace NJ
{
    public class NJGame : Engine
    {
        public NJGame()
            : base(Constants.TargetWidth, Constants.TargetHeight, Constants.GameWidth, Constants.GameHeight, "NokiaJam", false)
        {
            Screen.SetScale(6);
            Screen.Fullscreen = false;
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.ApplyChanges();

            IsMouseVisible = true;
            ExitOnEscapeKeypress = true;
            
            Controls.Load();
                
            Scene = new Level();
        }
    }
}