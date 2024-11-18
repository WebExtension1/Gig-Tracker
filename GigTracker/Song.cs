namespace GigTracker
{
    public partial class GigTracker
    {
        public class Song
        {
            public int songID;
            public string name;
            public List<string> songPages;
            public string length;
            public Band band;                                       // Band
            public string notes;
            public int amountCheck;
        }
    }
}