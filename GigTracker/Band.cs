namespace GigTracker
{
    public partial class GigTracker
    {
        public class Band
        {
            public int bandID;
            public string name;
            public List<Artist> members = new List<Artist>();       // Artist
            public List<string> bandPages = new List<string>();
            public string notes;
        }
    }
}