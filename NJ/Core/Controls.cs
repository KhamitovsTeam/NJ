using KTEngine;
using Microsoft.Xna.Framework.Input;

namespace Chip
{
    public class Controls
    {
        public static void Load()
        {
            // Left
            Input.Bind("left", Keys.Left);
            Input.Bind("left", Buttons.DPadLeft);
            Input.Bind("left", new Input.Joystick(Input.Joystick.Types.LEFT, Input.Joystick.Directions.LEFT, 0.3f));

            // Right
            Input.Bind("right", Keys.Right);
            Input.Bind("right", Buttons.DPadRight);
            Input.Bind("right", new Input.Joystick(Input.Joystick.Types.LEFT, Input.Joystick.Directions.RIGHT, 0.3f));

            // Up
            Input.Bind("up", Keys.Up);
            Input.Bind("up", Buttons.DPadUp);
            Input.Bind("up", new Input.Joystick(Input.Joystick.Types.LEFT, Input.Joystick.Directions.UP, 0.2f));

            // Down
            Input.Bind("down", Keys.Down);
            Input.Bind("down", Buttons.DPadDown);
            Input.Bind("down", new Input.Joystick(Input.Joystick.Types.LEFT, Input.Joystick.Directions.DOWN, 0.25f));

            // Jump
            Input.Bind("jump", Keys.X, Keys.Space);
            Input.Bind("jump", Buttons.A);

            // Shoot
            Input.Bind("shoot", Keys.C, Keys.Z, Keys.LeftControl, Keys.RightControl);
            Input.Bind("shoot", Buttons.X, Buttons.RightTrigger);

            // Action
            Input.Bind("action", Keys.C);
            Input.Bind("action", Buttons.X);

            // Confirm
            Input.Bind("confirm", Keys.C, Keys.Enter);
            Input.Bind("confirm", Buttons.A);

            // Cancel
            Input.Bind("cancel", Keys.Escape);
            Input.Bind("cancel", Buttons.B);

            // Pause
            Input.Bind("pause", Keys.Escape);
            Input.Bind("pause", Buttons.Start, Buttons.BigButton);

            // Map
            Input.Bind("map", Keys.M);
            Input.Bind("map", Buttons.Back);
        }
    }
}