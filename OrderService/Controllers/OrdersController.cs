using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly HttpClient _httpClient;

    private static readonly List<Order> Orders =
    [
        new Order
        {
            Id = 1,
            ProductId = 1,
            Quantity = 2
        },
        new Order
        {
            Id = 2,
            ProductId = 2,
            Quantity = 5
        }
    ];

    public OrdersController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        return Ok(Orders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = Orders.FirstOrDefault(x => x.Id == id);

        if (order is null)
            return NotFound();

        var response = await _httpClient.GetAsync(
            $"https://localhost:7274/api/products/{order.ProductId}");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(503, "Product service is unavailable.");
        }

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();

        return Ok(new
        {
            order.Id,
            order.ProductId,
            ProductName = product?.Name,
            ProductPrice = product?.Price,
            order.Quantity
        });
    }
}