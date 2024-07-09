namespace WebApiControllers.Constants
{
    public static class DataAccessConstants
    {
        // key name to point to connection string secret
        public const string MongoConn = "mongoConn";

        // database name to connect to
        public const string MongoDatabase = "recreation";

        // name of the collection containing records
        public const string MongoCollection = "restuarants";
    }
}
