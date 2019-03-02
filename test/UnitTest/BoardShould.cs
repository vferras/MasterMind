using Xunit;
using FluentAssertions;
using Api.Board.Aggregates;
using Api.Board.ValueObjects;
using Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    public class BoardShould
    {
        [Fact]
        public void InitializeTheBoard()
        {
            BoardAggregate.Initialize(new Colour[] { Colour.RED, Colour.GREEN, Colour.BLUE, Colour.WHITE });
            BoardAggregate.State.Should().Be(BoardState.INITIALIZED);

            Pattern.GetPattern().Length.Should().Be(4);
            Pattern.GetPattern()[0].Should().Be(Colour.RED);
            Pattern.GetPattern()[1].Should().Be(Colour.GREEN);
            Pattern.GetPattern()[2].Should().Be(Colour.BLUE);
            Pattern.GetPattern()[3].Should().Be(Colour.WHITE);

            GameHistoric.GetGameHistoric().Count.Should().Be(0);
        }

        [Fact]
        public void Return_GameHistoric_When_SomeRowAreFilled()
        {
            BoardAggregate.Initialize();

            GetRandomCombination(3).ForEach(combination => BoardAggregate.CheckCombination(combination));

            var historic = BoardAggregate.GetGameHistoric();
            historic.Should().HaveCount(3);
        }

        [Fact]
        public void Return_GameHistoric_State_Is_Initialized()
        {
            var pattern = new Colour[] { Colour.PURPLE, Colour.PURPLE, Colour.GREEN, Colour.BLUE };

            BoardAggregate.Initialize(pattern);

            BoardAggregate.CheckCombination(pattern);
            GetRandomCombination(3).ForEach(combination => BoardAggregate.CheckCombination(combination));

            var historic = BoardAggregate.GetGameHistoric();
            historic.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(new Colour[] { Colour.BLUE, Colour.GREEN, Colour.PURPLE, Colour.PURPLE }, 1, 1, BoardState.INITIALIZED)]
        [InlineData(new Colour[] { Colour.RED, Colour.GREEN, Colour.PURPLE, Colour.PURPLE }, 2, 0, BoardState.INITIALIZED)]
        [InlineData(new Colour[] { Colour.RED, Colour.GREEN, Colour.RED, Colour.BLUE }, 4, 0, BoardState.DISCOVERED)]
        public void Return_Feedback_GivenACombination(Colour[] colours, int positionAndColour, int colour, BoardState state)
        {
            BoardAggregate.Initialize(colours);

            var result = BoardAggregate.CheckCombination
            (
                new Colour[] { Colour.RED, Colour.GREEN, Colour.RED, Colour.BLUE }
            );

            result.colour.Should().Be(colour);
            result.positionAndColour.Should().Be(positionAndColour);
            BoardAggregate.State.Should().Be(state);
        }

        [Fact]
        public void Return_GameOver_When_AllBoardRowsAreFull()
        {
            BoardAggregate.Initialize();

            var combination = new Colour[] { Colour.PURPLE, Colour.PURPLE, Colour.GREEN, Colour.BLUE };

            for (var i = 1; i <= Constants.BOARD_SIZE; i++)
            {
                BoardAggregate.State.Should().Be(BoardState.INITIALIZED);
                BoardAggregate.CheckCombination(combination);
            }

            BoardAggregate.State.Should().Be(BoardState.GAMEOVER);
        }

        private List<Colour[]> GetRandomCombination(int size)
        {
            var colourList = new List<Colour[]>();

            for (var i = 1; i <= size; i++)
            {
                var colours = new Colour[4];
                for (var x = 0; x < 4; x++)
                {
                    colours[x] = (Colour)Enum.GetValues(typeof(Colour)).GetValue(new Random().Next(1, Constants.ROW_SIZE));
                }
                colourList.Add(colours);
            }
            colourList.Remove(Pattern.GetPattern());

            return colourList;
        }
    }
}