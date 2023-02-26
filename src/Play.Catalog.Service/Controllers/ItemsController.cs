using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.Extensions;
using Play.Catalog.Service.Entites;
using Play.Common.Repositories;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IRepository<Item> _itemsRepository;
    private readonly ILogger<ItemsController> _logger;
    private static int _requestCounter = 0;

    public ItemsController(IRepository<Item> itemsRepository,
        ILogger<ItemsController> logger)
    {
        _itemsRepository = itemsRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDTO>>> GetAsync()
    {
        _requestCounter++;
        Console.WriteLine($"Request {_requestCounter}: Starting...");

        if (_requestCounter <= 2)
        {
            Console.WriteLine($"Request {_requestCounter}: Delaying...");
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
        
        if (_requestCounter <= 4)
        {
            Console.WriteLine($"Request {_requestCounter}: 500 (Internal Server Error)...");
            return StatusCode(500);
        }

        var items = await _itemsRepository.GetAllAsync();

            Console.WriteLine($"Request {_requestCounter}: 200 (OK)...");
        return Ok(items.Select(item => item.AsDTO()));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);

        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDTO>> PostAsync(CreateItemDTO createItem)
    {
        var item = new Item
        {
            Name = createItem.Name,
            Description = createItem.Description,
            Price = createItem.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };
        await _itemsRepository.CreateAsync(item);

        return CreatedAtAction(nameof(GetByIdAsync), new { Id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDTO updateItem)
    {
        var existingItem = await _itemsRepository.GetAsync(id);
        if (existingItem is null) return NotFound();
    
        existingItem.Name = updateItem.Name;
        existingItem.Description = updateItem.Description;
        existingItem.Price = updateItem.Price;

        await _itemsRepository.UpdateAsync(existingItem);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);
        if (item is null) return NotFound();

        await _itemsRepository.DeleteAsync(id);

        return NoContent();
    }
}
