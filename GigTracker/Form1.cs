using System.Media;
using System.Diagnostics;
using static GigTracker.GigTracker;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Security.Policy;
using System.Security.AccessControl;

namespace GigTracker
{
    public partial class GigTracker : Form
    {
        public Info info = new Info();
        Panel panel = new Panel();
        public string chromeLink = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";

        public GigTracker()
        {
            InitializeComponent();

            panel = new Panel();
            panel.Size = new Size(885, 603);
            panel.Location = new Point(336, 38);
            panel.AutoScroll = true;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.KeyDown += Shortcut;
            Controls.Add(panel);

            SelectProfile();
        }





        public void DisplayGigInfo(Gig gig)
        {
            panel.Controls.Clear();
            panel.Tag = gig;

            int YDepth = 0;
            Label label;

            List<Album> albums = new List<Album>();

            ToolTip toolTip = new ToolTip();

            foreach (Band band in gig.bands)
            {
                label = new Label();
                label.Font = new Font("Arial", 30, FontStyle.Bold);
                label.Text = band.name;
                label.Tag = band;
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += BandClicked;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open band info");
                panel.Controls.Add(label);
                YDepth += 50;
            }

            label = new Label();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Text = gig.date.ToString();
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            label.Click += DateClicked;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open all dates info");
            panel.Controls.Add(label);
            YDepth += 35;

            if (gig.tour != "")
            {
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"Tour: {gig.tour}";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                if (gig.album.name != null)
                {
                    label.Tag = gig.album;
                    label.Click += AlbumClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open toured album info");
                }
                panel.Controls.Add(label);
                YDepth += 20;
            }

            if (gig.venue.venue != null)
            {
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"Venue: {gig.venue.venue}, {gig.venue.city}";
                label.Tag = gig.venue;
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += VenueClicked;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open venue info");
                panel.Controls.Add(label);
                YDepth += 20;
            }

            if (gig.notes != "")
            {
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"Notes: {gig.notes}";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                panel.Controls.Add(label);
                YDepth += 20;
            }

            foreach (string link in gig.gigPages)
            {
                if (link != "")
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = link.Split(':')[0];
                    label.Tag = link.Split(':')[link.Split(':').Count() - 1].Substring(2);
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += PageClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open hyperlinked page");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
            }

            YDepth += 15;

            if (gig.supports.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Supports";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllBandInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all band info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Band band in gig.supports)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = band.name;
                    label.Tag = band;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += BandClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open band info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }

            if (gig.setlist.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Setlist";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                if (gig.setlistLink != "")
                {
                    label.Tag = gig.setlistLink;
                    label.Click += PageClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open the setlist");
                }
                panel.Controls.Add(label);
                YDepth += 40;

                int songIndex = 1;
                foreach (Song song in gig.setlist)
                {
                    Panel songPanel = GetFormattedSong(song, gig.bands[0], songIndex, true);
                    songPanel.Location = new Point(-1, YDepth);
                    panel.Controls.Add(songPanel);
                    YDepth += songPanel.Size.Height - 1;

                    Album album = GetAlbumFromSong(song, gig.bands[0]);
                    albums.Remove(album);
                    albums.Add(album);
                    songIndex++;
                }

                if (gig.encore.Count > 0)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Bold);
                    label.Text = "Encore";
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    if (gig.setlistLink != "")
                    {
                        label.Tag = gig.setlistLink;
                        label.Click += PageClicked;
                        label.Cursor = Cursors.Hand;
                        toolTip.SetToolTip(label, "Click to open the setlist");
                    }
                    panel.Controls.Add(label);
                    YDepth += 20;

                    foreach (Song song in gig.encore)
                    {
                        Panel songPanel = GetFormattedSong(song, gig.bands[0], songIndex, true);
                        songPanel.Location = new Point(-1, YDepth);
                        panel.Controls.Add(songPanel);
                        YDepth += songPanel.Size.Height - 1;

                        Album album = GetAlbumFromSong(song, gig.bands[0]);
                        albums.Remove(album);
                        albums.Add(album);
                        songIndex++;
                    }
                    YDepth += 20;
                }
            }

            if (albums.Count > 0)
            {
                if (albums[0] != null)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 20, FontStyle.Bold);
                    label.Text = "Albums";
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += DisplayAllAlbumInfo;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open all album info");
                    panel.Controls.Add(label);
                    YDepth += 40;

                    foreach (Album album in info.albums)
                    {
                        album.amountCheck = 0;
                    }

                    foreach (Song song in gig.setlist)
                    {
                        Album album = GetAlbumFromSong(song, gig.bands[0]);
                        album.amountCheck++;
                        albums.Remove(album);
                        albums.Add(album);
                    }

                    foreach (Song song in gig.encore)
                    {
                        Album album = GetAlbumFromSong(song, gig.bands[0]);
                        album.amountCheck++;
                        albums.Remove(album);
                        albums.Add(album);
                    }

