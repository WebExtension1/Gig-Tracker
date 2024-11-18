namespace GigTracker
{
    partial class GigTracker
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.displayList = new System.Windows.Forms.ListBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.profilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yearsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.citiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gigsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bandsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.artistsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.albumsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.songsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.venuesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // displayList
            // 
            this.displayList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.displayList.FormattingEnabled = true;
            this.displayList.ItemHeight = 15;
            this.displayList.Location = new System.Drawing.Point(12, 38);
            this.displayList.Name = "displayList";
            this.displayList.Size = new System.Drawing.Size(312, 602);
            this.displayList.TabIndex = 1;
            this.displayList.SelectedIndexChanged += new System.EventHandler(this.displayList_SelectedIndexChanged);
            this.displayList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Shortcut);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Gainsboro;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.profilesToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.statsToolStripMenuItem,
            this.gigsMenuItem,
            this.bandsMenuItem,
            this.artistsMenuItem,
            this.albumsMenuItem,
            this.songsMenuItem,
            this.venuesMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1234, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // profilesToolStripMenuItem
            // 
            this.profilesToolStripMenuItem.Name = "profilesToolStripMenuItem";
            this.profilesToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.profilesToolStripMenuItem.Text = "Profiles";
            this.profilesToolStripMenuItem.Click += new System.EventHandler(this.profilesToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.editToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.addNewToolStripMenuItem.Text = "New";
            this.addNewToolStripMenuItem.Click += new System.EventHandler(this.addNewToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // statsToolStripMenuItem
            // 
            this.statsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.yearsToolStripMenuItem,
            this.citiesToolStripMenuItem});
            this.statsToolStripMenuItem.Name = "statsToolStripMenuItem";
            this.statsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.statsToolStripMenuItem.Text = "Stats";
            this.statsToolStripMenuItem.Click += new System.EventHandler(this.statsToolStripMenuItem_Click);
            // 
            // yearsToolStripMenuItem
            // 
            this.yearsToolStripMenuItem.Name = "yearsToolStripMenuItem";
            this.yearsToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.yearsToolStripMenuItem.Text = "Years";
            this.yearsToolStripMenuItem.Click += new System.EventHandler(this.yearsToolStripMenuItem_Click);
            // 
            // citiesToolStripMenuItem
            // 
            this.citiesToolStripMenuItem.Name = "citiesToolStripMenuItem";
            this.citiesToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.citiesToolStripMenuItem.Text = "Cities";
            this.citiesToolStripMenuItem.Click += new System.EventHandler(this.citiesToolStripMenuItem_Click);
            // 
            // gigsMenuItem
            // 
            this.gigsMenuItem.Name = "gigsMenuItem";
            this.gigsMenuItem.Size = new System.Drawing.Size(42, 20);
            this.gigsMenuItem.Text = "Gigs";
            this.gigsMenuItem.Click += new System.EventHandler(this.gigsMenuItem_Click);
            this.gigsMenuItem.DoubleClick += new System.EventHandler(this.gigsMenuItem_DoubleClick);
            // 
            // bandsMenuItem
            // 
            this.bandsMenuItem.Name = "bandsMenuItem";
            this.bandsMenuItem.Size = new System.Drawing.Size(51, 20);
            this.bandsMenuItem.Text = "Bands";
            this.bandsMenuItem.Click += new System.EventHandler(this.bandsMenuItem_Click);
            this.bandsMenuItem.DoubleClick += new System.EventHandler(this.bandsMenuItem_DoubleClick);
            // 
            // artistsMenuItem
            // 
            this.artistsMenuItem.Name = "artistsMenuItem";
            this.artistsMenuItem.Size = new System.Drawing.Size(52, 20);
            this.artistsMenuItem.Text = "Artists";
            this.artistsMenuItem.Click += new System.EventHandler(this.artistsMenuItem_Click);
            this.artistsMenuItem.DoubleClick += new System.EventHandler(this.artistsMenuItem_DoubleClick);
            // 
            // albumsMenuItem
            // 
            this.albumsMenuItem.Name = "albumsMenuItem";
            this.albumsMenuItem.Size = new System.Drawing.Size(60, 20);
            this.albumsMenuItem.Text = "Albums";
            this.albumsMenuItem.Click += new System.EventHandler(this.albumsMenuItem_Click);
            this.albumsMenuItem.DoubleClick += new System.EventHandler(this.albumsMenuItem_DoubleClick);
            // 
            // songsMenuItem
            // 
            this.songsMenuItem.Name = "songsMenuItem";
            this.songsMenuItem.Size = new System.Drawing.Size(51, 20);
            this.songsMenuItem.Text = "Songs";
            this.songsMenuItem.Click += new System.EventHandler(this.songsMenuItem_Click);
            this.songsMenuItem.DoubleClick += new System.EventHandler(this.songsMenuItem_DoubleClick);
            // 
            // venuesMenuItem
            // 
            this.venuesMenuItem.Name = "venuesMenuItem";
            this.venuesMenuItem.Size = new System.Drawing.Size(56, 20);
            this.venuesMenuItem.Text = "Venues";
            this.venuesMenuItem.Click += new System.EventHandler(this.venuesMenuItem_Click);
            this.venuesMenuItem.DoubleClick += new System.EventHandler(this.venuesMenuItem_DoubleClick);
            // 
            // GigTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1234, 654);
            this.Controls.Add(this.displayList);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximumSize = new System.Drawing.Size(1250, 693);
            this.MinimumSize = new System.Drawing.Size(1250, 693);
            this.Name = "GigTracker";
            this.Text = "GigTracker";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ListBox displayList;
        private MenuStrip menuStrip;
        private ToolStripMenuItem gigsMenuItem;
        private ToolStripMenuItem bandsMenuItem;
        private ToolStripMenuItem artistsMenuItem;
        private ToolStripMenuItem albumsMenuItem;
        private ToolStripMenuItem songsMenuItem;
        private ToolStripMenuItem venuesMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem addNewToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem profilesToolStripMenuItem;
        private ToolStripMenuItem statsToolStripMenuItem;
        private ToolStripMenuItem yearsToolStripMenuItem;
        private ToolStripMenuItem citiesToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
    }
}