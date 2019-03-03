using System.Collections.Generic;

namespace Api.Game.ValueObjects
{
    public class GameHistoric
    {
        private static List<List<Colour>> Historic { get; set; }

        public static void Reset() => Historic = new List<List<Colour>>();

        public static void AddCombinationChecked(List<Colour> colours) => Historic.Add(colours);

        public static List<List<Colour>> GetGameHistoric() => Historic;
    }
}