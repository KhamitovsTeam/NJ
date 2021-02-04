//#define DRAW_MINIMAP
#define DRAW_COINS

using KTEngine;
using Microsoft.Xna.Framework;
using System;

#if DRAW_MINIMAP
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Chip
{
    public class HUD
    {
        private readonly Graphic heart0Icon = new Graphic(GFX.Gui["heart0"], new Rectangle(0, 0, 8, 6));
        private readonly Graphic heart1Icon = new Graphic(GFX.Gui["heart1"], new Rectangle(0, 0, 8, 6));
        private readonly Graphic heart2Icon = new Graphic(GFX.Gui["heart2"], new Rectangle(0, 0, 8, 6));
        private readonly Graphic heart3Icon = new Graphic(GFX.Gui["heart3"], new Rectangle(0, 0, 8, 6));

        private readonly Graphic playerAvatar = new Graphic(GFX.Gui["player_avatar"], new Rectangle(0, 0, 8, 9));
        private readonly Vector2[] playerHeartsPos;

        private Graphic enemyAvatar;
        private Vector2[] enemyHeartsPos;

#if DRAW_COINS
        private readonly Graphic coinIcon = new Graphic(GFX.Gui["coin"], new Rectangle(0, 0, 7, 7));
        private readonly Text coinText;
        private bool isCoinsVisible = true;
#endif

#if DRAW_MINIMAP
        private const int MINI_MAP_WIDTH = 32;
        private const int MINI_MAP_HEIGHT = 17;
        
        private readonly int mapX;
        private readonly int mapY;
        private readonly Rectangle mapBounds;
        private bool isMapVisible = true;
        
        // Lists for render
        private readonly List<Entity> buildings;
        private readonly List<Entity> pipes;
        private readonly List<Entity> treadmills;
#endif

        private Vector2 offset;

        private readonly Level level;

        private readonly int playerW;
        private readonly int playerH;

        private bool display = true;
        private const int OFFSET = 5;

        private Enemy currentEnemy;
        private int enemyHeartContainersCount;
        private int enemyHeartContainersWidth;

        public HUD(Level level)
        {
            this.level = level;

#if DRAW_COINS
            coinText = new Text(Fonts.MainFont, "0", Vector2.Zero, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
#endif

            playerHeartsPos = new Vector2[3];
            for (var i = 0; i < playerHeartsPos.Length; i++)
                playerHeartsPos[i] = new Vector2(24f + (playerHeartsPos.Length - 1) + i * 9, 5f);

            var player = Player.Instance;

            playerW = player.MoveCollider.Width / 2;
            playerH = player.MoveCollider.Height / 2;

#if DRAW_MINIMAP
            buildings = level.GetEntities().Where(entity => entity.GetType() == typeof(Building)).ToList();
            pipes = level.GetEntities().Where(entity => entity.GetType() == typeof(Pipe)).ToList();
            treadmills = level.GetEntities().Where(entity => entity.GetType() == typeof(Treadmill)).ToList();
            
            mapX = Engine.Instance.Screen.Width - OFFSET - MINI_MAP_WIDTH;
            mapY = OFFSET;
            mapBounds = new Rectangle(mapX, mapY, MINI_MAP_WIDTH, MINI_MAP_HEIGHT);
#endif

            playerAvatar.X = 9f;
            playerAvatar.Y = 10f;
            playerAvatar.Scale = Vector2.One * 1.3f;
        }

        public void SetEnemy(Enemy enemy)
        {
            isCoinsVisible = false;
#if DRAW_MINIMAP
            isMapVisible = false;
#endif
            currentEnemy = enemy;
            enemyAvatar = new Graphic(GFX.Gui[currentEnemy.AvatarPath], new Rectangle(0, 0, 15, 15));
            enemyAvatar.X = Engine.Instance.Screen.Width - OFFSET - 16f;
            enemyAvatar.Y = 7f;
            enemyHeartContainersCount = currentEnemy.Health / 3;
            enemyHeartsPos = new Vector2[enemyHeartContainersCount];
            for (var i = 0; i < enemyHeartsPos.Length; i++)
                enemyHeartsPos[i] = new Vector2(Engine.Instance.Screen.Width - OFFSET - 21f - i * (enemyHeartContainersCount + 1) - 1, 5f);
            enemyHeartContainersWidth = enemyHeartContainersCount * 8 + (enemyHeartContainersCount - 1);
        }

        public void ClearEnemy()
        {
            currentEnemy = null;
            enemyHeartContainersCount = 0;
            enemyHeartsPos = null;
            isCoinsVisible = true;
#if DRAW_MINIMAP
            isMapVisible = true;
#endif
        }

        public void Update()
        {
            //             if (level != null)
            //             {
            // #if DRAW_BOSS
            //                 isCoinsVisible = false;
            //     #if DRAW_MINIMAP
            //                 isMapVisible = false;
            //     #endif
            // #else
            //                 isCoinsVisible = !level.InBossFight;
            //     #if DRAW_MINIMAP
            //                 isMapVisible = !level.InBossFight;
            //     #endif
            // #endif
            //             }
        }

        public void Render()
        {
            if (!display) return;
            offset = new Vector2(Engine.Instance.CurrentCamera.Render.X, Engine.Instance.CurrentCamera.Render.Y);
            Draw.SetOffset(offset);

            // Player frame
            Draw.HollowRect(OFFSET, OFFSET, 19f, 19f, 1f, Constants.DarkGreen);
            Draw.HollowRect(OFFSET - 1, OFFSET - 1, 19f, 19f, 1f, Constants.Background);
            Draw.Rect(OFFSET, OFFSET, 17f, 17f, Constants.NormalGreen);

            // Player Avatar
            playerAvatar.X = 9f;
            playerAvatar.Y = 10f;
            playerAvatar.Scale = Vector2.One * 1.3f;
            playerAvatar.Render();

            // Heart icons frame
            var width = playerHeartsPos.Length / 3 * 26;
            Draw.HollowRect(26f, 5f, width + 2f, 8f, 1f, Constants.DarkGreen);
            Draw.HollowRect(25f, 4f, width + 2f, 8f, 1f, Constants.Background);
            Draw.Rect(26f, 5f, width, 6f, Constants.LightGreen);

            for (var i = 0; i < Player.Instance.PlayerData.HeartContainers; i++)
            {
                var hearts = Player.Instance.PlayerData.Lives - 3 * i;
                if (hearts >= 3)
                {
                    heart3Icon.Scale.X = 1;
                    heart3Icon.RenderAt(playerHeartsPos[i]);
                }
                else
                {
                    switch (hearts)
                    {
                        case 2:
                            heart2Icon.Scale.X = 1;
                            heart2Icon.RenderAt(playerHeartsPos[i]);
                            break;
                        case 1:
                            heart1Icon.Scale.X = 1;
                            heart1Icon.RenderAt(playerHeartsPos[i]);
                            break;
                        default:
                            heart0Icon.Scale.X = 1;
                            heart0Icon.RenderAt(playerHeartsPos[i]);
                            break;
                    }
                }
            }

            if (currentEnemy != null)
            {
                // Enemy frame
                Draw.HollowRect(Engine.Instance.Screen.Width - OFFSET - 18f, OFFSET, 19f, 19f, 1f, Constants.DarkGreen);
                Draw.HollowRect(Engine.Instance.Screen.Width - OFFSET - 19f, OFFSET - 1, 19f, 19f, 1f, Constants.Background);
                Draw.Rect(Engine.Instance.Screen.Width - OFFSET - 18f, OFFSET, 17f, 17f, Constants.NormalGreen);

                enemyAvatar.X = Engine.Instance.Screen.Width - OFFSET - 16f;
                enemyAvatar.Y = 7f;
                enemyAvatar.Render();

                // x = screen.width - offset - avatar.width - containerCount * 8 - (containerCount - 1) 
                var x = Engine.Instance.Screen.Width - OFFSET - 24f - enemyHeartContainersCount * 8 -
                        (enemyHeartContainersCount - 1);
                Draw.HollowRect(x + 2f, 5f, enemyHeartContainersWidth + 2f, 8f, 1f, Constants.DarkGreen);
                Draw.HollowRect(x + 1f, 4f, enemyHeartContainersWidth + 2f, 8f, 1f, Constants.Background);
                Draw.Rect(x + 2f, 5f, enemyHeartContainersWidth, 6f, Constants.LightGreen);

                for (var i = 0; i < enemyHeartContainersCount; i++)
                {
                    var hearts = currentEnemy.Health - 3 * i;
                    if (hearts >= 3)
                    {
                        heart3Icon.Scale.X = -1;
                        heart3Icon.RenderAt(enemyHeartsPos[i]);
                    }
                    else
                    {
                        switch (hearts)
                        {
                            case 2:
                                heart2Icon.Scale.X = -1;
                                heart2Icon.RenderAt(enemyHeartsPos[i]);
                                break;
                            case 1:
                                heart1Icon.Scale.X = -1;
                                heart1Icon.RenderAt(enemyHeartsPos[i]);
                                break;
                            default:
                                heart0Icon.Scale.X = -1;
                                heart0Icon.RenderAt(enemyHeartsPos[i]);
                                break;
                        }
                    }
                }
            }

#if DRAW_COINS
            if (isCoinsVisible)
            {
                // Coins text
                coinText.DrawText = string.Concat(Player.Instance.PlayerData.Coins);
                coinText.X = 25f + 8f + offset.X;
                coinText.Y = (float)Math.Floor(offset.Y + 14f);

                while (coinText.DrawText.Length < 2)
                    coinText.DrawText = "0" + coinText.DrawText;

                // Coins frame
                Draw.HollowRect(26f, 15f, coinText.Width + 9f, 9f, 1f, Constants.DarkGreen);
                Draw.HollowRect(25f, 14f, coinText.Width + 9f, 9f, 1f, Constants.Background);
                Draw.Rect(26f, 15f, coinText.Width + 7f, 7f, Constants.LightGreen);

                coinIcon.X = 26f;
                coinIcon.Y = 15f;

                coinText.Render();
                coinIcon.Render();
            }
#endif

#if DRAW_MINIMAP
            if (isMapVisible)
            {
                // Mini map frame
                Draw.HollowRect(Engine.Instance.Screen.Width - OFFSET - MINI_MAP_WIDTH, OFFSET, MINI_MAP_WIDTH + 2f,
                    MINI_MAP_HEIGHT + 2f, 1f, Constants.DarkGreen);
                Draw.HollowRect(Engine.Instance.Screen.Width - (OFFSET + 1) - MINI_MAP_WIDTH, OFFSET - 1,
                    MINI_MAP_WIDTH + 2f, MINI_MAP_HEIGHT + 2f, 1f, Constants.Background);
                Draw.Rect(Engine.Instance.Screen.Width - OFFSET - MINI_MAP_WIDTH, OFFSET, MINI_MAP_WIDTH,
                    MINI_MAP_HEIGHT, Constants.DarkGreen);
                Draw.End();

                //Set up the spritebatch to draw using scissoring (for text cropping)
                Draw.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                    null, new RasterizerState() {ScissorTestEnable = true}, null, level.Camera.Matrix);

                //Copy the current scissor rect so we can restore it after
                var currentRect = Draw.SpriteBatch.GraphicsDevice.ScissorRectangle;

                //Set the current scissor rectangle
                Draw.SpriteBatch.GraphicsDevice.ScissorRectangle = mapBounds;

                var playerX = mapX - (int) Math.Floor((Player.Instance.Position.X - playerW) / 16f) +
                              MINI_MAP_WIDTH / 2;
                var playerY = mapY - (int) Math.Floor((Player.Instance.Position.Y - playerH) / 16f) +
                              MINI_MAP_HEIGHT / 2;

                var bounds = new Rectangle((int) Player.Instance.Position.X - Engine.Instance.Screen.Width / 2,
                    (int) Player.Instance.Position.Y - Engine.Instance.Screen.Height / 2,
                    Engine.Instance.Screen.Width, Engine.Instance.Screen.Height);
                RenderMap(playerX, playerY, bounds, 1);

                Draw.Rect(mapX + (float) Math.Floor(MINI_MAP_WIDTH / 2f),
                    mapY + (float) Math.Floor(MINI_MAP_HEIGHT / 2f), 1f, 1f, Constants.LightGreen);

                //Reset scissor rectangle to the saved value
                Draw.SpriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
            }

            //End the spritebatch
            Draw.SpriteBatch.End();
            Draw.Begin(level.Camera);
#endif
            Draw.ResetOffset();
        }

#if DRAW_MINIMAP
        private int MapWidth()
        {
            return level.Grid.Columns;
        }

        private int MapHeight()
        {
            return level.Grid.Rows;
        }
        
        // TODO: render map to bitmap
        private void RenderMap(float x, float y, Rectangle bounds, float scale)
        {
            Draw.Rect((float) Math.Floor(x), (float) Math.Floor(y), MapWidth() * scale, MapHeight() * scale, Constants.NormalGreen);
            for (var i = bounds.Y / 16 - MINI_MAP_HEIGHT / 2; i < bounds.Y / 16 + MINI_MAP_HEIGHT; i++)
            {
                for (var j = bounds.X / 16 - MINI_MAP_WIDTH / 2; j < bounds.X / 16 + MINI_MAP_WIDTH; j++)
                {
                    if (!level.Grid[j, i]) continue;
                    var wallX = x + j * scale;
                    var wallY = y + i * scale;
                    Draw.Rect(wallX, wallY, scale, scale, Constants.DarkGreen);
                }
            }

            // Render buildings
            foreach (var entity in buildings)
            {
                var entityX = (float) Math.Floor(x) + ((entity.Position.X - 8) / 16f) * scale;
                var entityY = (float) Math.Floor(y) + ((entity.Position.Y - 8) / 16f) * scale;
                Draw.Rect(entityX, entityY, scale, scale, Constants.DarkGreen);
            }

            // Render pipes
            foreach (var entity in pipes)
            {
                var entityX = (float) Math.Floor(x) + ((entity.Position.X - 8) / 16f) * scale;
                var entityY = (float) Math.Floor(y) + ((entity.Position.Y - 8) / 16f) * scale;
                Draw.Rect(entityX, entityY, scale, scale, Constants.DarkGreen);
            }

            // Render treadmills
            foreach (var entity in treadmills)
            {
                var entityX = (float) Math.Floor(x) + (entity.Position.X / 16f) * scale;
                var entityY = (float) Math.Floor(y) + (entity.Position.Y / 16f) * scale;
                Draw.Rect(entityX, entityY, (((Treadmill) entity).MoveCollider.Width / 16f) * scale, scale, Constants.DarkGreen);
            }
        }
#endif
    }
}