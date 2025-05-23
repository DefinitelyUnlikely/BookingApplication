using BookingApplication.Models.Dtos;
using BookingApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingApplication.Controllers;

[ApiController]
[Route("room")]
public class RoomController : ControllerBase
{
    private readonly RoomService roomService;

    public RoomController(RoomService roomService)
    {
        this.roomService = roomService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
    {
        try
        {
            var room = await roomService.CreateFromRequestAsync(request);

            if (room == null)
                return BadRequest("Invalid request");

            return Ok(room);
        }
        catch (ArgumentException e)
        {
            return BadRequest($"The request could not be processed: {e.Message}");
        }
        catch
        {
            return StatusCode(500, new { errors = "An unexpected error occured." });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{roomId}")]
    public async Task<IActionResult> Update(Guid roomId, [FromBody] EditRoomRequest request)
    {
        try
        {
            var room = await roomService.GetByIdAsync(roomId);

            if (room == null)
                return NotFound();

            room = await roomService.EditFromRequestAsync(request);
            return Ok(room);
        }
        catch (ArgumentException e)
        {
            return BadRequest($"The request could not be processed: {e.Message}");
        }
        catch
        {
            return StatusCode(500, new { errors = "An unexpected error occured." });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{roomId}")]
    public async Task<IActionResult> Delete(Guid roomId)
    {
        try
        {
            var room = await roomService.GetByIdAsync(roomId);

            if (room == null)
                return NotFound();

            await roomService.DeleteAsync(roomId);
            return Ok();
        }
        catch
        {
            return StatusCode(500, new { errors = "An unexpected error occured." });
        }
    }

    [HttpGet("{roomId}")] // 
    public async Task<IActionResult> Get(Guid roomId)
    {
        try
        {
            var room = await roomService.GetByIdAsync(roomId);

            if (room == null)
                return NotFound();

            return Ok(room);
        }
        catch
        {
            return StatusCode(500, new { errors = "An unexpected error occured." });
        }
    }
}