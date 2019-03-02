using System;
using System.Collections.Generic;
using System.Linq;
using Api.Board.ValueObjects;
using UnitTest;

namespace Api.Board.Aggregates
{
    public class BoardAggregate
    {
        public static BoardState State { get; private set; }

        public static void Initialize(Colour[] colours = null)
        {
            Pattern.GenerateCombination(colours);
            GameHistoric.Reset();
            State = BoardState.INITIALIZED;
        }

        public static (int colour, int positionAndColour, bool result) CheckCombination(Colour[] checkedColours)
        {
            var pattern = Pattern.GetPattern().ToList();
            int foreachPosition = 0;
            List<Colour> positionAndColour = new List<Colour>(), colour = new List<Colour>();

            if (State != BoardState.INITIALIZED) return (0, 0, false);

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

        public static List<Colour[]> GetGameHistoric()
        {
            return GameHistoric.GetGameHistoric();
        }

        private static void CheckIfGameIsFinished(int positionAndColour)
        {
            State = positionAndColour == Constants.ROW_SIZE ? BoardState.DISCOVERED
                : GameHistoric.GetGameHistoric().Count == Constants.BOARD_SIZE ? BoardState.GAMEOVER : BoardState.INITIALIZED;
        }
    }
}