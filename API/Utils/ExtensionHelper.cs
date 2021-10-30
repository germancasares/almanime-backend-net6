using API.Models.Enums;
using System.Text.Json;

namespace API.Utils;

public static class ExtensionHelper
{
    public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize) => source.Skip((page - 1) * pageSize).Take(pageSize);
    public static string GetFullPath(this HttpRequest request) => $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}";
    public static string GetExtension(this IFormFile file) => Path.GetExtension(file.FileName);

    public static void Emit<T>(this ILogger<T> logger, ELoggingEvent loggingEvent, object obj)
    {
        var level = Log.LevelMap.ContainsKey(loggingEvent) ? Log.LevelMap[loggingEvent] : LogLevel.None;
        var eventId = (int)loggingEvent;

        // TODO: How can I add this to obj???
        //obj.EventScope = loggingEvent.GetType().Name;
        //obj.EventName = loggingEvent.ToString();

        string message = JsonSerializer.Serialize(obj);

        logger.Log(level, eventId, message);
    }
}
