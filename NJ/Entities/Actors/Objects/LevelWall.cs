using KTEngine;

namespace Chip
{
    public class LevelWall : Entity
    {
        public LevelWall(int wallX, int wallY, int x, int y, int width, int height)
            : base(wallX, wallY)
        {
            Add(new Hitbox(x, y, width, height)).Tag((int)Tags.Solid);
        }
    }
}