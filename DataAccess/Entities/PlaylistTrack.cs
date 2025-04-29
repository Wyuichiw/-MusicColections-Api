namespace DataAccess.Entities

{
    public class PlaylistTrack
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;
        public int TrackId { get; set; }
        public Track Track { get; set; } = null!;

        public override string ToString()
        {
            return $"Track: {Track?.Title ?? "Unknown"} in Playlist ID: {PlaylistId}";
        }
    }


}
