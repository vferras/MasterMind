using System;
using System.Collections.Generic;
using Api.Board.ValueObjects;

namespace UnitTest
{
    public class GameHistoric
    {
        private static List<Colour[]> Historic { get; set; }

        public static void Reset() => Historic = new List<Colour[]>();

        public static void AddCombinationChecked(Colour[] colours) => Historic.Add(colours);

        public static List<Colour[]> GetGameHistoric() => Historic;
    }
}