using Xunit;
using FluentAssertions;
using Api.Game.Aggregates;
using Api.Game.ValueObjects;
using Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    [Collection("UnitTest")]
    public class BoardShould
    {
        [Fact]
        public void Initialize_TheBoard()
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

            GetRandomCombination(3).ForEach(combination => Board.CheckPattern(combination));

            var historic = GameHistoric.GetGameHistoric();
            historic.Should().HaveCount(3);
        }

        [Fact]
        public void Return_GameHistoric_Before_TheGameFinished()
        {
            var pattern = new List<Colour> { Colour.Purple, Colour.Purple, Colour.Green, Colour.Blue };

            Board.Initialize(pattern);

            Board.CheckPattern(pattern);
            GetRandomCombination(3).ForEach(combination =>
            {
                var checkResult = Board.CheckPattern(combination);
                checkResult.result.Should().BeFalse();
                checkResult.positionAndColour.Should().Be(0);
                checkResult.colour.Should().Be(0);
            });

            var historic = GameHistoric.GetGameHistoric();
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

            var result = Board.CheckPattern
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

            GetRandomCombination(12).ForEach(combination =>
            {
                Board.State.Should().Be(BoardState.Initialized);
                Board.CheckPattern(combination);
            });

            Board.State.Should().Be(BoardState.GameOver);
        }

        [Fact]
        public void Finish_TheBoard()
        {
            Board.Initialize();

            Board.CheckPattern(GetRandomCombination(1).Single());

            Board.Finish();

            Board.State.Should().Be(BoardState.FinishedByUser);
            GameHistoric.GetGameHistoric().Count().Should().Be(1);
        }

        private static List<List<Colour>> GetRandomCombination(int size)
        {
            var colourList = new List<List<Colour>>();

            for (var i = 1; i <= size; i++)
            {
                var colours = new List<Colour>();
                for (var x = 0; x < 4; x++)
                {
                    var colour = (Colour)Enum.GetValues(typeof(Colour)).GetValue(new Random().Next(1, Constants.RowSize));
                    if (colour == Pattern.GetPattern()[x])
                    {
                        x--;
                        continue;
                    }
                    colours.Add(colour);
                }

                colourList.Add(colours);
            }

            return colourList;
        }
    }
}