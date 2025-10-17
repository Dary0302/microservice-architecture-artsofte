using Microsoft.AspNetCore.Mvc;
using DeliveryService.Application.Services;
using DeliveryService.Core.Entities;

namespace DeliveryService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouriersController : ControllerBase
{
    private readonly ICourierService svc;
    public CouriersController(ICourierService svc) => this.svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await svc.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var c = await svc.GetAsync(id);
        if (c == null) return NotFound();
        return Ok(c);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Courier c)
    {
        var created = await svc.CreateAsync(c);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Courier c)
    {
        await svc.UpdateAsync(id, c);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await svc.DeleteAsync(id);
        return NoContent();
    }
}