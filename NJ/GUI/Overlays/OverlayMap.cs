using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chip
{
    public class OverlayMap : OverlayComponent
    {
        public static OverlayMap Instance;

        #region Constants

        private const float BottomPadding = 28;
        private const float EndPadding = 80;
        private const float StartPadding = 80;
        private const float TopPadding = 54;

        #endregion

        private readonly Text mapTitle = new Text(Fonts.MainFont, Texts.MainText["map"], Vector2.One, Constants.Background);
        private readonly float closeButtonPadding;
        private readonly float mapScale;
        private readonly float mapX;
        private readonly float mapY;
        private readonly int playerW;
        private readonly int playerH;

        private int coins;  // how many coins was selected
        private int kittens;  //  how many kittens was selected

        // Lists for render
        private readonly List<Entity> buildings;
        private readonly List<Entity> pipes;
        private readonly List<Entity> treadmils;

        // imagine selected items
        private readonly Text coinsText = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text kittensText = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);

        public OverlayMap(Level level)
            : base(level)
        {
            Instance = this;

            closeButtonPadding = Engine.Instance.Screen.Width - ButtonUI.Width(Texts.MainText["close"], "cancel") / 2f - 8f;

            Player player = Player.Instance;

            playerW = player.MoveCollider.Width / 2;
            playerH = player.MoveCollider.Height / 2;

            float mapScaleW = (Engine.Instance.Screen.Width - EndPadding - StartPadding) / MapWidth();
            float mapScaleH = (Engine.Instance.Screen.Height - BottomPadding - TopPadding) / MapHeight();
            mapScale = Math.Min(mapScaleH, mapScaleW);

            mapX = Engine.Instance.Screen.Width - EndPadding - StartPadding - (MapWidth() * mapScale / 2f);
            mapY = Engine.Instance.Screen.Height - BottomPadding - TopPadding - (MapHeight() * mapScale / 2f);

            buildings = Level.GetEntities().Where(entity => entity.GetType() == typeof(Building)).ToList();
            pipes = Level.GetEntities().Where(entity => entity.GetType() == typeof(Pipe)).ToList();
            treadmils = Level.GetEntities().Where(entity => entity.GetType() == typeof(Treadmill)).ToList();
        }

        public override void Show()
        {
            base.Show();
            if (Player.Instance == null) return;
            coins = Player.Instance.PlayerData.Coins;
            kittens = Player.Instance.PlayerData.Kittens;
        }

        public override void Hide()
        {
            base.Hide();
            Level.IsGamePaused = false;
        }

        public override void Update()
        {
            base.Update();
            if (Input.Pressed("cancel"))
            {
                Hide();
            }
        }

        public override void Render()
        {
            Draw.SetOffset(Engine.Instance.CurrentCamera.Render);

            base.Render();

            RenderMap(mapX, mapY, mapScale);

            mapTitle.DrawText = Calc.WrapText(Fonts.MainFont, Texts.MainText["pause_map"], Engine.Instance.Screen.Width);
            mapTitle.Color = Constants.Background;
            mapTitle.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width / 2f;
            mapTitle.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 30 + mapTitle.Height;
            mapTitle.Alpha = Alpha;

            mapTitle.Render();

            ButtonUI.Render(new Vector2(closeButtonPadding, Engine.Instance.Screen.Height - 13f), Texts.MainText["close"], Constants.Background, "cancel", 1f);

            //Renderer.HollowRect(START_PADDING, TOP_PADDING, Engine.Instance.Screen.Width - END_PADDING - START_PADDING, Engine.Instance.Screen.Height - BOTTOM_PADDING - TOP_PADDING, 1f, Color.Red);

            if (kittens == 0)
                kittensText.DrawText = "000";
            else
            {
                kittensText.DrawText = kittens.ToString();
                while (kittensText.DrawText.Length < 3)
                    kittensText.DrawText = "0" + kittensText.DrawText;
            }
            kittensText.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            kittensText.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 35;
            kittensText.Render();

            if (coins == 0)
                coinsText.DrawText = "000";
            else
            {
                coinsText.DrawText = coins.ToString();
                while (coinsText.DrawText.Length < 3)
                    coinsText.DrawText = "0" + coinsText.DrawText;
            }
            coinsText.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            coinsText.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 50;
            coinsText.Render();

            Draw.ResetOffset();
        }

        private int MapWidth()
        {
            return Level.Grid.Columns;
        }

        private int MapHeight()
        {
            return Level.Grid.Rows;
        }

        private void RenderMap(float x, float y, float scale)
        {
            Draw.Rect((float)Math.Floor(x), (float)Math.Floor(y), MapWidth() * scale, MapHeight() * scale, Constants.NormalGreen);
            for (int i = 0; i < Level.Grid.Rows; i++)
            {
                for (int j = 0; j < Level.Grid.Columns; j++)
                {
                    if (!Level.Grid[j, i]) continue;
                    float wallX = x + j * scale;
                    float wallY = y + i * scale;
                    Draw.Rect(wallX, wallY, scale, scale, Constants.DarkGreen);
                }
            }

            // Render buildings
            foreach (Entity entity in buildings)
            {
                float entityX = (float)Math.Floor(x) + ((entity.Position.X - 8) / 16f) * scale;
                float entityY = (float)Math.Floor(y) + ((entity.Position.Y - 8) / 16f) * scale;
                Draw.Rect(entityX, entityY, scale, scale, Constants.DarkGreen);
            }

            // Render pipes
            foreach (Entity entity in pipes)
            {
                float entityX = (float)Math.Floor(x) + ((entity.Position.X - 8) / 16f) * scale;
                float entityY = (float)Math.Floor(y) + ((entity.Position.Y - 8) / 16f) * scale;
                Draw.Rect(entityX, entityY, scale, scale, Constants.DarkGreen);
            }

            // Render treadmils
            foreach (Entity entity in treadmils)
            {
                float entityX = (float)Math.Floor(x) + (entity.Position.X / 16f) * scale;
                float entityY = (float)Math.Floor(y) + (entity.Position.Y / 16f) * scale;
                Draw.Rect(entityX, entityY, (((Treadmill)entity).MoveCollider.Width / 16f) * scale, scale, Constants.DarkGreen);
            }

            // Render player
            float playerX = (x) + (float)Math.Floor((Player.Instance.Position.X - playerW) / 16f) * scale;
            float playerY = (y) + (float)Math.Floor((Player.Instance.Position.Y - playerH) / 16f) * scale;
            Draw.Rect(playerX, playerY, scale, scale, Constants.LightGreen);

            //Draw.Rect((float) Math.Floor(x), (float) Math.Floor(y), MapWidth() * scale, MapHeight() * scale, Color.White, 0.5f);
        }
    }
}