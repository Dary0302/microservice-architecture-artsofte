using Microsoft.AspNetCore.Mvc;
using DeliveryService.Core.Interfaces;

namespace DeliveryService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryLogsController : ControllerBase
{
    private readonly IDeliveryLogRepository repo;
    public DeliveryLogsController(IDeliveryLogRepository repo) => this.repo = repo;

    [HttpGet("byDelivery/{deliveryId}")]
    public async Task<IActionResult> GetByDelivery(int deliveryId)
    {
        var list = await repo.ListByDeliveryAsync(deliveryId);
        return Ok(list);
    }
}