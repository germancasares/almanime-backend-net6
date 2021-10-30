using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Jobs
{
    public static class ExtensionHelper
    {
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
}
