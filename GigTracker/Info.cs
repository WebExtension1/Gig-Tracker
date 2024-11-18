namespace GigTracker
{
    public partial class GigTracker
    {
        // ABOVE AND BEYOND READING
        // Do some reading on static - the ENTIRE Info class can probably become static
        // This means you will never actually create an Info object, there just is 1 of them and it is the class
        // Also you could read about "singletons"


        public class Info
        {
            public List<Gig> gigs = new List<Gig>();
            public List<Band> bands = new List<Band>();
            public List<Album> albums = new List<Album>();
            public List<Song> songs = new List<Song>();
            public List<Venue> venues = new List<Venue>();
            public List<Artist> artists = new List<Artist>();
            public int selected = 1;
            public string profile = "";

            public void PopulateInfo(Info info)
            {
                gigs = new List<Gig>();
                bands = new List<Band>();
                albums = new List<Album>();
                songs = new List<Song>();
                venues = new List<Venue>();
                artists = new List<Artist>();

                string readLine;
                StreamReader sr;
                sr = new StreamReader(@"Venues.txt");
                do
                {
                    readLine = sr.ReadLine();
                    if (readLine != "end")
                    {
                        Venue venue = new Venue();
                        venue.venueID = Convert.ToInt32(readLine);
                        venue.city = sr.ReadLine();
                        venue.cityPages = sr.ReadLine().Split(',').ToList();
                        venue.venue = sr.ReadLine();
                        venue.venuePages = sr.ReadLine().Split(',').ToList();
                        venue.notes = sr.ReadLine();
                        venues.Add(venue);
                    }
                }
                while (readLine != "end");
                sr.Close();

                sr = new StreamReader(@$"Artists.txt");
                do
                {
                    readLine = sr.ReadLine();
                    if (readLine != "end")
                    {
                        Artist artist = new Artist();
                        artist.artistID = Convert.ToInt32(readLine);
                        artist.name = sr.ReadLine();
                        artist.artistPages = sr.ReadLine().Split(',').ToList();
                        artist.notes = sr.ReadLine();
                        artists.Add(artist);
                    }
                }
                while (readLine != "end");
                sr.Close();

                sr = new StreamReader(@"Bands.txt");
                do
                {
                    readLine = sr.ReadLine();
                    if (readLine != "end")
                    {
                        Band band = new Band();
                        band.bandID = Convert.ToInt32(readLine);
                        band.name = sr.ReadLine();
                        band.members = GetArtistArrayFromID(info, sr.ReadLine().Split(',').ToList());
                        band.bandPages = sr.ReadLine().Split(',').ToList();
                        band.notes = sr.ReadLine();
                        bands.Add(band);
                    }
                }
                while (readLine != "end");
                sr.Close();

                sr = new StreamReader(@"Songs.txt");
                do
                {
                    readLine = sr.ReadLine();
                    if (readLine != "end")
                    {
                        Song song = new Song();
                        song.songID = Convert.ToInt32(readLine);
                        song.name = sr.ReadLine();
                        song.songPages = sr.ReadLine().Split(',').ToList();
                        song.length = sr.ReadLine();
                        song.band = GetBandFromID(info, sr.ReadLine());
                        song.notes = sr.ReadLine();
                        songs.Add(song);
                    }
                }
                while (readLine != "end");
                sr.Close();

                sr = new StreamReader(@"Albums.txt");
                do
                {
                    readLine = sr.ReadLine();
                    if (readLine != "end")
                    {
                        Album album = new Album();
                        album.albumID = Convert.ToInt32(readLine);
                        album.name = sr.ReadLine();
                        album.albumPages = sr.ReadLine().Split(',').ToList();
                        album.band = GetBandFromID(info, sr.ReadLine());
                        album.songs = GetSongsArrayFromID(info, sr.ReadLine().Split(',').ToList());
                        album.notes = sr.ReadLine();
                        albums.Add(album);
                    }
                }
                while (readLine != "end");
                sr.Close();

                sr = new StreamReader(@"Gigs.txt");
                do
                {
                    readLine = sr.ReadLine();
                    if (readLine != "end")
                    {
                        Gig gig = new Gig();
                        gig.gigID = Convert.ToInt32(readLine);
                        gig.bands = GetBandsArrayFromID(info, sr.ReadLine().Split(',').ToList());
                        gig.supports = GetBandsArrayFromID(info, sr.ReadLine().Split(',').ToList());
                        gig.tour = sr.ReadLine();
                        gig.album = GetAlbumFromID(info, sr.ReadLine());

                        string[] date = sr.ReadLine().Split('/');
                        gig.date = new DateOnly(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));

                        gig.setlistLink = sr.ReadLine();
                        gig.gigPages = sr.ReadLine().Split('/').ToList();
                        gig.venue = GetVenueFromID(info, sr.ReadLine());
                        gig.setlist = GetSongsArrayFromID(info, sr.ReadLine().Split(',').ToList());
                        gig.encore = GetSongsArrayFromID(info, sr.ReadLine().Split(',').ToList());
                        gig.notes = sr.ReadLine();
                        gigs.Add(gig);
                    }
                }
                while (readLine != "end");
                sr.Close();

                info.gigs = info.gigs.OrderBy(o => o.date).ToList();
                info.bands = info.bands.OrderBy(o => o.name).ToList();
                info.artists = info.artists.OrderBy(o => o.name).ToList();
                info.albums = info.albums.OrderBy(o => o.name).ToList();
                info.songs = info.songs.OrderBy(o => o.name).ToList();
                info.venues = info.venues.OrderBy(o => o.city).ToList();
            }
        }
    }
}