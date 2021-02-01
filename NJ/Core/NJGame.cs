using KTEngine;

namespace NJ
{
    public class NJGame : Engine
    {
        public NJGame()
            : base(84, 48, 84 * 5, 48 * 5, "NokiaJam", false)
        {
            Scene = new Level();
        }
    }
}