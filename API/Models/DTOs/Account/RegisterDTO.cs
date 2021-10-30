﻿namespace API.Models.DTOs.Account;

public record RegisterDTO
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public IFormFile? Avatar { get; set; }
}
