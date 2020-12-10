namespace DataMiningSpotifyTop.Source
{
    public static class SongsFileCleaner
    {
        public static string CleanText(string fileContent)
        {
            return fileContent.Replace("\"\"", "");
        }
    }
}
