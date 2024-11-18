namespace GigTracker
{
    public partial class GigTracker
    {
        public class Album
        {
            public int albumID;
            public string name;
            public List<string> albumPages;
            public Band band;                                       // Band
            public List<Song> songs;                                // Song
            public string notes;
            public int amountCheck;
        }
    }
}