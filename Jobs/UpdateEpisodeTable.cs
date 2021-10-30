using API.Models.DTOs;
using Jobs.Contracts;
using Jobs.Contracts.Episode;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace Jobs
{
    public class UpdateEpisodeTable
    {
        public const string UpdateEpisodeTableQueue = "update-episode-table";

        private const int MAX_PER_PAGE = 20;
        private const string KitsuAPI = "https://kitsu.io/api/edge";
        private static readonly string EpisodeURL = $"{{0}}/anime/{{1}}/episodes?page[limit]={{2}}";
        private static readonly HttpClient Client = new HttpClient();

        //private readonly IEpisodeService _episodeService;
        private readonly ILogger<UpdateEpisodeTable> _logger;

        public UpdateEpisodeTable(
            //IEpisodeService episodeService,
            ILogger<UpdateEpisodeTable> logger
        )
        {
            //_episodeService = episodeService;
            _logger = logger;
        }

        //[FunctionName("UpdateEpisodeTable")]
        public async Task Run([QueueTrigger(UpdateEpisodeTableQueue)] AnimeContract anime)
        {
            var episodes = await GetEpisodes(anime.ID);

            var episodesDTOs = episodes
                .Select(e => MapEpisode(e, anime.Slug))
                .ToList();

            episodesDTOs.ForEach(e => CreateOrUpdateEpisode(e));
        }

        private async Task<List<EpisodeDataModel>> GetEpisodes(string animeID)
        {
            var episodeDataModels = new List<EpisodeDataModel>();

            var url = string.Format(EpisodeURL, KitsuAPI, animeID, MAX_PER_PAGE);
            while (!string.IsNullOrWhiteSpace(url))
            {
                var (next, rawEpisodes) = await ProcessEpisodePage(url);
                episodeDataModels.AddRange(rawEpisodes);
                url = next;
            }

            return episodeDataModels;
        }

        private async Task<(string Next, List<EpisodeDataModel> EpisodeDataModel)> ProcessEpisodePage(string url)
        {
            var response = await Client.GetStringAsync(url);
            var episodeCollection = JsonSerializer.Deserialize<EpisodeCollection>(response);
            return (episodeCollection.Links.Next, episodeCollection.Data);
        }

        private static EpisodeDTO MapEpisode(EpisodeDataModel model, string animeSlug)
        {
            var episode = model.Attributes;

            var endDateCorrect = DateTime.TryParseExact(episode.Airdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime airDate);

            return new EpisodeDTO
            {
                Aired = endDateCorrect ? airDate : null,
                Duration = episode.Length,
                Name = episode.CanonicalTitle,
                Number = episode.Number.Value,
                AnimeSlug = animeSlug,
            };
        }

        private void CreateOrUpdateEpisode(EpisodeDTO episode)
        {
            //if (_episodeService.GetByAnimeSlugAndNumber(episode.AnimeSlug, episode.Number) == null)
            //{
            //    _episodeService.Create(episode);
            //    _logger.Emit(ELoggingEvent.EpisodeCreated, new { episode.AnimeSlug, EpisodeNumber = episode.Number });

            //}
            //else
            //{
            //    _episodeService.Update(episode);
            //    _logger.Emit(ELoggingEvent.EpisodeUpdated, new { episode.AnimeSlug, EpisodeNumber = episode.Number });
            //}
        }
    }
}
