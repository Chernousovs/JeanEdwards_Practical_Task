namespace JE_PracticalTask.APIs
{
    public static class OMDb_API
    {
        private static string BaseUrl = "http://www.omdbapi.com/?apikey=2cb8c6a7";
        public static string FindByTitle = BaseUrl + "&s=\"{0}\"";
        public static string FindByImdbId = BaseUrl + "&i={0}";
    }
}
