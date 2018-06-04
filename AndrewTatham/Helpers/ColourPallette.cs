using System.Drawing;

namespace AndrewTatham.Helpers
{
    public class ColourPalette
    {
        public static readonly Color BlipColour = Color.LightGreen;
        public static readonly Pen BlipPen = Pens.LightGreen;

        public static readonly Color EnemyColour = Color.LightGreen;
        public static readonly Pen EnemyPen = Pens.LightGreen;

        public static readonly Color AimColour = Color.Red;
        public static readonly Pen AimPen = Pens.Red;

        public static readonly Color GuessFactorColour = Color.Purple;
        public static readonly Pen GuessFactorPen = new Pen(Color.Purple, 5);
        public static readonly Brush GuessFactorBrush = Brushes.Purple;

        public static readonly Color WaveColour = Color.Green;
        public static readonly Pen WavePen = Pens.Green;

        public static readonly Color VirtualBulletColour = Color.GreenYellow;
        public static readonly Pen VirtualBulletPen = Pens.GreenYellow;

        public static readonly Color MovementColour = Color.Yellow;
        public static readonly Pen MovementPen = Pens.Yellow;

        public static readonly Color AntiGravityMovementColour = Color.Yellow;
        public static readonly Pen AntiGravityMovementPen = Pens.Yellow;

        public static readonly Color LocationHistoryColour = Color.YellowGreen;
        public static readonly Pen LocationHistoryPen = Pens.YellowGreen;

        public static readonly Color WaveSurfingMovementColour = Color.Yellow;
        public static readonly Pen WaveSurfingMovementPen = Pens.Yellow;
        public static readonly Brush WaveSurfingMovementBrush = Brushes.Yellow;

        public static readonly Pen RadarPen = Pens.Orange;

        public static readonly Pen PifColour = Pens.SteelBlue;
    }
}