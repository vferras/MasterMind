using System;
using System.Collections.Generic;
using Api.Board.ValueObjects;
using UnitTest;

namespace Api.Board.Aggregates
{
    public class BoardAggregate
    {
        public static BoardState State { get; private set; }

        public static void Initialize()
        {
            Pattern.GenerateCombination();
            GameHistoric.Reset();
            State = BoardState.INITIALIZED;
        }

        public static (int colour, int positionAndColour) CheckCombination(Colour[] colours)
        {
            GameHistoric.AddCombinationChecked(colours);
            return (0, 0);
        }

        public static List<Colour[]> GetGameHistoric()
        {
            return GameHistoric.GetGameHistoric();
        }
    }
}