using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    /// <summary>
    /// Команды для консоли (TAB).
    /// Справку можно получить по команде "help"
    /// </summary>
    public static class Commands
    {
        [Command("godmode", "enable/disable god mode")]
        private static void CmdGodMode(bool on = true)
        {
            Engine.Commands.Log("God mode is " + (on ? "enabled" : "disabled"));
        }

        [Command("load", "load level (to/from)")]
        private static void CmdLoad(string to = "01_00", string from = "navigator")
        {
            SaveData.Instance.CurrentSession.ToLevel = to;
            SaveData.Instance.CurrentSession.FromLevel = from;
            Engine.Scene = new Loader(SaveData.Instance.CurrentSession/*, Player.Instance*/);
        }

        [Command("boss1", "go to boss1")]
        private static void CmdBoss1()
        {
            SaveData.Instance.CurrentSession.ToLevel = "01_02";
            SaveData.Instance.CurrentSession.FromLevel = "01_01";
            SaveData.Instance.CurrentSession.PlayerData?.Footwear.Add(Powerups.RocketBoots);
            Player.Instance.Position = new Vector2(434, 428);
            Engine.Scene = new Loader(SaveData.Instance.CurrentSession/*, Player.Instance*/);
        }
    }
}