namespace DataAccess.Entities

{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string YearsActive { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public string Genres { get; set; } = string.Empty;

        public List<Album>? Albums { get; set; } = new();

        public override string ToString()
        {
            return $"{Name} ({Country}) – {Genres}";
        }
    }


}
