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