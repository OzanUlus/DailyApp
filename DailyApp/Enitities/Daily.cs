namespace DailyApp.Enitities
{
    public class Daily
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string AddedIp { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
