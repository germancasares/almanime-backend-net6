namespace API.Models.DTOs.Account;

public record LoginDTO
{
    public string? Identifier { get; set; }
    public string? Password { get; set; }
    public bool IsPersistent { get; set; }
}
