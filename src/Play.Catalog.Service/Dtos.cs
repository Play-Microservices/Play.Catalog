namespace Play.Catalog.Service.DTOs;

public record ItemDTO(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

public record CreateItemDTO(string Name, string Description, decimal Price);

public record UpdateItemDTO(string Name, string Description, decimal Price);
