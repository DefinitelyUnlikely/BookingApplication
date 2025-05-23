using System.Security.Claims;
using BookingApplication.Interfaces;
using BookingApplication.Models;
using BookingApplication.Models.Dtos;
using BookingApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookingApplication.Controllers;

[ApiController]
[Route("booking")]
public class BookingController(IService<Booking, CreateBookingRequest, EditBookingRequest> bookingService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Book([FromBody] CreateBookingRequest request)
    {
        try
        {
            if (request.UserId.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier) || !User.IsInRole("Admin"))
            {
                return Forbid("User id does not match logged in user. To book for someone else, please login as an admin");
            }

            var booking = await bookingService.CreateFromRequestAsync(request);
            return Created(nameof(Book), new { Message = $"Created booking with Id {booking.Id}" });
        }
        catch (ArgumentException e)
        {
            return BadRequest($"The request could not be processed: {e.Message}");
        }
        catch (Exception)
        {
            return BadRequest("An unspecified error has happened");
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] EditBookingRequest request)
    {
        try
        {
            if (request.UserId.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier) || !User.IsInRole("Admin"))
            {
                return Unauthorized("User id does not match logged in user. To edit a booking for someone else or transfer a booking to someone else, please login as an admin");
            }

            var booking = await bookingService.EditFromRequestAsync(request);
            return Created(nameof(Edit), new { Message = $"Edited booking with Id {booking.Id}", Booking = booking });
        }
        catch (ArgumentException e)
        {
            return BadRequest($"The request could not be processed: {e.Message}");
        }
        catch (Exception)
        {
            return BadRequest("An unspecified error has happened");
        }
    }

    [Authorize]
    [HttpDelete("{bookingId}")] //
    public async Task<IActionResult> Cancel(Guid bookingId)
    {
        try
        {
            var booking = await bookingService.GetByIdAsync(bookingId);
            if (booking?.UserId.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier) || !User.IsInRole("Admin"))
            {
                return Forbid("To cancel someone else's booking, please log in as an admin.");
            }
            // We imagine a method named something like this exists and we'll change it when 
            // the actual method does exist.
            // await bookingService.CancelById(bookingId);

            // Can we not use delete?
            await bookingService.DeleteAsync(bookingId);

            return Ok();
        }
        catch (Exception)
        {
            return BadRequest("An unspecified error has happened");
        }
    }
}

// public class CreateBookingRequest : IRequest
// {
//     public required DateTime StartDate { get; set; }
//     public required DateTime EndDate { get; set; }
//     public required Guid RoomId { get; set; }
//     public required string UserId { get; set; }
//     public Guid? ActivityId { get; set; }
// }

// public class EditBookingRequest : IRequest
// {
//     public required Guid Id { get; set; }
//     public required DateTime StartDate { get; set; }
//     public required DateTime EndDate { get; set; }
//     public required Guid RoomId { get; set; }
//     public required string UserId { get; set; }
//     public Guid? ActivityId { get; set; }
// }

