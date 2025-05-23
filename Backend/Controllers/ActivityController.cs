using BookingApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("activity")]
public class ActivityController : ControllerBase
{
    private readonly ActivityService _activityService;

    public ActivityController(ActivityService activityService)
    {
        _activityService = activityService;
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateActivityRequest request)
    {
        try
        {
            var activity = await _activityService.CreateFromRequestAsync(request);
            return CreatedAtAction(nameof(Get), new { activityId = activity.Id} , activity );
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message});
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{activityId}")]
    public async Task<IActionResult> Update(Guid activityId, [FromBody] EditActivityRequest request)
    {
       try
       {
        if (activityId != request.Id)
        {
            return BadRequest(new { error = "Activity Id in URL must match the Id in the request body!"});
        }
        var activity = await _activityService.EditFromRequestAsync(request);
        return Ok(activity);
       }
       catch (Exception ex)
       {
        return BadRequest(new { error = ex.Message});
       }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{activityId}")]
    public async Task<IActionResult> Delete(Guid activityId)
    {
        try
            {
                await _activityService.DeleteAsync(activityId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
    }

    [HttpGet("{activityId}")] //
    public async Task<IActionResult> Get(Guid activityId)
    {
        try
        {
            var activity = await _activityService.GetByIdAsync(activityId);
            
            if (activity == null)
            {
                return NotFound(new { error = $"Activity with ID {activityId} not found"});
            }
            return Ok(activity);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message});
        }
    }
}

public class CreateActivityRequest : IRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}

public class EditActivityRequest : IRequest
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}