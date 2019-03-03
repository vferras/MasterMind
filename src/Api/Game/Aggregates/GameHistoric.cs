using System.Collections.Generic;
using Api.Game.ValueObjects;

namespace Api.Game.Aggregates
{
    public class GameHistoric
    {
        private static List<List<Colour>> Historic { get; set; }

        public static void Reset() => Historic = new List<List<Colour>>();

        public static void AddCombinationChecked(List<Colour> colours) => Historic.Add(colours);

        public static List<List<Colour>> GetGameHistoric() => Historic;
    }
}