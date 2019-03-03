using Xunit;
using FluentAssertions;
using Api.Board.Aggregates;
using Api.Board.ValueObjects;
using Api;
using System;
using System.Collections.Generic;

namespace UnitTest
{
    public class BoardShould
    {
        [Fact]
        public void InitializeTheBoard()
        {
            Board.Initialize(new List<Colour> { Colour.Red, Colour.Green, Colour.Blue, Colour.White });
            Board.State.Should().Be(BoardState.Initialized);

            Pattern.GetPattern().Count.Should().Be(4);
            Pattern.GetPattern()[0].Should().Be(Colour.Red);
            Pattern.GetPattern()[1].Should().Be(Colour.Green);
            Pattern.GetPattern()[2].Should().Be(Colour.Blue);
            Pattern.GetPattern()[3].Should().Be(Colour.White);

            GameHistoric.GetGameHistoric().Count.Should().Be(0);
        }

        [Fact]
        public void Return_GameHistoric_When_SomeRowAreFilled()
        {
            Board.Initialize();

            GetRandomCombination(3).ForEach(combination => Board.CheckCombination(combination));

            var historic = Board.GetGameHistoric();
            historic.Should().HaveCount(3);
        }

        [Fact]
        public void Return_GameHistoric_Before_TheGameFinished()
        {
            var pattern = new List<Colour> { Colour.Purple, Colour.Purple, Colour.Green, Colour.Blue };

            Board.Initialize(pattern);

            Board.CheckCombination(pattern);
            GetRandomCombination(3).ForEach(combination =>
            {
                var checkResult = Board.CheckCombination(combination);
                checkResult.result.Should().BeFalse();
                checkResult.positionAndColour.Should().Be(0);
                checkResult.colour.Should().Be(0);
            });

            var historic = Board.GetGameHistoric();
            historic.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(Colour.Blue, Colour.Green, Colour.Purple, Colour.Purple, 1, 1, BoardState.Initialized)]
        [InlineData(Colour.Red, Colour.Green, Colour.Purple, Colour.Purple, 2, 0, BoardState.Initialized)]
        [InlineData(Colour.Red, Colour.Green, Colour.Red, Colour.Blue, 4, 0, BoardState.Discovered)]
        public void Return_Feedback_GivenACombination(Colour colour1, Colour colour2, Colour colour3, Colour colour4, int positionAndColour, int colour, BoardState state)
        {
            var colours = new List<Colour> { colour1, colour2, colour3, colour4 };
            Board.Initialize(colours);

            var result = Board.CheckCombination
            (
                new List<Colour> { Colour.Red, Colour.Green, Colour.Red, Colour.Blue }
            );

            result.colour.Should().Be(colour);
            result.positionAndColour.Should().Be(positionAndColour);
            Board.State.Should().Be(state);
        }

        [Fact]
        public void Return_GameOver_When_AllBoardRowsAreFull()
        {
            Board.Initialize();

            var combination = new List<Colour> { Colour.Purple, Colour.Purple, Colour.Green, Colour.Blue };

            for (var i = 1; i <= Constants.BoardSize; i++)
            {
                Board.State.Should().Be(BoardState.Initialized);
                Board.CheckCombination(combination);
            }

            Board.State.Should().Be(BoardState.GameOver);
        }

        private static List<List<Colour>> GetRandomCombination(int size)
        {
            var colourList = new List<List<Colour>>();

            for (var i = 1; i <= size; i++)
            {
                var colours = new List<Colour>();
                for (var x = 0; x < 4; x++)
                {
                    colours.Add((Colour)Enum.GetValues(typeof(Colour)).GetValue(new Random().Next(1, Constants.RowSize)));
                }
                colourList.Add(colours);
            }
            colourList.Remove(Pattern.GetPattern());

            return colourList;
        }
    }
}