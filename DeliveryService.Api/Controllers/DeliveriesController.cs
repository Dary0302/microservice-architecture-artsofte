using Microsoft.AspNetCore.Mvc;
using DeliveryService.Application.Services;
using DeliveryService.Core.Entities;

namespace DeliveryService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveriesController : ControllerBase
{
    private readonly IDeliveryServiceApp svc;
    public DeliveriesController(IDeliveryServiceApp svc) => this.svc = svc;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var d = await svc.GetAsync(id);
        if (d == null) return NotFound();
        return Ok(d);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign([FromBody] AssignRequest req)
    {
        var d = await svc.AssignAsync(req.OrderId, req.CourierId);
        return CreatedAtAction(nameof(Get), new { id = d.Id }, d);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest req)
    {
        await svc.UpdateStatusAsync(id, req.Status);
        return NoContent();
    }
}

public record AssignRequest(int OrderId, int CourierId);
public record UpdateStatusRequest(DeliveryStatus Status);