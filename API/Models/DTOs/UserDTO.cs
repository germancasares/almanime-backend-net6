namespace API.Models.DTOs;

public record UserDTO
{
    public string? Name { get; set; }
    public IFormFile? Avatar { get; set; }
}
