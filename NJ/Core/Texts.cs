using KTEngine;

namespace Chip
{
    public class Texts
    {
        public static TextData MainText;

        public static void Load(string lang = "ru")
        {
            MainText = new TextData(lang);
        }
    }
}