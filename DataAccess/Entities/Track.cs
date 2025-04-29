using DataAccess.Entities;

public class Track
{
    public int TrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }  

    public int TrackNumber { get; set; }
    public string Composer { get; set; } = string.Empty;
    public string Lyricist { get; set; } = string.Empty;

    public int? AlbumId { get; set; }
    public Album? Album { get; set; }

    public override string ToString()
    {
        return $"{TrackNumber}. {Title} [{Duration}]";  
    }
}
