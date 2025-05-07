using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Play.Catalog.API.DTOs;
using Play.Catalog.API.Extensions;
using Play.Catalog.API.Entites;
using Play.Common.Repositories;
using Play.Catalog.Contracts;

namespace Play.Catalog.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ItemsController(IRepository<Item> itemsRepository,
    IPublishEndpoint publishEndpoint,
    ILogger<ItemsController> logger) : ControllerBase
{
    private readonly IRepository<Item> _itemsRepository = itemsRepository;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILogger<ItemsController> _logger = logger;

    [HttpGet]
    public async Task<IEnumerable<ItemDTO>> GetAll()
    {
        var items = await _itemsRepository.GetAllAsync();

        return items.Select(item => item.AsDTO());
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

        await _publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

        return CreatedAtAction(nameof(GetByIdAsync), new { Id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDTO updateItem)
    {
        _logger.LogInformation("Updating item with id {Id}", id);
        var existingItem = await _itemsRepository.GetAsync(id);
        if (existingItem is null) return NotFound();
    
        existingItem.Name = updateItem.Name;
        existingItem.Description = updateItem.Description;
        existingItem.Price = updateItem.Price;

        await _itemsRepository.UpdateAsync(existingItem);

        await _publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);
        if (item is null) return NotFound();

        await _itemsRepository.DeleteAsync(id);
        
        await _publishEndpoint.Publish(new CatalogItemDeleted(id));

        return NoContent();
    }
}