                    albums = albums.OrderByDescending(o => o.amountCheck).ToList();
                    Panel albumPanel = new Panel();
                    albumPanel.Size = new Size(0, 0);
                    albumPanel.Location = new Point(-10, -10);
                    int plays = 10000, tempYDepth = 15;
                    Label playsLabel = new Label();
                    foreach (Album album in albums)
                    {
                        if (album.amountCheck < plays)
                        {
                            panel.Controls.Add(albumPanel);
                            YDepth += albumPanel.Height - 1;
                            albumPanel = new Panel();
                            albumPanel.BorderStyle = BorderStyle.FixedSingle;
                            albumPanel.Location = new Point(-1, YDepth);
                            albumPanel.Size = new Size(867, 45);
                            tempYDepth = 15;
                            plays = album.amountCheck;
                            playsLabel = new Label();
                            playsLabel.AutoSize = true;
                            playsLabel.Text = plays.ToString();
                            playsLabel.Font = new Font("Arial", 10, FontStyle.Bold);
                            playsLabel.Location = new Point(16, 16);
                            albumPanel.Controls.Add(playsLabel);
                        }
                        else
                        {
                            playsLabel.Location = new Point(15, playsLabel.Location.Y + 8);
                            albumPanel.Size = new Size(867, albumPanel.Size.Height + 16);
                        }
                        label = new Label();
                        label.Tag = album;
                        label.Click += AlbumClicked;
                        label.Cursor = Cursors.Hand;
                        toolTip.SetToolTip(label, "Click to open album info");
                        label.Text = album.name;
                        label.AutoSize = true;
                        label.Location = new Point(60, tempYDepth);
                        tempYDepth += 18;
                        albumPanel.Controls.Add(label);
                    }
                    panel.Controls.Add(albumPanel);
                    albumPanel.Size = new Size(867, albumPanel.Size.Height + 16);
                }
            }
        }

        public void DisplayBandInfo(Band band)
        {
            panel.Controls.Clear();
            panel.Tag = band;

            int YDepth = 0;

            ToolTip toolTip = new ToolTip();

            Label label = new Label();
            label.Font = new Font("Arial", 30, FontStyle.Bold);
            label.Text = band.name;
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            label.Click += DisplayAllBandInfo;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open all band info");
            panel.Controls.Add(label);
            YDepth += 50;

            if (band.bandPages[0] != "")
            {
                foreach (string link in band.bandPages)
                {
                    if (link != "")
                    {
                        label = new Label();
                        label.Font = new Font("Arial", 10, FontStyle.Regular);
                        label.Text = link.Split(':')[0];
                        label.Tag = link.Split(':')[link.Split(':').Count() - 1].Substring(2);
                        label.Location = new Point(0, YDepth);
                        label.AutoSize = true;
                        label.Click += PageClicked;
                        label.Cursor = Cursors.Hand;
                        toolTip.SetToolTip(label, "Click to open hyperlinked page");
                        panel.Controls.Add(label);
                        YDepth += 20;
                    }
                }
                YDepth += 20;
            }

            if (band.notes != "")
            {
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"Notes: {band.notes}";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                panel.Controls.Add(label);
                YDepth += 20;
            }

            List<Gig> gigs = new List<Gig>();
            foreach (Gig gig in info.gigs)
            {
                foreach (Band headlineBand in gig.bands)
                {
                    if (band == headlineBand)
                    {
                        gigs.Add(gig);
                    }
                }
                foreach (Band supportBand in gig.supports)
                {
                    if (band == supportBand)
                    {
                        gigs.Add(gig);
                    }
                }
            }

            if (gigs.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Gigs";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllGigInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all gig info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Gig gig in gigs)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{gig.bands[0].name} - {gig.date}";
                    label.Tag = gig;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += GigClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open gig info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }

            if (band.members.Count > 0)
            {
                if (band.members[0].name != null)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 20, FontStyle.Bold);
                    label.Text = "Members";
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += DisplayAllArtistInfo;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open all artist info");
                    panel.Controls.Add(label);
                    YDepth += 35;
                }
                foreach (Artist artist in band.members)
                {
                    if (artist.name != null)
                    {
                        label = new Label();
                        label.Font = new Font("Arial", 10, FontStyle.Regular);
                        label.Text = $"{artist.name}";
                        label.Tag = artist;
                        label.Location = new Point(0, YDepth);
                        label.AutoSize = true;
                        label.Click += ArtistClicked;
                        label.Cursor = Cursors.Hand;
                        toolTip.SetToolTip(label, "Click to open artist info");
                        panel.Controls.Add(label);
                        YDepth += 20;
                    }
                }
                YDepth += 20;
            }

            if (gigs.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Venues";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllVenueInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all venue info");
                panel.Controls.Add(label);
                YDepth += 35;

                List<Venue> venues = new List<Venue>();

                foreach (Gig gig in gigs)
                {
                    venues.Remove(gig.venue);
                    venues.Add(gig.venue);
                }

                foreach (Venue venue in venues)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{venue.venue}, {venue.city}";
                    label.Tag = venue;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += VenueClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open venue info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }

            if (gigs.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Albums";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllAlbumInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all album info");
                panel.Controls.Add(label);
                YDepth += 40;

                foreach (Album album in info.albums)
                {
                    album.amountCheck = 0;
                }

                List<Album> albums = new List<Album>();
                foreach (Gig gig in info.gigs)
                {
                    if (gig.bands.Contains(band))
                    {
                        foreach (Song song in gig.setlist)
                        {
                            Album album = GetAlbumFromSong(song, band);
                            album.amountCheck++;
                            albums.Remove(album);
                            albums.Add(album);
                        }
                        foreach (Song song in gig.encore)
                        {
                            Album album = GetAlbumFromSong(song, band);
                            album.amountCheck++;
                            albums.Remove(album);
                            albums.Add(album);
                        }
                    }
                }

                albums = albums.OrderByDescending(o => o.amountCheck).ToList();
                Panel albumPanel = new Panel();
                albumPanel.Size = new Size(0, 0);
                albumPanel.Location = new Point(-10, -10);
                int plays = 10000, tempYDepth = 15;
                Label playsLabel = new Label();
                foreach (Album album in albums)                                                                      ///////////////////////
                {
                    if (album.amountCheck < plays)
                    {
                        panel.Controls.Add(albumPanel);
                        YDepth += albumPanel.Height - 1;
                        albumPanel = new Panel();
                        albumPanel.BorderStyle = BorderStyle.FixedSingle;
                        albumPanel.Location = new Point(-1, YDepth);
                        albumPanel.Size = new Size(867, 45);
                        tempYDepth = 15;
                        plays = album.amountCheck;
                        playsLabel = new Label();
                        playsLabel.AutoSize = true;
                        playsLabel.Text = plays.ToString();
                        playsLabel.Font = new Font("Arial", 10, FontStyle.Bold);
                        playsLabel.Location = new Point(16, 16);
                        albumPanel.Controls.Add(playsLabel);
                    }
                    else
                    {
                        playsLabel.Location = new Point(15, playsLabel.Location.Y + 8);
                        albumPanel.Size = new Size(867, albumPanel.Size.Height + 16);
                    }
                    label = new Label();
                    label.Tag = album;
                    label.Click += AlbumClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open album info");
                    label.Text = album.name;
                    label.AutoSize = true;
                    label.Location = new Point(60, tempYDepth);
                    tempYDepth += 18;
                    albumPanel.Controls.Add(label);
                }
                panel.Controls.Add(albumPanel);
                YDepth += albumPanel.Height + 14;
            }

            if (gigs.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Songs";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllSongInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all song info");
                panel.Controls.Add(label);
                YDepth += 40;

                foreach (Song song in info.songs)
                {
                    song.amountCheck = 0;
                }

                List<Song> songs = new List<Song>();
                foreach (Gig gig in info.gigs)
                {
                    if (gig.bands.Contains(band))
                    {
                        foreach (Song song in gig.setlist)
                        {
                            song.amountCheck++;
                            songs.Remove(song);
                            songs.Add(song);
                        }
                        foreach (Song song in gig.encore)
                        {
                            song.amountCheck++;
                            songs.Remove(song);
                            songs.Add(song);
                        }
                    }
                }

                songs = songs.OrderByDescending(o => o.amountCheck).ToList();
                Panel songPanel = new Panel();
                songPanel.Size = new Size(0, 0);
                songPanel.Location = new Point(-10, -10);
                int plays = 10000, tempYDepth = 15;
                Label playsLabel = new Label();
                foreach (Song song in songs)                                                            ///////////////////////////////////
                {
                    if (song.amountCheck < plays)
                    {
                        panel.Controls.Add(songPanel);
                        YDepth += songPanel.Height - 1;
                        songPanel.Height += 14;
                        songPanel = new Panel();
                        songPanel.BorderStyle = BorderStyle.FixedSingle;
                        songPanel.Location = new Point(-1, YDepth);
                        songPanel.Size = new Size(867, 45);
                        tempYDepth = 15;
                        plays = song.amountCheck;
                        playsLabel = new Label();
                        playsLabel.AutoSize = true;
                        playsLabel.Text = plays.ToString();
                        playsLabel.Font = new Font("Arial", 10, FontStyle.Bold);
                        playsLabel.Location = new Point(16, 16);
                        songPanel.Controls.Add(playsLabel);
                    }
                    else
                    {
                        playsLabel.Location = new Point(15, playsLabel.Location.Y + 8);
                        songPanel.Size = new Size(867, songPanel.Size.Height + 18);
                    }
                    label = new Label();
                    label.Tag = song;
                    label.Click += SongClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open song info");
                    label.Text = song.name;
                    label.AutoSize = true;
                    label.Location = new Point(60, tempYDepth);
                    tempYDepth += 18;
                    songPanel.Controls.Add(label);
                }
                panel.Controls.Add(songPanel);
                YDepth += songPanel.Height + 14;
            }
        }

        public void DisplayArtistInfo(Artist artist)
        {
            panel.Controls.Clear();
            panel.Tag = artist;

            int YDepth = 0;

            ToolTip toolTip = new ToolTip();

            Label label = new Label();
            label.Font = new Font("Arial", 30, FontStyle.Bold);
            label.Text = artist.name;
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            label.Click += DisplayAllArtistInfo;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open all artist info");
            panel.Controls.Add(label);
            YDepth += 50;

            foreach (string link in artist.artistPages)
            {
                if (link != "")
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = link.Split(':')[0];
                    label.Tag = link.Split(':')[link.Split(':').Count() - 1].Substring(2);
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += PageClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open hyperlinked page");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
            }

            if (artist.notes != "")
            {
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"Notes: {artist.notes}";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                panel.Controls.Add(label);
                YDepth += 20;
            }
            YDepth += 20;

            List<Band> bands = new List<Band>();
            foreach (Band band in info.bands)
            {
                if (band.members.Contains(artist))
                {
                    bands.Add(band);
                }
            }
            if (bands.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Bands";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllBandInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all band info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Band band in bands)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{band.name}";
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += BandClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open band info");
                    label.Tag = band;
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }
        }

        public void DisplayAlbumInfo(Album album)
        {
            panel.Controls.Clear();
            panel.Tag = album;

            int YDepth = 0;

            ToolTip toolTip = new ToolTip();

            Label label = new Label();
            label.Font = new Font("Arial", 30, FontStyle.Bold);
            label.Text = album.name;
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            label.Click += DisplayAllAlbumInfo;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open all album info");
            panel.Controls.Add(label);
            YDepth += 50;

            label = new Label();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Text = album.band.name;
            label.Tag = album.band;
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            label.Click += BandClicked;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open band info");
            panel.Controls.Add(label);
            YDepth += 35;

            foreach (string link in album.albumPages)
            {
                if (link != "")
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = link.Split(':')[0];
                    label.Tag = link.Split(':')[link.Split(':').Count() - 1].Substring(2);
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += PageClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open hyperlinked page");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
            }
            YDepth += 20;

            if (album.songs[0].name != null)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Songs";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllSongInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all song info");
                panel.Controls.Add(label);
                YDepth += 40;

                int songIndex = 1;
                foreach (Song song in album.songs)
                {
                    Panel songPanel = GetFormattedSong(song, song.band, songIndex, false);
                    songPanel.Location = new Point(-1, YDepth);
                    panel.Controls.Add(songPanel);
                    YDepth += songPanel.Size.Height - 1;
                    songIndex++;
                }
                YDepth += 20;
            }

            List<Gig> gigs = GetGigsFromAlbum(album);
            if (gigs != null)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Gigs";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllGigInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all gig info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Gig gig in gigs)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{gig.bands[0].name} - {gig.date}";
                    label.Tag = gig;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += GigClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open gig info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }

            if (gigs != null)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Venues";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllVenueInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all venue info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Gig gig in gigs)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{gig.venue.venue}, {gig.venue.city}";
                    label.Tag = gig.venue;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += VenueClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open venue info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }
        }

        public void DisplaySongInfo(Song song)
        {
            panel.Controls.Clear();
            panel.Tag = song;

            int YDepth = 0;

            ToolTip toolTip = new ToolTip();

            Label label = new Label();
            label.Font = new Font("Arial", 30, FontStyle.Bold);
            label.Text = song.name;
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            label.Click += DisplayAllSongInfo;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open all song info");
            panel.Controls.Add(label);
            YDepth += 50;

            label = new Label();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Text = song.band.name;
            label.Tag = song.band;
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            label.Click += BandClicked;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open band info");
            panel.Controls.Add(label);
            YDepth += 35;

            label = new Label();
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.Text = song.length;
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            panel.Controls.Add(label);
            YDepth += 20;

            if (song.notes != "")
            {
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"Notes: {song.notes}";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                panel.Controls.Add(label);
                YDepth += 20;
            }

            foreach (string link in song.songPages)
            {
                if (link != "")
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = link.Split(':')[0];
                    label.Tag = link.Split(':')[link.Split(':').Count() - 1].Substring(2);
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += PageClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open hyperlinked page");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
            }
            YDepth += 20;

            List<Gig> gigs = new List<Gig>();
            foreach (Gig gig in info.gigs)
            {
                if (gig.setlist.Contains(song) || gig.encore.Contains(song))
                {
                    gigs.Remove(gig);
                    gigs.Add(gig);
                }
            }
            if (gigs.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Gigs";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllGigInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all gig info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Gig gig in gigs)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{gig.bands[0].name} - {gig.date}";
                    label.Tag = gig;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += GigClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open gig info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }

            List<Album> albums = new List<Album>();
            foreach (Album album in info.albums)
            {
                if (album.songs.Contains(song))
                {
                    albums.Remove(album);
                    albums.Add(album);
                }
            }
            if (albums.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Albums";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllAlbumInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all album info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Album album in albums)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{album.name}";
                    label.Tag = album;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += AlbumClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open album info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }

            List<Venue> venues = new List<Venue>();
            foreach (Gig gig in gigs)
            {
                venues.Remove(gig.venue);
                venues.Add(gig.venue);
            }
            if (venues.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Venues";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllVenueInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all venue info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Venue venue in venues)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{venue.venue} - {venue.city}";
                    label.Tag = venue;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += VenueClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open venue info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }
        }

        public void DisplayVenueInfo(Venue venue)
        {
            panel.Controls.Clear();
            panel.Tag = venue;

            int YDepth = 0;

            ToolTip toolTip = new ToolTip();

            Label label = new Label();
            label.Font = new Font("Arial", 30, FontStyle.Bold);
            label.Text = venue.venue;
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            label.Click += CityClicked;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open all venue info");
            panel.Controls.Add(label);
            YDepth += 50;

            foreach (string link in venue.venuePages)
            {
                if (link != "")
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = link.Split(':')[0];
                    label.Tag = link.Split(':')[link.Split(':').Count() - 1].Substring(2);
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += PageClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open hyperlinked page");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
            }
            YDepth += 20;

            label = new Label();
            label.Font = new Font("Arial", 30, FontStyle.Bold);
            label.Text = venue.city;
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            label.Click += CityClicked;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open all city info");
            panel.Controls.Add(label);
            YDepth += 50;

            foreach (string link in venue.cityPages)
            {
                if (link != "")
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = link.Split(':')[0];
                    label.Tag = link.Split(':')[link.Split(':').Count() - 1].Substring(2);
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += PageClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open hyperlinked page");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
            }

            if (venue.notes != "")
            {
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"Notes: {venue.notes}";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                panel.Controls.Add(label);
                YDepth += 20;
            }
            YDepth += 20;

            List<Gig> gigs = new List<Gig>();
            foreach (Gig gig in info.gigs)
            {
                if (gig.venue == venue)
                {
                    gigs.Remove(gig);
                    gigs.Add(gig);
                }
            }
            if (gigs.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Gigs";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllGigInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all gigs info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Gig gig in gigs)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{gig.bands[0].name} - {gig.date}";
                    label.Tag = gig;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += GigClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open gig info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }

            List<Band> bands = new List<Band>();
            foreach (Gig gig in gigs)
            {
                foreach (Band band in gig.bands)
                {
                    bands.Remove(band);
                    bands.Add(band);
                }
                foreach (Band band in gig.supports)
                {
                    bands.Remove(band);
                    bands.Add(band);
                }
            }
            if (bands.Count > 0)
            {
                label = new Label();
                label.Font = new Font("Arial", 20, FontStyle.Bold);
                label.Text = "Bands";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += DisplayAllBandInfo;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open all band info");
                panel.Controls.Add(label);
                YDepth += 35;

                foreach (Band band in bands)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = $"{band.name}";
                    label.Tag = band;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    label.Click += BandClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open band info");
                    panel.Controls.Add(label);
                    YDepth += 20;
                }
                YDepth += 20;
            }
        }

        public void DisplayAllGigInfo(object sender, EventArgs e)
        {
            panel.Controls.Clear();
        }

        public void DisplayAllBandInfo(object sender, EventArgs e)
        {
            panel.Controls.Clear();
        }

        public void DisplayAllArtistInfo(object sender, EventArgs e)
        {
            panel.Controls.Clear();
        }

        public void DisplayAllAlbumInfo(object sender, EventArgs e)
        {
            panel.Controls.Clear();
        }

        public void DisplayAllSongInfo(object sender, EventArgs e)
        {
            panel.Controls.Clear();
        }

        public void DisplayAllVenueInfo(object sender, EventArgs e)
        {
            panel.Controls.Clear();
        }

        public void DisplayTopInfo()
        {
            panel.Controls.Clear();

            int YDepth = 0;

            Label label = new Label();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Text = "Top";
            label.Location = new Point(0, YDepth);
            label.AutoSize = true;
            panel.Controls.Add(label);
            YDepth += 45;
        }





        public void CreateNew()
        {
            panel.Controls.Clear();

            Panel editPanel = new Panel();
            int maxID = 0;
            switch (info.selected)
            {
                case 1:
                    Gig gig = new Gig();
                    foreach (Gig gigMax in info.gigs)
                    {
                        if (gigMax.gigID > maxID)
                        {
                            maxID = gigMax.gigID;
                        }
                    }
                    gig.gigID = maxID + 1;
                    gig.date = DateOnly.Parse($"{DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}");
                    editPanel = DisplayGigEdit(gig);
                    break;
                case 2:
                    Band band = new Band();
                    foreach (Band bandMax in info.bands)
                    {
                        if (bandMax.bandID > maxID)
                        {
                            maxID = bandMax.bandID;
                        }
                    }
                    band.bandID = maxID + 1;
                    editPanel = DisplayBandEdit(band);
                    break;
                case 3:
                    Artist artist = new Artist();
                    foreach (Artist artistMax in info.artists)
                    {
                        if (artistMax.artistID > maxID)
                        {
                            maxID = artistMax.artistID;
                        }
                    }
                    artist.artistID = maxID + 1;
                    editPanel = DisplayArtistEdit(artist);
                    break;
                case 4:
                    Album album = new Album();
                    foreach (Album albumMax in info.albums)
                    {
                        if (albumMax.albumID > maxID)
                        {
                            maxID = albumMax.albumID;
                        }
                    }
                    album.albumID = maxID + 1;
                    editPanel = DisplayAlbumEdit(album);
                    break;
                case 5:
                    Song song = new Song();
                    foreach (Song songMax in info.songs)
                    {
                        if (songMax.songID > maxID)
                        {
                            maxID = songMax.songID;
                        }
                    }
                    song.songID = maxID + 1;
                    editPanel = DisplaySongEdit(song);
                    break;
                case 6:
                    Venue venue = new Venue();
                    foreach (Venue venueMax in info.venues)
                    {
                        if (venueMax.venueID > maxID)
                        {
                            maxID = venueMax.venueID;
                        }
                    }
                    venue.venueID = maxID + 1;
                    editPanel = DisplayVenueEdit(venue);
                    break;
            }

            panel.Controls.Add(editPanel);
        }

        public void EditItem()
        {
            if (displayList.SelectedIndex != -1)
            {
                panel.Controls.Clear();

                Panel editPanel = new Panel();
                if (info.selected == 1)
                {
                    editPanel = DisplayGigEdit((Gig)panel.Tag);
                }
                else if (info.selected == 2)
                {
                    editPanel = DisplayBandEdit((Band)panel.Tag);
                }
                else if (info.selected == 3)
                {
                    editPanel = DisplayArtistEdit((Artist)panel.Tag);
                }
                else if (info.selected == 4)
                {
                    editPanel = DisplayAlbumEdit((Album)panel.Tag);
                }
                else if (info.selected == 5)
                {
                    editPanel = DisplaySongEdit((Song)panel.Tag);
                }
                else if (info.selected == 6)
                {
                    editPanel = DisplayVenueEdit((Venue)panel.Tag);
                }
                panel.Controls.Add(editPanel);
            }
        }

        public Panel DisplayGigEdit(Gig gig)
        {
            Panel editPanel = new Panel();
            editPanel.Size = new Size(855, 573);
            editPanel.Location = new Point(14, 10);
            editPanel.BorderStyle = BorderStyle.FixedSingle;
            editPanel.Tag = gig;

            Label label = new Label();
            label.Text = "Date";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(15, 36);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            TextBox date = new TextBox();
            date.Size = new Size(200, 25);
            date.Location = new Point(63, 35);
            date.Text = gig.date.ToString();
            editPanel.Controls.Add(date);

            label = new Label();
            label.Text = "Venue";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(15, 56);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            ComboBox venue = new ComboBox();
            foreach (Venue venueAdd in info.venues)
            {
                venue.Items.Add($"{venueAdd.venue}, {venueAdd.city}");
                if (gig.venue != null)
                {
                    if (gig.venue == venueAdd)
                    {
                        venue.SelectedIndex = venue.Items.Count - 1;
                    }
                }
            }
            venue.Location = new Point(63, 55);
            venue.Size = new Size(200, 25);
            editPanel. Controls.Add(venue);

            label = new Label();
            label.Text = "ID: " + gig.gigID.ToString();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Location = new Point(15, 5);
            label.AutoSize = true;
            editPanel.Controls.Add(label);
            
            label  = new Label();
            label.Text = "Bands";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(64, 105);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            ListBox headliners = new ListBox();
            headliners.SelectionMode = SelectionMode.MultiSimple;
            foreach (Band band in info.bands)
            {
                headliners.Items.Add(band.name);
                if (gig.bands != null)
                {
                    if (gig.bands.Contains(band))
                    {
                        headliners.SelectedIndex = headliners.Items.Count - 1;
                    }
                }
            }
            headliners.Location = new Point(64, 125);
            headliners.Size = new Size(200, 210);
            editPanel.Controls.Add(headliners);

            label = new Label();
            label.Text = "Supports";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(64, 330);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            ListBox supports = new ListBox();
            supports.SelectionMode = SelectionMode.MultiSimple;
            foreach (Band band in info.bands)
            {
                supports.Items.Add(band.name);
                if (gig.bands != null)
                {
                    if (gig.supports.Contains(band))
                    {
                        supports.SelectedIndex = supports.Items.Count - 1;
                    }
                }
            }
            supports.Location = new Point(64, 350);
            supports.Size = new Size(200, 210);
            editPanel.Controls.Add(supports);

            label = new Label();
            label.Text = "Setlist";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(328, 150);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            ListBox setlist = new ListBox();
            setlist.Location = new Point(328, 170);
            setlist.Size = new Size(210, 390);
            setlist.Click += SongRemovedFromList;
            editPanel.Controls.Add(setlist);

            label = new Label();
            label.Text = "Encore";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(592, 150);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            ListBox encore = new ListBox();
            encore.Location = new Point(592, 170);
            encore.Size = new Size(210, 390);
            encore.Click += SongRemovedFromList;
            editPanel.Controls.Add(encore);

            Button setlistButton = new Button();
            setlistButton.Text = "Add to Setlist";
            setlistButton.Location = new Point(328, 125);
            setlistButton.Size = new Size(100, 25);
            setlistButton.Click += AddButtonClicked;
            editPanel.Controls.Add(setlistButton);

            Button encoreButton = new Button();
            encoreButton.Text = "Add to Encore";
            encoreButton.Location = new Point(700, 125);
            encoreButton.Size = new Size(100, 25);
            encoreButton.Click += AddButtonClicked;
            editPanel.Controls.Add(encoreButton);

            ComboBox songComboBox = new ComboBox();
            songComboBox.Location = new Point(428, 127);
            songComboBox.Size = new Size(272, 25);
            foreach (Song song in info.songs)
            {
                songComboBox.Items.Add(song.name);
            }

            List<Song> setlistSongs = new List<Song>();
            foreach (Song song in gig.setlist)
            {
                setlist.Items.Add(song.name);
                setlistSongs.Add(song);
            }
            setlist.Tag = setlistSongs;

            List<Song> encoreSongs = new List<Song>();
            foreach (Song song in gig.encore)
            {
                encore.Items.Add(song.name);
                encoreSongs.Add(song);
            }
            encore.Tag = encoreSongs;

            editPanel.Controls.Add(songComboBox);

            setlistButton.Tag = new Tuple<ListBox, ComboBox>(setlist, songComboBox);
            encoreButton.Tag = new Tuple<ListBox, ComboBox>(encore, songComboBox);

            label = new Label();
            label.Text = "Tour name";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(265, 36);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            TextBox tourName = new TextBox();
            tourName.Size = new Size(200, 25);
            tourName.Location = new Point(345, 35);
            tourName.Text = gig.tour;
            editPanel.Controls.Add(tourName);

            label = new Label();
            label.Text = "Tour album";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(265, 56);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            ComboBox tourAlbum = new ComboBox();
            foreach (Album album in info.albums)
            {
                tourAlbum.Items.Add($"{album.name}");
                if (gig.album != null)
                {
                    if (gig.album == album)
                    {
                        tourAlbum.SelectedIndex = tourAlbum.Items.Count - 1;
                    }
                }
            }
            tourAlbum.Location = new Point(345, 55);
            tourAlbum.Size = new Size(200, 25);
            editPanel.Controls.Add(tourAlbum);

            label = new Label();
            label.Text = "Notes";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(15, 76);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            TextBox notes = new TextBox();
            notes.Size = new Size(740, 25);
            notes.Location = new Point(63, 75);
            notes.Text = gig.notes;
            editPanel.Controls.Add(notes);

            TextBox setlistLink = new TextBox();
            setlistLink.Size = new Size(200, 25);
            setlistLink.Location = new Point(603, 35);
            setlistLink.Text = gig.setlistLink;
            editPanel.Controls.Add(setlistLink);

            label = new Label();
            label.Text = "Setlist";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(550, 36);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            TextBox pages = new TextBox();
            pages.Size = new Size(200, 25);
            pages.Location = new Point(603, 56);
            if (gig.gigPages != null)
            {
                pages.Text = gig.gigPages[0];
            }
            editPanel.Controls.Add(pages);

            label = new Label();
            label.Text = "Links";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(550, 55);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            List<object> items = new List<object>();
            items.Add(gig);
            items.Add(venue);
            items.Add(date);
            items.Add(notes);
            items.Add(tourName);
            items.Add(tourAlbum);
            items.Add(setlistLink);
            items.Add(pages);
            items.Add(headliners);
            items.Add(supports);
            items.Add(setlist);
            items.Add(encore);

            Button saveButton = new Button();
            saveButton.Location = new Point(773, 5);
            saveButton.Text = "Save";
            saveButton.AutoSize = true;
            saveButton.BackColor = Color.LightGray;
            saveButton.Tag = items;
            saveButton.Click += NewGigAdded;
            editPanel.Controls.Add(saveButton);

            return editPanel;
        }

        public void NewGigAdded(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            List<object> items = (List<object>)button.Tag;
            Gig gig = (Gig)items[0];

            ComboBox comboBox = (ComboBox)items[1];
            gig.venue = info.venues[comboBox.SelectedIndex];

            TextBox textBox = (TextBox)items[2];
            gig.date = DateOnly.Parse(textBox.Text);

            textBox = (TextBox)items[3];
            gig.notes = textBox.Text;

            textBox = (TextBox)items[4];
            gig.tour = textBox.Text;

            comboBox = (ComboBox)items[5];
            if (comboBox.SelectedIndex != -1)
            {
                gig.album = info.albums[comboBox.SelectedIndex];
            }

            textBox = (TextBox)items[6];
            gig.setlistLink = textBox.Text;

            textBox = (TextBox)items[7];
            gig.gigPages = new List<string>();
            gig.gigPages.Add(textBox.Text);

            ListBox headliners = (ListBox)items[8];
            foreach (int index in headliners.SelectedIndices)
            {
                gig.bands = new List<Band>();
                gig.bands.Add(info.bands[index]);
            }

            ListBox supports = (ListBox)items[9];
            foreach (int index in supports.SelectedIndices)
            {
                gig.supports = new List<Band>();
                gig.supports.Add(info.bands[index]);
            }

            ListBox setlist = (ListBox)items[10];
            List<Song> setlistSongs = (List<Song>)setlist.Tag;
            gig.setlist = new List<Song>();
            foreach (Song song in setlistSongs)
            {
                gig.setlist.Add(song);
            }

            ListBox encore = (ListBox)items[11];
            List<Song> encoreSongs = (List<Song>)encore.Tag;
            gig.encore = new List<Song>();
            foreach (Song song in encoreSongs)
            {
                gig.encore.Add(song);
            }

            if (!info.gigs.Contains(gig))
            {
                info.gigs.Add(gig);
            }
            gigsMenuItem_Click(null, null);
        }

        public Panel DisplayBandEdit(Band band)
        {
            Panel editPanel = new Panel();
            editPanel.Size = new Size(855, 573);
            editPanel.Location = new Point(14, 10);
            editPanel.BorderStyle = BorderStyle.FixedSingle;
            editPanel.Tag = band;

            Label label = new Label();
            label.Text = "ID: " + band.bandID.ToString();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Location = new Point(15, 5);
            label.AutoSize = true;
            editPanel.Controls.Add(label);





            List<object> items = new List<object>();
            items.Add(band);

            Button saveButton = new Button();
            saveButton.Location = new Point(773, 5);
            saveButton.Text = "Save";
            saveButton.AutoSize = true;
            saveButton.BackColor = Color.LightGray;
            saveButton.Tag = items;
            saveButton.Click += NewBandAdded;
            editPanel.Controls.Add(saveButton);

            return editPanel;
        }

        public void NewBandAdded(object sender, EventArgs e)
        {

        }

        public Panel DisplayArtistEdit(Artist artist)
        {
            Panel editPanel = new Panel();
            editPanel.Size = new Size(855, 573);
            editPanel.Location = new Point(15, 15);
            editPanel.BorderStyle = BorderStyle.FixedSingle;
            editPanel.Tag = artist;

            Label label = new Label();
            label.Text = "ID: " + artist.artistID.ToString();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Location = new Point(15, 5);
            label.AutoSize = true;
            editPanel.Controls.Add(label);





            List<object> items = new List<object>();
            items.Add(artist);

            Button saveButton = new Button();
            saveButton.Location = new Point(773, 5);
            saveButton.Text = "Save";
            saveButton.AutoSize = true;
            saveButton.BackColor = Color.LightGray;
            saveButton.Tag = items;
            saveButton.Click += NewArtistAdded;
            editPanel.Controls.Add(saveButton);

            return editPanel;
        }

        public void NewArtistAdded(object sender, EventArgs e)
        {

        }

        public Panel DisplayAlbumEdit(Album album)
        {
            Panel editPanel = new Panel();
            editPanel.Size = new Size(855, 573);
            editPanel.Location = new Point(15, 15);
            editPanel.BorderStyle = BorderStyle.FixedSingle;
            editPanel.Tag = album;

            Label label = new Label();
            label.Text = "ID: " + album.albumID.ToString();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Location = new Point(15, 5);
            label.AutoSize = true;
            editPanel.Controls.Add(label);





            List<object> items = new List<object>();
            items.Add(album);

            Button saveButton = new Button();
            saveButton.Location = new Point(773, 5);
            saveButton.Text = "Save";
            saveButton.AutoSize = true;
            saveButton.BackColor = Color.LightGray;
            saveButton.Tag = items;
            saveButton.Click += NewAlbumAdded;
            editPanel.Controls.Add(saveButton);

            return editPanel;
        }

        public void NewAlbumAdded(object sender, EventArgs e)
        {

        }

        public Panel DisplaySongEdit(Song song)
        {
            Panel editPanel = new Panel();
            editPanel.Size = new Size(855, 573);
            editPanel.Location = new Point(15, 15);
            editPanel.BorderStyle = BorderStyle.FixedSingle;
            editPanel.Tag = song;

            Label label = new Label();
            label.Text = "ID: " + song.songID.ToString();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Location = new Point(15, 5);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Name";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(15, 56);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            TextBox name = new TextBox();
            name.Size = new Size(212, 25);
            name.Location = new Point(63, 55);
            name.Text = song.name;
            editPanel.Controls.Add(name);

            label = new Label();
            label.Text = "Length";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(275, 56);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            TextBox length = new TextBox();
            length.Size = new Size(212, 25);
            length.Location = new Point(323, 55);
            length.Text = song.length;
            editPanel.Controls.Add(length);

            label = new Label();
            label.Text = "Links";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(535, 56);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            TextBox links = new TextBox();
            links.Size = new Size(212, 25);
            links.Location = new Point(583, 55);
            if (song.songPages != null)
            {
                links.Text = song.songPages[0];
            }
            editPanel.Controls.Add(links);

            label = new Label();
            label.Text = "Notes";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(15, 76);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            TextBox notes = new TextBox();
            notes.Size = new Size(732, 25);
            notes.Location = new Point(63, 75);
            notes.Text = song.notes;
            editPanel.Controls.Add(notes);

            label = new Label();
            label.Text = "Bands";
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Location = new Point(275, 105);
            label.AutoSize = true;
            editPanel.Controls.Add(label);

            ListBox bands = new ListBox();
            foreach (Band band in info.bands)
            {
                bands.Items.Add(band.name);
                if (song.band != null)
                {
                    if (song.band == band)
                    {
                        bands.SelectedIndex = bands.Items.Count - 1;
                    }
                }
            }
            bands.Location = new Point(275, 125);
            bands.Size = new Size(260, 430);
            editPanel.Controls.Add(bands);

            List<object> items = new List<object>();
            items.Add(song);
            items.Add(name);
            items.Add(length);
            items.Add(links);
            items.Add(notes);
            items.Add(bands);

            Button saveButton = new Button();
            saveButton.Location = new Point(773, 5);
            saveButton.Text = "Save";
            saveButton.AutoSize = true;
            saveButton.BackColor = Color.LightGray;
            saveButton.Tag = items;
            saveButton.Click += NewSongAdded;
            editPanel.Controls.Add(saveButton);

            return editPanel;
        }

        public void NewSongAdded(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            List<object> items = (List<object>)button.Tag;
            Song song = (Song)items[0];

            TextBox textBox = (TextBox)items[1];
            song.name = textBox.Text;

            textBox = (TextBox)items[2];
            song.length = textBox.Text;

            textBox = (TextBox)items[3];
            song.songPages = new List<string>();
            song.songPages.Add(textBox.Text);

            textBox = (TextBox)items[4];
            song.notes = textBox.Text;

            ListBox listBox = (ListBox)items[5];
            song.band = info.bands[listBox.SelectedIndex];

            if (!info.songs.Contains(song))
            {
                info.songs.Add(song);
            }
        }

        public Panel DisplayVenueEdit(Venue venue)
        {
            Panel editPanel = new Panel();
            editPanel.Size = new Size(855, 573);
            editPanel.Location = new Point(15, 15);
            editPanel.BorderStyle = BorderStyle.FixedSingle;
            editPanel.Tag = venue;

            Label label = new Label();
            label.Text = "ID: " + venue.venueID.ToString();
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Location = new Point(15, 5);
            label.AutoSize = true;
            editPanel.Controls.Add(label);





            List<object> items = new List<object>();
            items.Add(venue);

            Button saveButton = new Button();
            saveButton.Location = new Point(773, 5);
            saveButton.Text = "Save";
            saveButton.AutoSize = true;
            saveButton.BackColor = Color.LightGray;
            saveButton.Tag = items;
            saveButton.Click += NewVenueAdded;
            editPanel.Controls.Add(saveButton);

            return editPanel;
        }

        public void NewVenueAdded(object sender, EventArgs e)
        {

        }

        public void AddButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Tuple<ListBox, ComboBox> tuple = (Tuple<ListBox, ComboBox>)button.Tag;

            int index = Convert.ToInt32(tuple.Item2.SelectedIndex);
            List<Song> songList = (List<Song>)tuple.Item1.Tag;

            tuple.Item1.Items.Add(info.songs[index].name);
            songList.Add(info.songs[index]);

            tuple.Item1.Tag = songList;
        }

        public void SongRemovedFromList(object sender, EventArgs e)
        {
            ListBox listbox = (ListBox)sender;
            List<Song> songs = (List<Song>)listbox.Tag;
            songs.RemoveAt(listbox.SelectedIndex);
            listbox.Items.Remove(listbox.SelectedItem);
        }





        public void SongClicked(object sender, EventArgs e)
        {
            try
            {
                Label label = (Label)sender;
                Song song = (Song)label.Tag;
                DisplaySongInfo(song);
            }
            catch (Exception ex)
            {
                Panel panel = (Panel)sender;
                Song song = (Song)panel.Tag;
                DisplaySongInfo(song);
            }
        }

        public void BandClicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Band band = (Band)label.Tag;
            DisplayBandInfo(band);
        }

        public void GigClicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Gig gig = (Gig)label.Tag;
            DisplayGigInfo(gig);
        }

        public void ArtistClicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Artist artist = (Artist)label.Tag;
            DisplayArtistInfo(artist);
        }

        public void AlbumClicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Album album = (Album)label.Tag;
            DisplayAlbumInfo(album);
        }

        public void VenueClicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Venue venue = (Venue)label.Tag;
            DisplayVenueInfo(venue);
        }

        public void PageClicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string link = (string)label.Tag;

            Process.Start(chromeLink, $"{link}");
        }

        public void DateClicked(object sender, EventArgs e)
        {
            panel.Controls.Clear();

            int YDepth = -15, year = 0;

            foreach (Gig gig in info.gigs)
            {
                Label label = new Label();
                if (gig.date.Year != year)
                {
                    YDepth += 15;
                    label = new Label();
                    label.Font = new Font("Arial", 20, FontStyle.Bold);
                    label.Text = gig.date.Year.ToString();
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    panel.Controls.Add(label);
                    YDepth += 34;
                    year = gig.date.Year;
                }
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"{gig.bands[0].name} - {gig.date}";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += GigClicked;
                label.Tag = gig;
                panel.Controls.Add(label);
                YDepth += 17;
            }
        }

        public void CityClicked(object sender, EventArgs e)
        {
            panel.Controls.Clear();

            int YDepth = -15;
            string city = "aaaaaa";
            info.venues = info.venues.OrderBy(o => o.city).ToList();

            foreach (Venue venue in info.venues)
            {
                Label label = new Label();
                if (venue.city != city)
                {
                    YDepth += 15;
                    label = new Label();
                    label.Font = new Font("Arial", 20, FontStyle.Bold);
                    label.Text = venue.city;
                    label.Location = new Point(0, YDepth);
                    label.AutoSize = true;
                    panel.Controls.Add(label);
                    YDepth += 34;
                    city = venue.city;
                }
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"{venue.venue}";
                label.Location = new Point(0, YDepth);
                label.AutoSize = true;
                label.Click += VenueClicked;
                label.Tag = venue;
                panel.Controls.Add(label);
                YDepth += 17;
            }
        }

        public Panel GetFormattedSong(Song song, Band band, int number, bool albumNeeded)
        {
            ToolTip toolTip = new ToolTip();

            Panel songPanel = new Panel();
            songPanel.Size = new Size(867, 45);
            songPanel.BorderStyle = BorderStyle.FixedSingle;
            Label label;

            if (albumNeeded)
            {
                label = new Label();
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.Text = $"{number})";
                label.Location = new Point(5, 15);
                label.AutoSize = true;
                songPanel.Controls.Add(label);
            }

            label = new Label();
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Text = song.name;
            label.Tag = song;
            label.Location = new Point(30, 15);
            label.AutoSize = true;
            label.Click += SongClicked;
            label.Cursor = Cursors.Hand;
            toolTip.SetToolTip(label, "Click to open song info");
            songPanel.Controls.Add(label);

            if (song.band != band)
            {
                label.Location = new Point(30, 7);
                label = new Label();
                label.Font = new Font("Arial", 8, FontStyle.Regular);
                label.Text = $"Cover by {song.band.name}";
                label.Tag = song.band;
                label.Location = new Point(30, 25);
                label.AutoSize = true;
                label.Click += BandClicked;
                label.Cursor = Cursors.Hand;
                toolTip.SetToolTip(label, "Click to open band info");
                songPanel.Controls.Add(label);
            }

            if (albumNeeded)
            {
                Album album = GetAlbumFromSong(song, band);
                if (album.name != null)
                {
                    label = new Label();
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.Text = album.name;
                    label.Tag = album;
                    label.Location = new Point(400, 15);
                    label.AutoSize = true;
                    label.Click += AlbumClicked;
                    label.Cursor = Cursors.Hand;
                    toolTip.SetToolTip(label, "Click to open album info");
                    songPanel.Controls.Add(label);
                }
            }

            label = new Label();
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.Text = song.length;
            label.Location = new Point(820, 15);
            label.AutoSize = true;
            songPanel.Controls.Add(label);

            return songPanel;
        }

        public Album GetAlbumFromSong(Song song, Band band)
        {
            foreach (Album album in info.albums)
            {
                if (album.songs.Contains(song))
                {
                    if (band == album.band)
                    {
                        return album;
                    }
                }
            }
            foreach (Album album in info.albums)
            {
                if (album.songs.Contains(song))
                {
                    return album;
                }
            }
            return new Album();
        }

        public List<Gig> GetGigsFromAlbum(Album album)
        {
            List<Gig> gigs = new List<Gig>();

            foreach (Gig gig in info.gigs)
            {
                foreach (Song song in gig.setlist)
                {

                    if (GetAlbumFromSong(song, gig.bands[0]) == album)
                    {
                        gigs.Remove(gig);
                        gigs.Add(gig);
                    }
                }
                foreach (Song song in gig.encore)
                {
                    if (GetAlbumFromSong(song, gig.bands[0]) == album)
                    {
                        gigs.Remove(gig);
                        gigs.Add(gig);
                    }
                }
            }

            return gigs;
        }





        public void SelectProfile()
        {
            panel.Controls.Clear();
            info.profile = "";

            Label label = new Label();
            label.Text = "Select a Profile:";
            label.Size = new Size(200, 15);
            label.Location = new Point(367, 250);
            panel.Controls.Add(label);

            Button button = new Button();
            button.Size = new Size(150, 30);
            button.Location = new Point(367, 305);
            button.Text = "Create profile";
            button.Click += LoadProfile;
            panel.Controls.Add(button);

            ComboBox comboBox = new ComboBox();
            comboBox.Size = new Size(150, 15);
            comboBox.Location = new Point(367, 275);
            comboBox.Tag = button;

            foreach (String profileName in Directory.GetDirectories(@"Profiles"))
            {
                comboBox.Items.Add(profileName.Substring(9));
            }
            comboBox.Text = "Robert";
            panel.Controls.Add(comboBox);

            comboBox.TextChanged += UpdateButtonText;
        }

        public void UpdateButtonText(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Button button = (Button)comboBox.Tag;

            if (Directory.Exists(@$"Profiles/{comboBox.Text}") && comboBox.Text != "")
            {
                button.Text = "Load Profile";
            }
            else
            {
                button.Text = "Create Profile";
            }
        }

        public void LoadProfile(object sender, EventArgs e)
        {
            if (panel.Controls[panel.Controls.Count - 1].Text != "")
            {
                string profile = panel.Controls[panel.Controls.Count - 1].Text;
                string directory = $"Profiles/{profile}";
                bool exists = Directory.Exists(@directory);
                info.profile = profile;

                string[] directories = { "Albums", "Artists", "Bands", "Gigs", "Songs", "Venues" };

                if (!exists)
                {
                    Directory.CreateDirectory(@directory);
                }
                foreach (string type in directories)
                {
                    if (!exists)
                    {
                        File.WriteAllText(@directory + $"/{type}.txt", "end");
                    }
                    File.Delete($@"{type}.txt");
                    File.Copy(directory + $"/{type}.txt", $@"{type}.txt");
                }

                info.PopulateInfo(info);

                panel.Controls.Clear();
                if (info.gigs.Count > 0)
                {
                    gigsMenuItem_Click(null, null);
                    displayList.SelectedIndex = 0;
                }
            }
        }





        static Band GetBandFromID(Info info, string id)
        {
            foreach (Band band in info.bands)
            {
                if (band.bandID == Convert.ToInt32(id))
                {
                    return band;
                }
            }
            return new Band();
        }

        static Artist GetArtistFromID(Info info, string id)
        {
            foreach (Artist artist in info.artists)
            {
                if (artist.artistID == Convert.ToInt32(id))
                {
                    return artist;
                }
            }
            return new Artist();
        }

        static Album GetAlbumFromID(Info info, string id)
        {
            foreach (Album album in info.albums)
            {
                try
                {
                    if (album.albumID == Convert.ToInt32(id))
                    {
                        return album;
                    }
                }
                catch { }
            }
            return new Album();
        }

        static Song GetSongFromID(Info info, string id)
        {
            foreach (Song song in info.songs)
            {
                if (song.songID == Convert.ToInt32(id))
                {
                    return song;
                }
            }
            return new Song();
        }

        static Venue GetVenueFromID(Info info, string id)
        {
            foreach (Venue venue in info.venues)
            {
                if (venue.venueID == Convert.ToInt32(id))
                {
                    return venue;
                }
            }
            return new Venue();
        }

        static List<Band> GetBandsArrayFromID(Info info, List<string> ids)
        {
            List<Band> bands = new List<Band>();
            if (ids[0] != "")
            {
                foreach (string id in ids)
                {
                    bands.Add(GetBandFromID(info, id));
                }
            }
            return bands;
        }

        static List<Song> GetSongsArrayFromID(Info info, List<string> ids)
        {
            List<Song> songs = new List<Song>();
            if (ids[0] != "")
            {
                foreach (string id in ids)
                {
                    songs.Add(GetSongFromID(info, id));
                }
            }
            return songs;
        }

        static List<Artist> GetArtistArrayFromID(Info info, List<string> ids)
        {
            List<Artist> artists = new List<Artist>();
            foreach (string id in ids)
            {
                try
                {
                    artists.Add(GetArtistFromID(info, id));
                }
                catch { }
            }
            return artists;
        }





        public void gigsMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                info.gigs = info.gigs.OrderBy(o => o.date).ToList();

                if (info.gigs.Count > 0)
                {
                    info.selected = 1;
                    displayList.Items.Clear();
                    foreach (Gig gig in info.gigs)
                    {
                        displayList.Items.Add($"{GetBandFromID(info, gig.bands[0].bandID.ToString()).name} - {gig.date}");
                    }
                }
            }
        }

        private void gigsMenuItem_DoubleClick(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                DisplayAllGigInfo(null, null);
            }
        }

        private void bandsMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                info.bands = info.bands.OrderBy(o => o.name).ToList();

                if (info.bands.Count > 0)
                {
                    info.selected = 2;
                    displayList.Items.Clear();
                    int index = 0;
                    foreach (Band band in info.bands)
                    {
                        displayList.Items.Add($"{band.name}");
                    }
                }
            }
        }

        private void bandsMenuItem_DoubleClick(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                DisplayAllBandInfo(null, null);
            }
        }

        private void artistsMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                info.artists = info.artists.OrderBy(o => o.name).ToList();

                if (info.artists.Count > 0)
                {
                    info.selected = 3;
                    displayList.Items.Clear();
                    foreach (Artist artist in info.artists)
                    {
                        displayList.Items.Add($"{artist.name}");
                    }
                }
            }
        }

        private void artistsMenuItem_DoubleClick(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                DisplayAllArtistInfo(null, null);
            }
        }

        private void albumsMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                info.albums = info.albums.OrderBy(o => o.name).ToList();

                if (info.albums.Count > 0)
                {
                    info.selected = 4;
                    displayList.Items.Clear();
                    foreach (Album album in info.albums)
                    {
                        displayList.Items.Add($"{album.name}");
                    }
                }
            }
        }

        private void albumsMenuItem_DoubleClick(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                DisplayAllAlbumInfo(null, null);
            }
        }

        private void songsMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                info.songs = info.songs.OrderBy(o => o.name).ToList();

                if (info.songs.Count > 0)
                {
                    info.selected = 5;
                    displayList.Items.Clear();
                    foreach (Song song in info.songs)
                    {
                        displayList.Items.Add($"{song.name}");
                    }
                }
            }
        }

        private void songsMenuItem_DoubleClick(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                DisplayAllSongInfo(null, null);
            }
        }

        private void venuesMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                info.venues = info.venues.OrderBy(o => o.city).ToList();

                if (info.venues.Count > 0)
                {
                    info.selected = 6;
                    displayList.Items.Clear();
                    foreach (Venue venue in info.venues)
                    {
                        displayList.Items.Add($"{venue.venue}, {venue.city}");
                    }
                }
            }
        }

        private void venuesMenuItem_DoubleClick(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                DisplayAllVenueInfo(null, null);
            }
        }





        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                CreateNew();
            }
            else
            {
                MessageBox.Show("Please select a profile first");
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                EditItem();
            }
            else
            {
                MessageBox.Show("Please select a profile first");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                File.Delete(@"Albums.txt");
                StreamWriter sw = new StreamWriter(@"Albums.txt");
                foreach (Album album in info.albums)
                {
                    sw.WriteLine(album.albumID);
                    sw.WriteLine(album.name);

                    for (int links = 0; links < album.albumPages.Count; links++)
                    {
                        sw.Write(album.albumPages[links]);
                        if (links + 1 != album.albumPages.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(album.band.bandID);

                    for (int songs = 0; songs < album.songs.Count; songs++)
                    {
                        sw.Write(album.songs[songs].songID);
                        if (songs + 1 != album.songs.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(album.notes);
                }
                sw.WriteLine("end");
                sw.Close();

                File.Delete(@"Bands.txt");
                sw = new StreamWriter(@"Bands.txt");
                foreach (Band band in info.bands)
                {
                    sw.WriteLine(band.bandID);
                    sw.WriteLine(band.name);

                    for (int members = 0; members < band.members.Count; members++)
                    {
                        sw.Write(band.members[members].artistID);
                        if (members + 1 != band.members.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    for (int pages = 0; pages < band.bandPages.Count; pages++)
                    {
                        sw.Write(band.bandPages[pages]);
                        if (pages + 1 != band.bandPages.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(band.notes);
                }
                sw.WriteLine("end");
                sw.Close();

                File.Delete(@"Songs.txt");
                sw = new StreamWriter(@"Songs.txt");
                foreach (Song song in info.songs)
                {
                    sw.WriteLine(song.songID);
                    sw.WriteLine(song.name);

                    for (int links = 0; links < song.songPages.Count; links++)
                    {
                        sw.Write(song.songPages[links]);
                        if (links + 1 != song.songPages.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(song.length);
                    sw.WriteLine(song.band.bandID);
                    sw.WriteLine(song.notes);
                }
                sw.WriteLine("end");
                sw.Close();

                File.Delete(@"Gigs.txt");
                sw = new StreamWriter(@"Gigs.txt");
                foreach (Gig gig in info.gigs)
                {
                    sw.WriteLine(gig.gigID);

                    for (int bands = 0; bands < gig.bands.Count; bands++)
                    {
                        sw.Write(gig.bands[bands].bandID);
                        if (bands + 1 != gig.bands.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    for (int supports = 0; supports < gig.supports.Count; supports++)
                    {
                        sw.Write(gig.supports[supports].bandID);
                        if (supports + 1 != gig.supports.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(gig.tour);
                    if (gig.album != null)
                    {
                        sw.WriteLine(gig.album.albumID);
                    }
                    else
                    {
                        sw.WriteLine();
                    }
                    sw.WriteLine(gig.date.Day + "/" + gig.date.Month + "/" + gig.date.Year);
                    sw.WriteLine(gig.setlistLink);

                    for (int pages = 0; pages < gig.gigPages.Count; pages++)
                    {
                        sw.Write(gig.gigPages[pages]);
                        if (pages + 1 != gig.gigPages.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(gig.venue.venueID);

                    for (int setlist = 0; setlist < gig.setlist.Count; setlist++)
                    {
                        sw.Write(gig.setlist[setlist].songID);
                        if (setlist + 1 != gig.setlist.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    for (int encore = 0; encore < gig.encore.Count; encore++)
                    {
                        sw.Write(gig.encore[encore].songID);
                        if (encore + 1 != gig.encore.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(gig.notes);
                }
                sw.WriteLine("end");
                sw.Close();

                File.Delete(@"Artists.txt");
                sw = new StreamWriter(@"Artists.txt");
                foreach (Artist artist in info.artists)
                {
                    sw.WriteLine(artist.artistID);
                    sw.WriteLine(artist.name);

                    for (int pages = 0; pages < artist.artistPages.Count; pages++)
                    {
                        sw.Write(artist.artistPages[pages]);
                        if (pages + 1 != artist.artistPages.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(artist.notes);
                }
                sw.WriteLine("end");
                sw.Close();

                File.Delete(@"Venues.txt");
                sw = new StreamWriter(@"Venues.txt");
                foreach (Venue venue in info.venues)
                {
                    sw.WriteLine(venue.venueID);
                    sw.WriteLine(venue.city);

                    for (int pages = 0; pages < venue.cityPages.Count; pages++)
                    {
                        sw.Write(venue.cityPages[pages]);
                        if (pages + 1 != venue.cityPages.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(venue.venue);

                    for (int pages = 0; pages < venue.venuePages.Count; pages++)
                    {
                        sw.Write(venue.venuePages[pages]);
                        if (pages + 1 != venue.venuePages.Count)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();

                    sw.WriteLine(venue.notes);
                }
                sw.WriteLine("end");
                sw.Close();

                string[] directories = { "Albums", "Artists", "Bands", "Gigs", "Songs", "Venues" };
                foreach (string type in directories)
                {
                    File.Delete(@$"Profiles/{info.profile}/{type}.txt");
                    File.Copy($@"{type}.txt", @$"Profiles/{info.profile}/{type}.txt");
                }
            }
            else
            {
                MessageBox.Show("Please select a profile first");
            }
        }

        private void displayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;

            switch (info.selected)
            {
                case 1:
                    DisplayGigInfo(info.gigs[listBox.SelectedIndex]);
                    break;
                case 2:
                    DisplayBandInfo(info.bands[listBox.SelectedIndex]);
                    break;
                case 3:
                    DisplayArtistInfo(info.artists[listBox.SelectedIndex]);
                    break;
                case 4:
                    DisplayAlbumInfo(info.albums[listBox.SelectedIndex]);
                    break;
                case 5:
                    DisplaySongInfo(info.songs[listBox.SelectedIndex]);
                    break;
                case 6:
                    DisplayVenueInfo(info.venues[listBox.SelectedIndex]);
                    break;
            }
        }

        private void profilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                SelectProfile();
                displayList.Items.Clear();
            }
        }

        private void statsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                DisplayTopInfo();
            }
        }

        private void yearsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                DateClicked(sender, e);
            }
        }

        private void citiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (info.profile != "")
            {
                CityClicked(sender, e);
            }
        }





        private void Shortcut(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Alt)
            {
                if (e.KeyCode == Keys.S)
                {
                    saveToolStripMenuItem_Click(null, null);
                }
                else if (e.KeyCode == Keys.E)
                {
                    editToolStripMenuItem_Click(null, null);
                }
                else if (e.KeyCode == Keys.N)
                {
                    addNewToolStripMenuItem_Click(null, null);
                }
                else if (e.KeyCode == Keys.P)
                {
                    SelectProfile();
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(@"Export.txt");
            foreach (Gig gig in info.gigs)
            {
                sw.WriteLine($"{gig.bands[0].name} - {gig.date}");
                if (gig.tour != "")
                {
                    sw.WriteLine($"Tour: {gig.tour}");
                }
                sw.WriteLine($"{gig.venue.venue}, {gig.venue.city}");
                if (gig.setlistLink != "")
                {
                    sw.WriteLine(gig.setlistLink);
                }
                if (gig.setlist.Count > 0)
                {
                    int index = 0;
                    sw.WriteLine("\nSetlist: ");
                    foreach (Song song in gig.setlist)
                    {
                        sw.Write($"{song.name}{(song.band == gig.bands[0] ? "" : $" ({song.band.name} cover)")}");
                        index++;
                        if (index != gig.setlist.Count)
                        {
                            sw.WriteLine();
                        }
                    }
                }
                if (gig.encore.Count > 0)
                {
                    int index = 0;
                    sw.WriteLine("\n\nEncore: ");
                    foreach (Song song in gig.encore)
                    {
                        sw.Write($"{song.name}{(song.band == gig.bands[0] ? "" : $" ({song.band.name} cover)")}");
                        index++;
                        if (index != gig.encore.Count)
                        {
                            sw.WriteLine();
                        }
                    }
                }
                sw.WriteLine("\n\n\n\n");
            }
            sw.Close();
        }
    }
}