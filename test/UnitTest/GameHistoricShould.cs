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
    public class GameHistoricShould
    {
        //[Fact]
        public void Return_TheHistoricOfAGame()
        {
            Board.Initialize(new List<Colour> { Colour.Red, Colour.Green, Colour.Blue, Colour.White });

            Board.CheckPattern(new List<Colour> { Colour.Blue, Colour.Green, Colour.White, Colour.Red });

            var gameHistoric = GameHistoric.GetGameHistoric();
            gameHistoric.Count.Should().Be(1);
            gameHistoric.First()[0].Should().Be(Colour.Blue);
            gameHistoric.First()[1].Should().Be(Colour.Green);
            gameHistoric.First()[2].Should().Be(Colour.White);
            gameHistoric.First()[3].Should().Be(Colour.Red);
        }
    }
}