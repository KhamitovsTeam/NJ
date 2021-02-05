using KTEngine;
using System;
using System.Collections;

namespace Chip
{
    public class Wall : Entity
    {
        //        public Tilemap Tilemap;
        //        public Grid Grid;
        //
        //        public Wall(int x, int y, string texturePath, Grid grid) 
        //            : base(x, y)
        //        {
        //            Tilemap = Add(Tilemap.Autotiles(GFX.Game[texturePath + "tiles"], grid, GFX.Tiles));
        //            Grid = Add(grid);
        //            Grid.Tag((int) Tags.Solid);
        //            Depth = 5;
        //        }

        public Level Level;
        public Grid Grid;
        public string TexturePath;

        private bool useRotation = false;

        public Wall(string texturePath, Grid grid, Level level = null)
            : base(0, 0)
        {
            Level = level;
            Grid = Add(grid);
            Grid.Tag((int)Tags.Solid);
            TexturePath = texturePath;
            Depth = -5;
        }

        public IEnumerator Generate()
        {
            yield return GenerateWalls();
        }

        private IEnumerator GenerateWalls()
        {
            Tilemap tiles = Add(new Tilemap(GFX.Game[TexturePath + "tiles"], 4, 4, Grid.Columns, Grid.Rows));
            for (int i = 0; i < Grid.Columns; ++i)
            {
                for (int j = 0; j < Grid.Rows; ++j)
                {
                    int depth = GetDepth(i, j);
                    if (Check(i, j))
                    {
                        if (depth == 0)
                        {
                            bool left = Check(i - 1, j);
                            bool top = Check(i, j - 1);
                            bool right = Check(i + 1, j);
                            bool bottom = Check(i, j + 1);
                            if (!top && left && right && bottom) // 1-st row in tile sprite
                            {
                                tiles.AddRange(i, j, new int[4] { 0, 1, 2, 3 }, 0f);
                            }
                            else if (!left && right && (top && bottom)) // 2d row in tile sprite
                            {
                                tiles.AddRange(i, j, new int[4] { 4, 5, 6, 7 }, useRotation ? 3f * Calc.PI / 2f : 0f);
                            }
                            else if (!right && left && (top && bottom)) // 3d row in tile sprite
                            {
                                tiles.AddRange(i, j, new int[4] { 8, 9, 10, 11 }, useRotation ? Calc.PI / 2f : 0f);
                            }
                            else if (!bottom && left && (right && top)) // 4th row in tile sprite
                            {
                                tiles.AddRange(i, j, new int[4] { 12, 13, 14, 15 }, useRotation ? Calc.PI : 0f);
                            }
                            else
                            {
                                // Углы
                                if (bottom && !left && right && !top) // 5th row in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[4] { 16, 17, 18, 19 }, 0f);
                                }
                                else if (bottom && !right && left && !top) // 6th row in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[4] { 20, 21, 22, 23 }, 0f);
                                }
                                else if (!bottom && !left && right && top) // 7th row in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[4] { 24, 25, 26, 27 }, useRotation ? 3f * Calc.PI / 2f : 0f);
                                }
                                else if (!bottom && !right && left && top) // 8th row in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[4] { 28, 29, 30, 31 }, useRotation ? Calc.PI / 2f : 0f);
                                }
                                else if (!bottom && !right && left && !top) // 9th row in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[4] { 32, 33, 34, 35 }, 0f);
                                }
                                else if (!bottom && right && !left && !top) // 10th row in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[4] { 36, 37, 38, 39 }, useRotation ? Calc.PI : 0f);
                                }
                                else if (bottom && !right && !left && !top) // 11th row in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[4] { 40, 41, 42, 43 }, useRotation ? Calc.PI : 0f);
                                }
                                else if (!bottom && !right && !left && top) // 12th row in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[4] { 44, 45, 46, 47 }, useRotation ? Calc.PI : 0f);
                                }
                                else if (!bottom && right && left && !top) // 1&2 columns in 13th row  in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[2] { 48, 49 }, useRotation ? Calc.PI : 0f);
                                }
                                else if (bottom && !right && !left && top) // 3&4 columns in 13th row  in tile sprite
                                {
                                    tiles.AddRange(i, j, new int[2] { 50, 51 }, useRotation ? Calc.PI : 0f);
                                }
                                else if (!bottom && !right && !left && !top) // 1&2 columns in 14th row
                                {
                                    tiles.AddRange(i, j, new int[2] { 52, 53 }, useRotation ? Calc.PI : 0f);
                                }
                                else
                                {
                                    tiles.AddRange(i, j, new int[1] { 1 }, 0f);
                                }
                            }
                        }
                        else if (depth == 1)
                        {
                            bool left = Check(i - 2, j);
                            bool bottom = Check(i, j - 2);
                            bool right = Check(i + 2, j);
                            bool top = Check(i, j + 2);
                            if (!bottom && left && (right && top))
                            {
                                tiles.AddRange(i, j, new int[4] { 56, 57, 58, 59 }, 0f); // 15th row  in tile sprite
                            }
                            else if (!left && right && (bottom && top))
                            {
                                tiles.AddRange(i, j, new int[4] { 56, 57, 58, 59 }, useRotation ? 3f * Calc.PI / 2f : 0f);
                            }
                            else if (!right && left && (bottom && top))
                            {
                                tiles.AddRange(i, j, new int[4] { 56, 57, 58, 59 }, useRotation ? Calc.PI / 2f : 0f);
                            }
                            else if (!top && left && (right && bottom))
                            {
                                tiles.AddRange(i, j, new int[4] { 56, 57, 58, 59 }, useRotation ? Calc.PI : 0f);
                            }
                            else
                            {
                                tiles.AddRange(i, j, new int[4] { 56, 57, 58, 59 }, useRotation ? Calc.PI / 2f : 0f);
                            }
                        }
                        else if (depth == 2)
                        {
                            tiles.AddRange(i, j, new int[4] { 60, 61, 62, 63 }, 0f);// 16th row  in tile sprite
                        }
                        else if (depth == 3 || Utils.Range(0, 4) == 0)
                        {
                            tiles.AddRange(i, j, new int[4] { 64, 65, 66, 67 }, 0f);// 17th row  in tile sprite
                        }
                        else
                        {
                            tiles.AddRange(i, j, new int[4] { 60, 61, 62, 63 }, 0f);
                        }
                    }
                    yield return 0f;
                }
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
            for (var i = 1; i < 5; ++i)
            {
                for (var j = -i; j <= i; ++j)
                {
                    var num = i - Math.Abs(j);
                    if (!Check(x + j, y - num) || num != 0 && !Check(x + j, y + num))
                        return depth;
                }
                depth = i;
            }
            return depth;
        }
    }
}