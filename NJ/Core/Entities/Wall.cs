using KTEngine;
using Microsoft.Xna.Framework;

namespace NJ
{
    public class Wall : Entity
    {
        public Level Level;
        public Grid Grid;

        public Wall(int x, int y, Grid grid, Level level = null)
            : base(x, y)
        {
            Level = level;
            Grid = Add(grid);
            Grid.Tag((int) Tags.Solid);
            Depth = 5;
        }
    }

}