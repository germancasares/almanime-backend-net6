namespace API.Models.Views;

public record UserView
{
    public string? AvatarUrl { get; set; }
    public string? Name { get; set; }
    public Guid IdentityID { get; set; }
    public List<string> Bookmarks { get; set; } = default!;
}
