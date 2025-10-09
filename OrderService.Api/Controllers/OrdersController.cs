using Microsoft.AspNetCore.Mvc;
using OrderService.Logic.Services;
using CoreLib.Dtos;
using CoreLib.Entities;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService service;
        public OrdersController(IOrderService service) => this.service = service;

        [HttpGet]
        public async Task<IActionResult> GetByUser([FromQuery] int userId)
        {
            var list = await service.GetByUserAsync(userId);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var o = await service.GetAsync(id);
            if (o == null) return NotFound();
            return Ok(o);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var created = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatus status)
        {
            await service.UpdateStatusAsync(id, status);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}
