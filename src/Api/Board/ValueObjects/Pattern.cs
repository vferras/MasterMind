using System;
using Api.Board.ValueObjects;

namespace Api.Board.ValueObjects
{
    public class Pattern
    {
        private static Colour[] MasterPattern { get; set; }

        public static void GenerateCombination(Colour[] colours = null)
        {
            colours = colours ?? RandomPatternCombination();
            InitializePattern();

            if (colours.Length != Constants.ROW_SIZE) throw new ArgumentException();
            Array.Copy(colours, MasterPattern, Constants.ROW_SIZE);
        }

        private static Colour[] RandomPatternCombination()
        {
            Colour[] pattern = new Colour[4];
            InitializePattern();

            for (var i = 0; i < Constants.ROW_SIZE; i++)
            {
                pattern[i] = GetRandomColour();
            }

            return pattern;
        }

        public static Colour[] GetPattern() => MasterPattern;

        private static Colour GetRandomColour() =>
            (Colour)Enum.GetValues(typeof(Colour)).GetValue(new Random().Next(1, Constants.ROW_SIZE));

        private static void InitializePattern() =>
            MasterPattern = new Colour[Constants.ROW_SIZE];
    }
}