using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class ParticlePresets
    {
        public static ParticlePreset Stars;
        public static ParticlePreset Water;
        public static ParticlePreset Piece;

        public static void Init()
        {
            Water = new ParticlePreset();
            Water.Color(Utils.HexToColor("f65ec6"), Utils.HexToColor("eb117d"));
            Water.Angle(1.470796f, 1.670796f);
            Water.Speed(10f, 20f);
            Water.Duration(1f, 2f);
            Water.Alpha(1f, 0.0f);
            Water.Friction = new Vector2(10f, 20f);
            Water.Size(2f, 0.0f);
            Water.Gravity = 0.0f;
            Water.Range = 8f;
            Water.Max = 5;

            Piece = new ParticlePreset();
            Piece.Angle(1.470796f, 1.670796f);
            Piece.Speed(10f, 20f);
            Piece.Duration(1f, 2f);
            Piece.Alpha(1f, 0.0f);
            Piece.Friction = new Vector2(10f, 20f);
            Piece.Size(2f, 0.0f);
            Piece.Gravity = 0.0f;
            Piece.Range = 8f;
            Piece.Max = 5;

            Stars = new ParticlePreset();
            Stars.Color(Constants.Dark, Constants.Light);
            Stars.Angle(Calc.PI, Calc.PI);
            Stars.Speed(100f, 300f);
            Stars.Duration(1f, 4f);
            Stars.Alpha(1f, 0f);
            Stars.Friction = new Vector2(10f, 20f);
            Stars.Size(2f, 1f);
            Stars.Gravity = 0.0f;
            Stars.Max = 100;
        }
    }
}