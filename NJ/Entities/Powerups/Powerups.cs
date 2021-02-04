using KTEngine;
using System.Collections.Generic;

namespace Chip
{
    public enum PowerupType
    {
        Footwear = 0,
        Body = 1,
        Armwear = 2
    }

    public class Powerups
    {
        public static int Count = 0;

        public static Dictionary<PowerupType, List<Powerup>> List = new Dictionary<PowerupType, List<Powerup>>();

        // Armwear
        //value="claws"
        public static Powerup Claws = Add(new Powerup(Texts.MainText["powerup_name_claws"], Texts.MainText["powerup_description_claws"], PowerupType.Armwear, new Graphic(GFX.Gui["icon_claws"])));
        //value="wallbreaker_gun"
        public static Powerup WallBreakerGun = Add(new Powerup(Texts.MainText["powerup_name_wallbreaker_gun"], Texts.MainText["powerup_description_wallbreaker_gun"], PowerupType.Armwear, new Graphic(GFX.Gui["icon_claws"])));//icon_wallbreaker_gun
        
        // Footwear
        //value="rocket_boots"
        public static Powerup RocketBoots = Add(new Powerup(Texts.MainText["powerup_name_rocket_boots"], Texts.MainText["powerup_description_rocket_boots"], PowerupType.Footwear, new Graphic(GFX.Gui["icon_doublejump"])));
        //value="spiked_boots"
        public static Powerup SpikedBoots = Add(new Powerup(Texts.MainText["powerup_name_spiked_boots"], Texts.MainText["powerup_description_spiked_boots"], PowerupType.Footwear, new Graphic(GFX.Gui["icon_claws"])));
        //value="levitation_boots"
        public static Powerup LevitationBoots = Add(new Powerup(Texts.MainText["powerup_name_levitation_boots"], Texts.MainText["powerup_description_levitation_boots"], PowerupType.Footwear, new Graphic(GFX.Gui["icon_claws"])));
        //value="jetpack"
        public static Powerup Jetpack = Add(new Powerup(Texts.MainText["powerup_name_jetpack"], Texts.MainText["powerup_description_jetpack"], PowerupType.Footwear, new Graphic(GFX.Gui["icon_claws"])));

        // Body
        public static Powerup EmptyBody = new Powerup(Texts.MainText["empty"], "...", PowerupType.Body);

        private static Powerup Add(Powerup powerup)
        {
            if (!List.ContainsKey(powerup.Type))
                List[powerup.Type] = new List<Powerup>();
            List[powerup.Type].Add(powerup);
            return powerup;
        }
    }
}