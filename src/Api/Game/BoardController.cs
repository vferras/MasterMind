using System;
using Api.Dtos;
using Api.Game.Aggregates;
using Microsoft.AspNetCore.Mvc;

namespace Api.Game
{
    [ApiController]
    public class BoardController : Controller
    {
        [HttpPost("board/initialize")]
        public IActionResult Initialize(ColoursDto request = null)
        {
            Board.Initialize(request.Colours);
            return Ok();
        }

        [HttpPost("board/pattern/check")]
        public IActionResult CheckPattern(ColoursDto colours)
        {
            var boardResult = Board.CheckPattern(colours.Colours);

            if (!boardResult.result)
            {
                return BadRequest("Board is not properly initialized");
            }

            return Ok(
                new
                {
                    ColoursCorrectlyGuessed = boardResult.colour,
                    ColoursAndPositionCorrectlyGuessed = boardResult.positionAndColour
                }
            );
        }

        [HttpPost("board/finish")]
        public IActionResult Finish()
        {
            Board.Finish();
            return Ok();
        }

        [HttpGet("board/historic")]
        public IActionResult GetHistoric()
        {
            return Ok(GameHistoric.GetGameHistoric());
        }

        [HttpGet("board/state")]
        public IActionResult GetState()
        {
            return Ok(Board.State);
        }
    }
}