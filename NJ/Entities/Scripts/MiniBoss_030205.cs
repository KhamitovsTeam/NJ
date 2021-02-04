using KTEngine;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Chip
{
    public class MiniBoss_030205 : Script
    {
        bool isTriggered = false;
        public override void OnTriggerEnter()
        {
            // move Blocker
            if (!isTriggered)
            {
                var list = Engine.Scene.GetEntities();
                foreach (var entity in list)
                {
                    if (entity.GetType() == typeof(Blocker))
                    {
                        entity.Y += 48f;
                    }
                }
                isTriggered = true;
                Music.Play("boss", true);
            }
        }

        public override void OnTriggerExit()
        {
           
        }
    }
}