using System;
using System.Collections.Generic;

namespace Api.Board.ValueObjects
{
    public class Pattern
    {
        private static List<Colour> _masterPattern;

        public static void GenerateCombination(List<Colour> colours = null)
        {
            colours = colours ?? RandomPatternCombination();

            if (colours.Count != Constants.RowSize) throw new ArgumentException();
            _masterPattern = new List<Colour>(colours);
        }

        private static List<Colour> RandomPatternCombination()
        {
            var pattern = new List<Colour>();

            for (var i = 0; i < Constants.RowSize; i++)
            {
                pattern.Add(GetRandomColour());
            }

            return pattern;
        }

        public static List<Colour> GetPattern() => _masterPattern;

        private static Colour GetRandomColour() =>
            (Colour)Enum.GetValues(typeof(Colour)).GetValue(new Random().Next(1, Constants.RowSize));
    }
}