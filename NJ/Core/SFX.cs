using KTEngine;

namespace Chip
{
    /// <summary>
    /// Все звуки используемые в игре
    /// </summary>
    public static class SFX
    {
        public static void Load()
        {
            Sounds.Root = "SFX";
            Sounds.Volume = Settings.Instance.SFXVolume / 10f;

            // Player sounds
            /*Sounds.Add("jump", "jump");
            Sounds.Add("down", "down");
            Sounds.Add("shoot", "shoot");
            Sounds.Add("player_death", "player_death");
            Sounds.Add("player_damage", "damage_a");

            // Menu sounds
            Sounds.Add("menu_click", "menu_click");
            Sounds.Add("menu_choose", "menu_choose");

            // Collectable stuff sounds
            Sounds.Add("star", "star");
            Sounds.Add("cat", "cat");
            Sounds.AddSet("coin", "coin_a", "coin_b", "coin_c");

            // Open chest
            Sounds.Add("chest", "chest");

            // Elevator activate
            Sounds.Add("elevator", "elevator");

            // Rock destroy sound
            Sounds.Add("destroy", "destroy");

            // Enemy sounds
            Sounds.AddSet("enemy_death", "enemy_death_a", "enemy_death_b");
            Sounds.Add("enemy_damage", "damage_b");
            
            // Laser
            Sounds.Add("laser", "laser");
            
            // Window
            Sounds.Add("window", "window");*/
        }

        public static void Play(string name, bool shuffle = true) {
            if (!Sounds.Contains(name))
                return;
            Sounds.Play(name, shuffle);
        }
    }
}