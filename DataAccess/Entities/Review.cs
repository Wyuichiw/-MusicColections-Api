namespace DataAccess.Entities

{
    public class Review
    {
        public int ReviewId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int AlbumId { get; set; }
        public Album? Album { get; set; }
        public int? Rating { get; set; } // 1 to 5
        public string? Comment { get; set; }

        public override string ToString()
        {
            return $"{UserName} rated {Album?.Title ?? "Unknown"}: {Rating}/5 - {Comment}";
        }
    }


}
