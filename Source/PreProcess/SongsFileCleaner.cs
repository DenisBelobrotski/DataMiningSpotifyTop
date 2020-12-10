namespace DataMiningSpotifyTop.Source.PreProcess
{
    public static class SongsFileCleaner
    {
        public static string CleanText(string fileContent)
        {
            return fileContent.Replace("\"\"", "");
        }
    }
}
