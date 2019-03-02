using Xunit;
using FluentAssertions;
using Api.Board.Aggregates;
using Api.Board.ValueObjects;
using System.Collections.Generic;

namespace UnitTest
{
    public class BoardShould
    {
        [Fact]
        public void InitializeTheBoard()
        {
            BoardAggregate.Initialize();
            BoardAggregate.State.Should().Be(BoardState.INITIALIZED);
            Pattern.GetPattern().Length.Should().Be(6);
        }

        [Fact]
        public void ReturnGameHistoric()
        {
            var combination1 = new Colour[] { Colour.BLUE, Colour.GREEN, Colour.PURPLE, Colour.PURPLE, Colour.GREEN, Colour.BLUE };
            var combination2 = new Colour[] { Colour.GREEN, Colour.PURPLE, Colour.PURPLE, Colour.RED, Colour.GREEN, Colour.RED };
            var combination3 = new Colour[] { Colour.PURPLE, Colour.GREEN, Colour.GREEN, Colour.PURPLE, Colour.GREEN, Colour.GREEN };

            BoardAggregate.Initialize();
            BoardAggregate.CheckCombination(combination1);
            BoardAggregate.CheckCombination(combination2);
            BoardAggregate.CheckCombination(combination3);

            var historic = BoardAggregate.GetGameHistoric();
            historic.Should().HaveCount(3);
        }

        [Theory]
        [InlineData(new Colour[] { Colour.BLUE, Colour.GREEN, Colour.PURPLE, Colour.PURPLE, Colour.GREEN, Colour.BLUE })]
        public void ReturnFeedbackGivenACombination(Colour[] colours)
        {
            Pattern.GenerateCombination(colours);

            var result = BoardAggregate.CheckCombination
            (
                new Colour[] { Colour.RED, Colour.GREEN, Colour.RED, Colour.BLUE, Colour.WHITE, Colour.YELLOW }
            );

            result.colour.Should().Be(4);
            result.positionAndColour.Should().Be(1);
        }
    }
}