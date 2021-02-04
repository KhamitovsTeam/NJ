using KTEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chip
{
    public class OverlayWorldSelector : OverlayComponent
    {
        public static OverlayWorldSelector Instance;

        private int coins;  // how many coins was selected
        private int kittens;  //  how many kittens was selected

        private readonly List<World> worldList = new List<World>();

        private readonly float cancelButtonPadding;
        private int selectedIndex;
        private bool isFirstRun = true;

        private readonly Vector2 centerPosition;
        private readonly Vector2 middleLeftPosition;
        private readonly Vector2 middleRightPosition;
        private readonly Vector2 backLeftPosition;
        private readonly Vector2 backRightPosition;

        private Text worldName;
        private Vector2 worldNamePos;

        // imagine selected items
        private readonly Text coinstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text kittenstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);

        public OverlayWorldSelector(Level level)
            : base(level)
        {
            Instance = this;

            cancelButtonPadding = Engine.Instance.Screen.Width - ButtonUI.Width(Texts.MainText["cancel"], "cancel") / 2f - 4f;

            centerPosition = new Vector2(Engine.Instance.Screen.Width / 2f, 120f);
            middleLeftPosition = new Vector2(100f, 85f);
            middleRightPosition = new Vector2(Engine.Instance.Screen.Width - 100f, 85f);
            backLeftPosition = new Vector2(130f, 50f);
            backRightPosition = new Vector2(Engine.Instance.Screen.Width - 130f, 50f);

            // [!] Название мира используется для спрайтов, если мир переименовывается, то
            // нужно переименовать его в файле Content/Graphics/Sprites.xml
            worldList.Add(new World("World1", "01_00")
            {
                X = centerPosition.X,
                Y = centerPosition.Y
            });

            worldList.Add(new World("World5", "05_00")
            {
                X = middleLeftPosition.X,
                Y = middleLeftPosition.Y
            });

            worldList.Add(new World("World4", "04_00")
            {
                X = backLeftPosition.X,
                Y = backLeftPosition.Y
            });

            worldList.Add(new World("World3", "03_00")
            {
                X = backRightPosition.X,
                Y = backRightPosition.Y
            });

            worldList.Add(new World("World2", "02_00")
            {
                X = middleRightPosition.X,
                Y = middleRightPosition.Y
            });

            UpdateName(worldList[selectedIndex].Name);
        }

        public override void Show()
        {
            base.Show();
            isFirstRun = true;
            Level.IsGamePaused = true;
            if (Player.Instance == null) return;
            coins = Player.Instance.PlayerData.Coins;
            kittens = Player.Instance.PlayerData.Kittens;
        }

        public override void Hide()
        {
            base.Hide();
            Level.IsGamePaused = false;
            Player.Instance.IgnoreAction = false;
        }

        public override void Update()
        {
            if (isFirstRun)
            {
                isFirstRun = false;
                return;
            }

            if (Input.Pressed("right"))
            {
                MoveRight();
            }
            else if (Input.Pressed("left"))
            {
                MoveLeft();
            }
            else if (Input.Pressed("confirm"))
            {
                RunLevel();
            }
            else if (Input.Pressed("cancel"))
            {
                Hide();
            }

            foreach (var world in worldList)
            {
                world.Update();
            }

            base.Update();
        }

        public override void Render()
        {
            Draw.SetOffset(Engine.Instance.CurrentCamera.Render);
            base.Render();

            foreach (var world in worldList)
            {
                world.Render();
            }

            worldName?.RenderAt(worldNamePos);

            ButtonUI.Render(new Vector2(cancelButtonPadding, Engine.Instance.Screen.Height - 32 * 0.4f), Texts.MainText["cancel"], Constants.Background, "cancel", 1f);

            if (kittens == 0)
                kittenstext.DrawText = "000";
            else
            {
                kittenstext.DrawText = kittens.ToString();
                while (kittenstext.DrawText.Length < 3)
                    kittenstext.DrawText = "0" + kittenstext.DrawText;
            }
            kittenstext.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            kittenstext.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 35;
            kittenstext.Render();

            if (coins == 0)
                coinstext.DrawText = "000";
            else
            {
                coinstext.DrawText = coins.ToString();
                while (coinstext.DrawText.Length < 3)
                    coinstext.DrawText = "0" + coinstext.DrawText;
            }
            coinstext.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            coinstext.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 50;
            coinstext.Render();

            Draw.ResetOffset();
        }

        private void UpdateName(string name)
        {
            worldName = new Text(Fonts.MainFont, name, Vector2.Zero, Constants.LightGreen);
            worldNamePos = new Vector2(Engine.Instance.Screen.Width / 2f, 154f);
        }

        private void MoveRight()
        {
            switch (selectedIndex)
            {
                case 0:
                    worldList[0].Move(worldList[0].Position, middleLeftPosition);
                    worldList[1].Move(worldList[1].Position, backLeftPosition);
                    worldList[2].Move(worldList[2].Position, backRightPosition);
                    worldList[3].Move(worldList[3].Position, middleRightPosition);
                    worldList[4].Move(worldList[4].Position, centerPosition);
                    break;
                case 1:
                    worldList[0].Move(worldList[0].Position, backLeftPosition);
                    worldList[1].Move(worldList[1].Position, backRightPosition);
                    worldList[2].Move(worldList[2].Position, middleRightPosition);
                    worldList[3].Move(worldList[3].Position, centerPosition);
                    worldList[4].Move(worldList[4].Position, middleLeftPosition);
                    break;
                case 2:
                    worldList[0].Move(worldList[0].Position, backRightPosition);
                    worldList[1].Move(worldList[1].Position, middleRightPosition);
                    worldList[2].Move(worldList[2].Position, centerPosition);
                    worldList[3].Move(worldList[3].Position, middleLeftPosition);
                    worldList[4].Move(worldList[4].Position, backLeftPosition);
                    break;
                case 3:
                    worldList[0].Move(worldList[0].Position, middleRightPosition);
                    worldList[1].Move(worldList[1].Position, centerPosition);
                    worldList[2].Move(worldList[2].Position, middleLeftPosition);
                    worldList[3].Move(worldList[3].Position, backLeftPosition);
                    worldList[4].Move(worldList[4].Position, backRightPosition);
                    break;
                case 4:
                    worldList[0].Move(worldList[0].Position, centerPosition);
                    worldList[1].Move(worldList[1].Position, middleLeftPosition);
                    worldList[2].Move(worldList[2].Position, backLeftPosition);
                    worldList[3].Move(worldList[3].Position, backRightPosition);
                    worldList[4].Move(worldList[4].Position, middleRightPosition);
                    break;
            }
            selectedIndex++;
            if (selectedIndex == 5)
                selectedIndex = 0;
            UpdateName(worldList[selectedIndex == 0 ? 0 : 5 - selectedIndex].Name);
        }

        private void MoveLeft()
        {
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = 4;
            UpdateName(worldList[selectedIndex == 0 ? 0 : 5 - selectedIndex].Name);
            switch (selectedIndex)
            {
                case 0:
                    worldList[0].Move(worldList[0].Position, centerPosition);
                    worldList[1].Move(worldList[1].Position, middleLeftPosition);
                    worldList[2].Move(worldList[2].Position, backLeftPosition);
                    worldList[3].Move(worldList[3].Position, backRightPosition);
                    worldList[4].Move(worldList[4].Position, middleRightPosition);
                    break;
                case 1:
                    worldList[0].Move(worldList[0].Position, middleLeftPosition);
                    worldList[1].Move(worldList[1].Position, backLeftPosition);
                    worldList[2].Move(worldList[2].Position, backRightPosition);
                    worldList[3].Move(worldList[3].Position, middleRightPosition);
                    worldList[4].Move(worldList[4].Position, centerPosition);
                    break;
                case 2:
                    worldList[0].Move(worldList[0].Position, backLeftPosition);
                    worldList[1].Move(worldList[1].Position, backRightPosition);
                    worldList[2].Move(worldList[2].Position, middleRightPosition);
                    worldList[3].Move(worldList[3].Position, centerPosition);
                    worldList[4].Move(worldList[4].Position, middleLeftPosition);
                    break;
                case 3:
                    worldList[0].Move(worldList[0].Position, backRightPosition);
                    worldList[1].Move(worldList[1].Position, middleRightPosition);
                    worldList[2].Move(worldList[2].Position, centerPosition);
                    worldList[3].Move(worldList[3].Position, middleLeftPosition);
                    worldList[4].Move(worldList[4].Position, backLeftPosition);
                    break;
                case 4:
                    worldList[0].Move(worldList[0].Position, middleRightPosition);
                    worldList[1].Move(worldList[1].Position, centerPosition);
                    worldList[2].Move(worldList[2].Position, middleLeftPosition);
                    worldList[3].Move(worldList[3].Position, backLeftPosition);
                    worldList[4].Move(worldList[4].Position, backRightPosition);
                    break;
            }
        }

        private void RunLevel()
        {
            if (SaveData.Instance == null || SaveData.Instance.CurrentSession == null) return;
            string toLevel = worldList[selectedIndex == 0 ? 0 : 5 - selectedIndex].File;
            string fromLevel = "";
            switch (toLevel)
            {
                case "01_00":
                    fromLevel = "navigator";
                    break;
                case "02_00":
                    fromLevel = "navigator";
                    break;
                case "03_00":
                    fromLevel = "navigator";
                    break;
                case "04_00":
                    fromLevel = "navigator";
                    break;
                case "05_00":
                    fromLevel = "navigator";
                    break;
            }

            SaveData.Instance.CurrentSession.FromLevel = fromLevel;
            SaveData.Instance.CurrentSession.ToLevel = toLevel;
            Engine.Scene = new Loader(SaveData.Instance.CurrentSession/*, Player.Instance*/);
        }
    }
}