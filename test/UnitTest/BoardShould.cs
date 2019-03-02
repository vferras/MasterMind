using Xunit;
using Api.Board.Aggregate;
using FluentAssertions;

namespace UnitTest
{
    public class BoardShould
    {
        [Fact]
        public void HaveARowSizeOf12()
        {
            var board = new BoardAggregate();
            BoardAggregate.Rows.Length.Should().Be(12);
        }
    }
}