namespace DataAccess.Entities

{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
       

        public override string ToString()
        {
            return $"Playlist: {Title} by {Author}, created on {CreatedDate:yyyy-MM-dd}";
        }
    }


}
