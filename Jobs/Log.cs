using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Jobs
{
    public static class Log
    {
        public static Dictionary<ELoggingEvent, LogLevel> LevelMap { get; } = new Dictionary<ELoggingEvent, LogLevel>
        {
            { ELoggingEvent.AnimeStatusNotInRange, LogLevel.Warning },
            { ELoggingEvent.SlugIsDelete, LogLevel.Warning },
            { ELoggingEvent.StartDateNotRecognized, LogLevel.Warning },

            { ELoggingEvent.AnimeCreated, LogLevel.Information },
            { ELoggingEvent.AnimeUpdated, LogLevel.Information },
            { ELoggingEvent.EpisodeCreated, LogLevel.Information },
            { ELoggingEvent.EpisodeUpdated, LogLevel.Information },
        };
    }
}
