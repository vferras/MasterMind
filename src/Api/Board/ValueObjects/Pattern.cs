using System;
using Api.Board.ValueObjects;

namespace Api.Board.ValueObjects
{
    public class Pattern
    {
        private static Colour[] MasterPattern => new Colour[6];

        public static void GenerateCombination(Colour[] colours)
        {
            if (colours.Length != 6) throw new ArgumentException();
            Array.Copy(colours, MasterPattern, 6);
        }

        public static void GenerateCombination()
        {
            for (var i = 0; i <= 5; i++)
            {
                MasterPattern[i] = GetRandomColour();
            }
        }

        public static Colour[] GetPattern() => MasterPattern;

        private static Colour GetRandomColour() =>
            (Colour)Enum.GetValues(typeof(Colour)).GetValue(new Random().Next(0, 5));
    }
}