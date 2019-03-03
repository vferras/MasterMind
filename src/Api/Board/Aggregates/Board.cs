using System.Collections.Generic;
using System.Linq;
using Api.Board.ValueObjects;

namespace Api.Board.Aggregates
{
    public class Board
    {
        public static BoardState State { get; private set; }

        public static void Initialize(List<Colour> colours = null)
        {
            Pattern.GenerateCombination(colours);
            GameHistoric.Reset();
            State = BoardState.Initialized;
        }

        public static (int colour, int positionAndColour, bool result) CheckCombination(List<Colour> checkedColours)
        {
            var pattern = Pattern.GetPattern().ToList();
            var foreachPosition = 0;
            List<Colour> positionAndColour = new List<Colour>(), colour = new List<Colour>();

            if (State != BoardState.Initialized) return (0, 0, false);

            foreach (var checkedColour in checkedColours)
            {
                if (checkedColour == pattern[foreachPosition])
                {
                    positionAndColour.Add(checkedColour);
                }
                else
                {
                    if (pattern.Any(c => c == checkedColour)
                        && !positionAndColour.Contains(checkedColour)
                        && !colour.Contains(checkedColour))
                    {
                        colour.Add(checkedColour);
                    }
                }

                foreachPosition++;
            }

            GameHistoric.AddCombinationChecked(checkedColours);

            CheckIfGameIsFinished(positionAndColour.Count);

            return (colour.Count, positionAndColour.Count, true);
        }

        public static IEnumerable<List<Colour>> GetGameHistoric() => GameHistoric.GetGameHistoric();

        private static void CheckIfGameIsFinished(int positionAndColour)
        {
            State = positionAndColour == Constants.RowSize ? BoardState.Discovered
                : GameHistoric.GetGameHistoric().Count == Constants.BoardSize ? BoardState.GameOver : BoardState.Initialized;
        }
    }
}