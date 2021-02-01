using KTEngine;
using Microsoft.Xna.Framework;

namespace NJ
{
    public class Level : Scene
    {
        public Level()
        {
            Add(new Player());
        }
    }
}