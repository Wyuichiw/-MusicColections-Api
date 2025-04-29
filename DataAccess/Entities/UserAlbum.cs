namespace DataAccess.Entities
{
    public class UserAlbum
    {
        public int UserAlbumId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int AlbumId { get; set; }
        public Album? Album { get; set; } = null!;
        public DateTime DateAdded { get; set; }
        public string Status { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{UserName} added '{Album?.Title ?? "Unknown"}' on {DateAdded:yyyy-MM-dd} – Status: {Status}";
        }
    }
}
