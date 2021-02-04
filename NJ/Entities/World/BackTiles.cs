using KTEngine;
using System.Collections;

namespace Chip
{
    public class BackTiles : Entity
    {
        public int TilePadding = 0;
        public Level Level;
        public Grid Grid;
        public string TexturePath;

        public BackTiles(string texturePath, Grid grid, Level level = null)
            : base(0, 0)
        {
            Level = level;
            Grid = Add(grid);
            Grid.Tag((int)Tags.Sceneries);
            Depth = 6;
        }

        public IEnumerator Generate()
        {
            yield return GenerateBuilding();
        }

        private IEnumerator GenerateBuilding()
        {
            Tilemap tiles = Add(new Tilemap(GFX.Game["sceneries/building_back"], 16, 16, Grid.Columns, Grid.Rows));
            for (int i = 0; i < Grid.Columns; ++i)
            {
                for (int j = 0; j < Grid.Rows; ++j)
                {
                    int depth = GetDepth(i, j);
                    if (Check(i, j))
                    {
                        if (depth == 0) //Верхний слой задника
                        {
                            tiles.AddRange(i, j, new int[4] { 0, 1, 2, 3 }, 0.0f);
                        }
                        else if (depth == 1)
                        {
                            tiles.AddRange(i, j, new int[4] { 4, 5, 6, 7 }, 0.0f);
                        }
                    }
                }
                yield return 0f;
            }
            yield return 0f;
        }

        private bool Check(int x, int y)
        {
            return Grid[x, y];
        }

        private int GetDepth(int x, int y)
        {
            var depth = 0;
            if (!Check(x, y - 1))
            {
                depth = 0;
            }
            if (Check(x, y - 1))
            {
                depth = 1;
            }
            return depth;
        }
    }
}