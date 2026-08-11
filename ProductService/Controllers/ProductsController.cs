using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products =
    [
        new Product { Id = 1, Name = "Laptop", Price = 1000 },
        new Product { Id = 2, Name = "Mouse", Price = 30 },
        new Product { Id = 3, Name = "Keyboard", Price = 80 }
    ];

    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(Products);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetProduct(int id)
    {
        var product = Products.FirstOrDefault(x => x.Id == id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }
}