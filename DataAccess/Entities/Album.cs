namespace DataAccess.Entities
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public int TrackCount { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;

        public int ArtistId { get; set; }  
        public Artist? Artist { get; set; }

        public List<Track>? Tracks { get; set; } = new();  

        public override string ToString()
        {
            return $"{Title} by {Artist?.Name ?? "Unknown"} ({ReleaseYear}) – {Genre}";
        }
    }
}
