using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Data
{
    public class MusicColectionsDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<UserAlbum> UserAlbums { get; set; }
        public object Users { get; set; }

        public MusicColectionsDbContext()
        {
            // Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MusicCollectionDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            optionsBuilder.UseSqlServer(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlaylistTrack>()
                .HasKey(pt => new { pt.PlaylistId, pt.TrackId });

            // Artists
            modelBuilder.Entity<Artist>().HasData(
                new Artist { ArtistId = 1, Name = "Heitspeech", Country = "Ukraine", YearsActive = "2022–", Genres = "Punk", Biography = "Ukrainian punk band." },
                new Artist { ArtistId = 2, Name = "Imagine Dragons", Country = "USA", YearsActive = "2008–", Genres = "Alternative/Pop Rock", Biography = "American pop rock band." },
                new Artist { ArtistId = 3, Name = "Michael Jackson", Country = "USA", YearsActive = "1964–2009", Genres = "Pop, R&B", Biography = "The King of Pop." },
                new Artist { ArtistId = 4, Name = "Nirvana", Country = "USA", YearsActive = "1987–1994", Genres = "Grunge", Biography = "American grunge band." },
                new Artist { ArtistId = 5, Name = "Linkin Park", Country = "USA", YearsActive = "1996–2017", Genres = "Nu Metal", Biography = "American rock band known for hybrid sound." },
                new Artist { ArtistId = 6, Name = "The Weeknd", Country = "Canada", YearsActive = "2010–", Genres = "R&B, Pop", Biography = "Canadian singer and producer." }
            );

            // Albums
            modelBuilder.Entity<Album>().HasData(
                new Album { AlbumId = 1, Title = "Wrath of a Generation", Genre = "Punk", ReleaseYear = 2024, TrackCount = 3, Label = "Indie UA", ArtistId = 1, Format = "Digital" },
                new Album { AlbumId = 2, Title = "Evolve", Genre = "Alternative", ReleaseYear = 2017, TrackCount = 2, Label = "Interscope", ArtistId = 2, Format = "CD" },
                new Album { AlbumId = 3, Title = "Thriller", Genre = "Pop", ReleaseYear = 1982, TrackCount = 2, Label = "Epic", ArtistId = 3, Format = "Vinyl" },
                new Album { AlbumId = 4, Title = "Nevermind", Genre = "Grunge", ReleaseYear = 1991, TrackCount = 3, Label = "DGC", ArtistId = 4, Format = "CD" },
                new Album { AlbumId = 5, Title = "Hybrid Theory", Genre = "Nu Metal", ReleaseYear = 2000, TrackCount = 5, Label = "Warner Bros.", ArtistId = 5, Format = "CD" },
                new Album { AlbumId = 6, Title = "After Hours", Genre = "R&B", ReleaseYear = 2020, TrackCount = 5, Label = "XO", ArtistId = 6, Format = "Digital" }
            );

            // Tracks
            modelBuilder.Entity<Track>().HasData(
                // Heitspeech
                new Track { TrackId = 1, Title = "Birds Fly Home", TrackNumber = 1, Composer = "Heitspeech", Lyricist = "Heitspeech", AlbumId = 1 },
                new Track { TrackId = 2, Title = "I Will Kill All Gods", Duration = new TimeSpan(0, 2, 58), TrackNumber = 2, Composer = "Heitspeech", Lyricist = "Heitspeech", AlbumId = 1 },
                new Track { TrackId = 3, Title = "I Will Die Not My Death", Duration = new TimeSpan(0, 4, 12), TrackNumber = 3, Composer = "Heitspeech", Lyricist = "Heitspeech", AlbumId = 1 },

                // Imagine Dragons
                new Track { TrackId = 4, Title = "Believer", Duration = new TimeSpan(0, 3, 24), TrackNumber = 1, Composer = "Imagine Dragons", Lyricist = "Dan Reynolds", AlbumId = 2 },
                new Track { TrackId = 5, Title = "Whatever It Takes", Duration = new TimeSpan(0, 3, 21), TrackNumber = 2, Composer = "Imagine Dragons", Lyricist = "Dan Reynolds", AlbumId = 2 },

                // Michael Jackson
                new Track { TrackId = 6, Title = "Thriller", Duration = new TimeSpan(0, 5, 57), TrackNumber = 1, Composer = "Rod Temperton", Lyricist = "Rod Temperton", AlbumId = 3 },
                new Track { TrackId = 7, Title = "Billie Jean", Duration = new TimeSpan(0, 4, 54), TrackNumber = 2, Composer = "Michael Jackson", Lyricist = "Michael Jackson", AlbumId = 3 },

                // Nirvana
                new Track { TrackId = 8, Title = "Smells Like Teen Spirit", Duration = new TimeSpan(0, 5, 1), TrackNumber = 1, Composer = "Kurt Cobain", Lyricist = "Kurt Cobain", AlbumId = 4 },
                new Track { TrackId = 9, Title = "Come As You Are", Duration = new TimeSpan(0, 3, 39), TrackNumber = 2, Composer = "Kurt Cobain", Lyricist = "Kurt Cobain", AlbumId = 4 },
                new Track { TrackId = 10, Title = "Lithium", Duration = new TimeSpan(0, 4, 17), TrackNumber = 3, Composer = "Kurt Cobain", Lyricist = "Kurt Cobain", AlbumId = 4 },

                // Linkin Park
                new Track { TrackId = 11, Title = "In the End", Duration = new TimeSpan(0, 3, 36), TrackNumber = 1, Composer = "Linkin Park", Lyricist = "Chester Bennington", AlbumId = 5 },
                new Track { TrackId = 12, Title = "Papercut", Duration = new TimeSpan(0, 3, 5), TrackNumber = 2, Composer = "Linkin Park", Lyricist = "Mike Shinoda", AlbumId = 5 },
                new Track { TrackId = 13, Title = "One Step Closer", Duration = new TimeSpan(0, 2, 36), TrackNumber = 3, Composer = "Linkin Park", Lyricist = "Chester Bennington", AlbumId = 5 },
                new Track { TrackId = 14, Title = "Crawling", Duration = new TimeSpan(0, 3, 29), TrackNumber = 4, Composer = "Linkin Park", Lyricist = "Mike Shinoda", AlbumId = 5 },
                new Track { TrackId = 15, Title = "Points of Authority", Duration = new TimeSpan(0, 3, 20), TrackNumber = 5, Composer = "Linkin Park", Lyricist = "Mike Shinoda", AlbumId = 5 },

                // The Weeknd
                new Track { TrackId = 16, Title = "Blinding Lights", Duration = new TimeSpan(0, 3, 20), TrackNumber = 1, Composer = "The Weeknd", Lyricist = "The Weeknd", AlbumId = 6 },
                new Track { TrackId = 17, Title = "Save Your Tears", Duration = new TimeSpan(0, 3, 35), TrackNumber = 2, Composer = "The Weeknd", Lyricist = "The Weeknd", AlbumId = 6 },
                new Track { TrackId = 18, Title = "In Your Eyes", Duration = new TimeSpan(0, 3, 58), TrackNumber = 3, Composer = "The Weeknd", Lyricist = "The Weeknd", AlbumId = 6 },
                new Track { TrackId = 19, Title = "Heartless", Duration = new TimeSpan(0, 3, 18), TrackNumber = 4, Composer = "The Weeknd", Lyricist = "The Weeknd", AlbumId = 6 },
                new Track { TrackId = 20, Title = "Alone Again", Duration = new TimeSpan(0, 4, 10), TrackNumber = 5, Composer = "The Weeknd", Lyricist = "The Weeknd", AlbumId = 6 }
            );

            // Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review { ReviewId = 1, UserName = "rockFan89", AlbumId = 1, Rating = 5, Comment = "Pure punk energy!" },
                new Review { ReviewId = 2, UserName = "dragonLover", AlbumId = 2, Rating = 4, Comment = "Catchy and motivating." },
                new Review { ReviewId = 3, UserName = "mjForever", AlbumId = 3, Rating = 5, Comment = "A timeless classic." },
                new Review { ReviewId = 4, UserName = "grungeSoul", AlbumId = 4, Rating = 4, Comment = "Emotional and raw." },
                new Review { ReviewId = 5, UserName = "nuMetalFan", AlbumId = 5, Rating = 5, Comment = "Still hits hard!" },
                new Review { ReviewId = 6, UserName = "rbnbeats", AlbumId = 6, Rating = 3, Comment = "Smooth but repetitive." }
            );

            // Playlists
            modelBuilder.Entity<Playlist>().HasData(
                new Playlist { PlaylistId = 1, Title = "Morning Boost", Author = "Anna", CreatedDate = new DateTime(2024, 3, 1) },
                new Playlist { PlaylistId = 2, Title = "Old School Vibes", Author = "Mike", CreatedDate = new DateTime(2023, 10, 15) },
                new Playlist { PlaylistId = 3, Title = "Chill Evenings", Author = "Olena", CreatedDate = new DateTime(2025, 1, 10) }
            );

            // PlaylistTracks
            modelBuilder.Entity<PlaylistTrack>().HasData(
                new PlaylistTrack { PlaylistId = 1, TrackId = 4 },  // Believer
                new PlaylistTrack { PlaylistId = 1, TrackId = 11 }, // In the End
                new PlaylistTrack { PlaylistId = 1, TrackId = 16 }, // Blinding Lights

                new PlaylistTrack { PlaylistId = 2, TrackId = 6 },  // Thriller
                new PlaylistTrack { PlaylistId = 2, TrackId = 8 },  // Smells Like Teen Spirit
                new PlaylistTrack { PlaylistId = 2, TrackId = 13 }, // One Step Closer

                new PlaylistTrack { PlaylistId = 3, TrackId = 17 }, // Save Your Tears
                new PlaylistTrack { PlaylistId = 3, TrackId = 18 }, // In Your Eyes
                new PlaylistTrack { PlaylistId = 3, TrackId = 19 }  // Heartless
            );
           
            modelBuilder.Entity<Playlist>().HasData(
                new Playlist { PlaylistId = 6, Title = "My Favorite Tracks", Author = "User123", CreatedDate = new DateTime(2025, 4, 28) }
            );

            // Додавання
            modelBuilder.Entity<PlaylistTrack>().HasData(
                new PlaylistTrack { PlaylistId = 6, TrackId = 1 },  // Birds Fly Home (Heitspeech)
                new PlaylistTrack { PlaylistId = 6, TrackId = 4 },  // Believer (Imagine Dragons)
                new PlaylistTrack { PlaylistId = 6, TrackId = 6 },  // Thriller (Michael Jackson)
                new PlaylistTrack { PlaylistId = 6, TrackId = 8 },  // Smells Like Teen Spirit (Nirvana)
                new PlaylistTrack { PlaylistId = 6, TrackId = 11 }, // In the End (Linkin Park)
                new PlaylistTrack { PlaylistId = 6, TrackId = 16 }, // Blinding Lights (The Weeknd)
                new PlaylistTrack { PlaylistId = 6, TrackId = 17 }, // Save Your Tears (The Weeknd)
                new PlaylistTrack { PlaylistId = 6, TrackId = 19 }  // Heartless (The Weeknd)
            );




        }
    }
}
