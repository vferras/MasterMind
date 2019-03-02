using System;
using Api.Board.ValueObjects;
using FluentAssertions;
using Xunit;

namespace UnitTest
{
    public class RowShould
    {
        [Fact]
        public void HaveASizeOf4()
        {
            Row.Colours.Length.Should().Be(4);
        }
    }
}
