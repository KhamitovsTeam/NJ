using System.Collections.Generic;
using KTEngine;
using Microsoft.Xna.Framework.Input;

namespace Chip
{
    public class DBG
    {
        public static void Load()
        {
            Engine.Commands.FunctionKeyActions[0] = LoadLevel12;
            Engine.Commands.FunctionKeyActions[1] = LoadLevel2;
            Engine.Commands.FunctionKeyActions[2] = LoadLevel13;
            Engine.Commands.FunctionKeyActions[3] = LoadLevel4;
            Engine.Commands.FunctionKeyActions[4] = LoadLevel5;
            Engine.Commands.FunctionKeyActions[5] = LoadLevel6;
            Engine.Commands.FunctionKeyActions[6] = LoadLevel7;
            Engine.Commands.FunctionKeyActions[7] = LoadLevel8;
            Engine.Commands.FunctionKeyActions[9] = LoadLevel9;
            Engine.Commands.FunctionKeyActions[10] = LoadLevel10;
            Engine.Commands.FunctionKeyActions[11] = LoadLevel11;

#if DEBUG && !CONSOLE && !__MOBILE__
            Engine.Commands.BuildCommandsList();
#endif
        }

        public static void Update()
        {
            if (Input.Pressed(Keys.H))
            {
                Player.Instance.PlayerData.Lives = 9;
            }

            if (Input.Pressed(Keys.S))
            {
                Music.Toggle();
            }

            if (Input.Pressed(Keys.W))
            {
                //Scene = new WorldSelect();
                OverlayDialog.Instance.DialogItem = new DialogItem(
                    "Hello! How are you?"
                );
                OverlayDialog.Instance.Show();
            }

            if (Input.Pressed(Keys.F12))
            {
                SaveData.TryDelete(4);
                SaveData.Instance.CurrentSession.PlayerData = new PlayerData();
                SaveData.Instance.CurrentSession.LevelsData = new List<LevelData>();
                SaveData.Instance.CurrentSession = new Session();
            }
        }

        private static void LoadLevel1()
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "01_01";
                SaveData.Instance.CurrentSession.FromLevel = "01_00";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel2()
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "01_02";
                SaveData.Instance.CurrentSession.FromLevel = "01_01";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel3()
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "01_03";
                SaveData.Instance.CurrentSession.FromLevel = "01_02";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel4()
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "01_04";
                SaveData.Instance.CurrentSession.FromLevel = "01_03";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel5()
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "01_05";
                SaveData.Instance.CurrentSession.FromLevel = "01_04";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel6()
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "02_01";
                SaveData.Instance.CurrentSession.FromLevel = "02_00";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel7()
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "02_02";
                SaveData.Instance.CurrentSession.FromLevel = "02_01";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel8()
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "02_03";
                SaveData.Instance.CurrentSession.FromLevel = "02_02";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel9()
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "02_04";
                SaveData.Instance.CurrentSession.FromLevel = "02_01";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel10() //world3 main
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "03_01";
                SaveData.Instance.CurrentSession.FromLevel = "03_05";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel11() //world3 right
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "03_02";
                SaveData.Instance.CurrentSession.FromLevel = "03_01";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel12() //world5
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "05_00";
                SaveData.Instance.CurrentSession.FromLevel = "05_01";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }

        private static void LoadLevel13() //world6
        {
            if (SaveData.Instance != null && SaveData.Instance.CurrentSession != null)
            {
                SaveData.Instance.CurrentSession.ToLevel = "06_01";
                SaveData.Instance.CurrentSession.FromLevel = "06_00";
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
                Engine.Scene = new Loader(null);
        }
    }
}