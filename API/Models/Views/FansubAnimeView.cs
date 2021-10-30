﻿namespace API.Models.Views;

public record FansubAnimeView
{
    public string? Slug { get; set; }
    public string? Name { get; set; }
    public string? CoverImage { get; set; }
    public DateTime FinishedDate { get; set; }
}
