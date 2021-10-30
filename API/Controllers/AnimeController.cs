﻿using API.Models.Enums;
using API.Models.Views;
using API.Models.Views.Derived;
using API.Services.Interfaces;
using API.Utils;
using API.Utils.DataAnnotations;
using API.Utils.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnimeController : ControllerBase
{
    private readonly IAnimeService _animeService;
    //private readonly IBookmarkService _bookmarkService;
    //private readonly IEpisodeService _episodeService;

    public AnimeController(
        IAnimeService animeService
    //IBookmarkService bookmarkService,
    //IEpisodeService episodeService
    )
    {
        _animeService = animeService;
        //_bookmarkService = bookmarkService;
        //_episodeService = episodeService;
    }

    [HttpGet("{ID}")]
    public IActionResult Get(Guid ID)
    {
        var anime = _animeService.GetByID(ID);

        if (anime == null) return NotFound();

        return Ok(anime.MapToViewWithEpisodes());
    }

    //[HttpGet("{ID}/episodes")]
    //public IActionResult GetEpisodes(Guid ID)
    //{
    //    var episodes = _episodeService.GetByAnimeID(ID);

    //    return Ok(_mapper.Map<IEnumerable<EpisodeVM>>(episodes));
    //}

    //[HttpGet("{ID}/episodes/{number}")]
    //public IActionResult GetEpisode(Guid ID, int number)
    //{
    //    var episode = _episodeService.GetByAnimeIDAndNumber(ID, number);

    //    if (episode == null) return NotFound();

    //    return Ok(_mapper.Map<EpisodeVM>(episode));
    //}

    [HttpGet("slug/{slug}")]
    public IActionResult GetBySlug(string slug)
    {
        var anime = _animeService.GetBySlug(slug);

        if (anime == null) return NotFound();

        return Ok(anime.MapToViewWithEpisodes());
    }

    //[HttpGet("slug/{slug}/episodes")]
    //public IActionResult GetEpisodesBySlug(string slug)
    //{
    //    var episodes = _episodeService.GetByAnimeSlug(slug);

    //    return Ok(_mapper.Map<IEnumerable<EpisodeVM>>(episodes));
    //}

    //[HttpGet("slug/{slug}/episodes/{number}")]
    //public IActionResult GetEpisodeBySlug(string slug, int number)
    //{
    //    var episode = _episodeService.GetByAnimeSlugAndNumber(slug, number);

    //    if (episode == null) return NotFound();

    //    return Ok(_mapper.Map<EpisodeVM>(episode));
    //}

    [HttpGet("year/{year}/season/{season}")]
    public IActionResult GetSeason(
        int year,
        ESeason season,
        [FromQuery] int page = 1,
        [FromQuery][Max(25)] int size = 8,
        [FromQuery] bool includeMeta = false
    )
    {
        var animesInPage = _animeService
            .GetSeason(year, season)
            .OrderBy(a => string.IsNullOrWhiteSpace(a.CoverImageUrl))
            .ThenBy(a => a.Name)
            .Page(page, size)
            .ToList();

        var animePage = new ModelWithMetaView<List<AnimeView>>
        {
            Models = animesInPage.Select(a => a.MapToView()).ToList(),
        };

        if (includeMeta)
        {
            animePage.Meta = new PaginationMetaView
            {
                BaseUrl = Request.GetFullPath(),
                Count = _animeService.GetAnimesInSeason(year, season),
                CurrentPage = page,
                PageSize = size,
            };
        }

        return Ok(animePage);
    }

    //[Authorize]
    //[HttpPost("slug/{slug}/bookmark")]
    //public IActionResult CreateBoookmark(string slug)
    //{
    //    var identityID = User.GetIdentityID();

    //    var bookmark = _bookmarkService.Create(slug, identityID);

    //    return Ok(_mapper.Map<BookmarkVM>(bookmark));
    //}

    //[Authorize]
    //[HttpDelete("slug/{slug}/bookmark")]
    //public void DeleteBoookmark(string slug)
    //{
    //    var identityID = User.GetIdentityID();

    //    _bookmarkService.Delete(slug, identityID);
    //}
}
