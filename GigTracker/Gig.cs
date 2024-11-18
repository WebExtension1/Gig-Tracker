namespace GigTracker
{
    public partial class GigTracker
    {
        public class Gig
        {
            public int gigID;
            public List<Band> bands;                                // Band
            public List<Band> supports = new List<Band>();          // Band
            public string tour;
            public Album album;                                     // Album
            public DateOnly date;
            public string setlistLink;
            public List<string> gigPages;
            public Venue venue;                                     // Venue
            public List<Song> setlist = new List<Song>();           // Song
            public List<Song> encore = new List<Song>();            // Song
            public string notes;
        }
    }
}