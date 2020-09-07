using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Input;
using static listen2me.MediaController;

namespace listen2me
{
    class Details : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return cp;
            }
        }
        public Details()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddLine(new Point(curvSize, 0), new Point(e.ClipRectangle.Width - curvSize, 0));
            path.AddArc(e.ClipRectangle.Width - 1 - curvSize * 2, 0, curvSize * 2, curvSize * 2, -90f, 90f);
            
            path.AddLine(new Point(e.ClipRectangle.Width - 1, curvSize), new Point(e.ClipRectangle.Width - 1, e.ClipRectangle.Height - curvSize - bottomLine - 1));
            path.AddArc(e.ClipRectangle.Width - 1 - curvSize * 2, e.ClipRectangle.Height - curvSize * 2 - bottomLine - 1, curvSize * 2, curvSize * 2, 0f, 90f);

            //path.AddLine(new Point(e.ClipRectangle.Width - curvSize - 1, e.ClipRectangle.Height - 11), new Point(curPos + arrowWide, e.ClipRectangle.Height - 11));
            //path.AddLine(new Point(curPos + arrowWide, e.ClipRectangle.Height - 11), new Point(curPos, e.ClipRectangle.Height - 1));
            //path.AddLine(new Point(curPos, e.ClipRectangle.Height), new Point(curPos - arrowWide - 1, e.ClipRectangle.Height - 11));
            //path.AddLine(new Point(curPos - arrowWide - 1, e.ClipRectangle.Height - 11), new Point(curvSize, e.ClipRectangle.Height - 11));

            path.AddLine(new Point(e.ClipRectangle.Width - 1 - curvSize, e.ClipRectangle.Height - bottomLine - 1), new Point(curvSize, e.ClipRectangle.Height - bottomLine - 1));
            path.AddArc(0, e.ClipRectangle.Height - curvSize * 2 - bottomLine - 1, curvSize * 2, curvSize * 2, 90f, 90f);

            path.AddLine(new Point(0, e.ClipRectangle.Height - curvSize - bottomLine - 1), new Point(0, curvSize));
            path.AddArc(0, 0, curvSize * 2, curvSize * 2, 180f, 90f);
            path.CloseFigure();

            Pen outline = new Pen(Color.FromArgb(64, 64, 64), 2);
            Brush fill = new SolidBrush(Color.FromArgb(232, 16, 16, 16));
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(outline, path);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }

        protected override void OnResize(EventArgs e)
        {
            this.songName.Size = new Size(this.Width - 130, this.songName.Font.Height);
            this.artistName.Size = new Size(this.Width - 130, this.artistName.Font.Height);
            this.albumName.Size = new Size(this.Width - 130, this.albumName.Font.Height);
            base.OnResize(e);
        }

        public void SetMediaInfo(MediaInfo info)
        {
            Image img = info.Thumbnail;
            int imageMin = Math.Min(img.Width, img.Height);
            Bitmap newImg = ImageHelper.ResizeImage(img, (float)imageMin / (float)100);
            bigThumbnail.Image = newImg;

            artistName.Text = info.Artist;
            albumName.Text = info.AlbumTitle;
            songName.Text = info.Title;
        }

        public void SetThumbnail(Image img)
        {
            int imageMin = Math.Min(img.Width, img.Height);
            Bitmap newImg = ImageHelper.ResizeImage(img, (float)imageMin / (float)100);
            bigThumbnail.Image = newImg;
        }

        private void InitializeComponent()
        {
            this.bigThumbnail = new PictureBox();
            this.songName = new TransparentLabel();
            this.artistName = new TransparentLabel();
            this.albumName = new TransparentLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bigThumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // bigThumbnail
            // 
            this.bigThumbnail.Location = new Point(10, 10);
            this.bigThumbnail.Name = "bigThumbnail";
            this.bigThumbnail.Size = new Size(100, 100);
            this.bigThumbnail.TabIndex = 0;
            this.bigThumbnail.TabStop = false;
            //
            // songName
            //
            this.songName.AutoSize = false;
            this.songName.Location = new Point(120, 10);
            this.songName.Name = "songName";
            this.songName.Font = new Font("Arial",  18);
            this.songName.TabIndex = 1;
            this.songName.TabStop = false;
            this.songName.ForeColor = Color.White;
            this.songName.Text = "Song Name";
            this.songName.Size = new Size(this.Width - 130, this.songName.Font.Height);
            //
            // songName
            //
            this.artistName.AutoSize = false;
            this.artistName.Location = new Point(120, 15 + this.songName.Font.Height);
            this.artistName.Name = "artistName";
            this.artistName.Font = new Font("Arial", 11);
            this.artistName.TabIndex = 1;
            this.artistName.TabStop = false;
            this.artistName.ForeColor = Color.White;
            this.artistName.Text = "Artist Name";
            this.artistName.Size = new Size(this.Width - 130, this.artistName.Font.Height);
            //
            // songName
            //
            this.albumName.AutoSize = false;
            this.albumName.Location = new Point(120, 20 + this.songName.Font.Height + this.artistName.Font.Height);
            this.albumName.Name = "albumName";
            this.albumName.Font = new Font("Arial", 7);
            this.albumName.TabIndex = 2;
            this.albumName.TabStop = false;
            this.albumName.ForeColor = Color.White;
            this.albumName.Text = "Album Name";
            this.albumName.Size = new Size(this.Width - 130, this.albumName.Font.Height);
            // 
            // Details
            // 
            this.ClientSize = new Size(500, 120);
            this.Controls.Add(this.bigThumbnail);
            this.Controls.Add(this.songName);
            this.Controls.Add(this.artistName);
            this.Controls.Add(this.albumName);
            this.FormBorderStyle = FormBorderStyle.None;
            //this.BackColor = Color.Transparent;
            this.Name = "Details";
            ((System.ComponentModel.ISupportInitialize)(this.bigThumbnail)).EndInit();
            this.ResumeLayout(false);
        }
        public int PointerPosition
        {
            get { return curPos; }
            set
            {
                if (curPos != value)
                {
                    curPos = value;
                    Refresh();
                }
            }
        }

        private PictureBox bigThumbnail;
        private Label songName;
        private Label artistName;
        private Label albumName;
        private int curPos = 50;
        private int curvSize = 10;
        private int arrowWide = 7;
        private int bottomLine = 0;
    }
}
