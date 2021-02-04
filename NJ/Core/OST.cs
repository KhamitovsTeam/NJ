using KTEngine;

namespace Chip
{
    /// <summary>
    /// Все треки используемые в игре
    /// </summary>
    public class OST
    {
        public static void Load()
        {
            Music.Root = "Music/";
            Music.Volume = Settings.Instance.MusicVolume / 10f;

            /*// Title theme
            Music.Add("main_theme", "CHIP - Main Theme");

            // Levels
            Music.Add("world_1", "CHIP - World 1");
            Music.Add("world_2", "CHIP - World 2");
            Music.Add("world_3", "CHIP - World 3");
            Music.Add("world_3_mirror", "CHIP - World 3 Mirror");
            Music.Add("world_5", "CHIP - World 5");
            Music.Add("world_6", "CHIP - World 6 Cats");

            // Bosses
            Music.Add("boss", "CHIP - Boss");
            Music.Add("final_boss", "CHIP - Final Boss");

            // Rocket
            Music.Add("rocket_loop", "CHIP - Rocket Loop");

            // Secret room
            Music.Add("secret_loop", "CHIP - Secret Loop");*/
        }
    }
}