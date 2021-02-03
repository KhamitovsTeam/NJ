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

        public override void Render()
        {
            base.Render();
            for (int x = 0; x < Grid.Columns; ++x)
            {
                for (int y = 0; y < Grid.Rows; ++y)
                {
                    if (Check(x, y))
                    {
                        Draw.Rect(x * 4, y * 4, 4, 4, Color.Aqua);
                    }       
                }
            }
        }
        
        private bool Check(int x, int y)
        {
            return Grid[x, y];
        }
    }
}