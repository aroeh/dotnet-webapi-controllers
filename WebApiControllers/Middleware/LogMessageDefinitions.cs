namespace WebApiControllers.Middleware
{
    public static partial class LogMessageDefinitions
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Information,
            Message = "Get all restuarants request received"
        )]
        public static partial void GetAllRestuarants(this ILogger logger);

        [LoggerMessage(
            EventId = 2,
            Level = LogLevel.Information,
            Message = "Get all restuarants request complete...returning results"
        )]
        public static partial void GetAllRestuarantsComplete(this ILogger logger);

        [LoggerMessage(
            EventId = 3,
            Level = LogLevel.Information,
            Message = "Find restuarants request received.  Criteria: {criteria}"
        )]
        public static partial void FindRestuarants(this ILogger logger, string criteria);

        [LoggerMessage(
            EventId = 4,
            Level = LogLevel.Information,
            Message = "Find restuarants request complete...returning results"
        )]
        public static partial void FindRestuarantsComplete(this ILogger logger);

        [LoggerMessage(
            EventId = 5,
            Level = LogLevel.Information,
            Message = "Get restuarant request received...Id {id}"
        )]
        public static partial void RestuarantById(this ILogger logger, string id);

        [LoggerMessage(
            EventId = 6,
            Level = LogLevel.Information,
            Message = "Get restuarant request complete...returning results"
        )]
        public static partial void RestuarantByIdComplete(this ILogger logger);

        [LoggerMessage(
            EventId = 7,
            Level = LogLevel.Information,
            Message = "Add restuarant request received...Request: {newRestuarant}"
        )]
        public static partial void AddRestuarant(this ILogger logger, string newRestuarant);

        [LoggerMessage(
            EventId = 8,
            Level = LogLevel.Information,
            Message = "Add restuarant request complete...returning results"
        )]
        public static partial void AddRestuarantComplete(this ILogger logger);

        [LoggerMessage(
            EventId = 9,
            Level = LogLevel.Information,
            Message = "Add restuarant request received...Request: {restuarant}"
        )]
        public static partial void UpdateRestuarant(this ILogger logger, string restuarant);

        [LoggerMessage(
            EventId = 10,
            Level = LogLevel.Information,
            Message = "Add restuarant request complete...returning results"
        )]
        public static partial void UpdateRestuarantComplete(this ILogger logger);

        [LoggerMessage(
            EventId = 99,
            Level = LogLevel.Error,
            Message = "An error occurred while processing your request: {traceId} | {message}"
        )]
        public static partial void Error(this ILogger logger, string traceId, string message);
    }
}
