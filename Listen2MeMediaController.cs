using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using Windows.Media.Control;

namespace application
{
    partial class Listen2MeMediaController : Form
    {
        public Listen2MeMediaController() : base()
        {
            __taskbar.SetAsParentOf(this);
        }

        protected override void OnResize(EventArgs e)
        {
            __taskbar.FetchInfo();
            base.OnResize(e);
        }

        protected override void OnClick(EventArgs e)
        {
            OnResize(new EventArgs());
            base.OnClick(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Transparent);
            UpdateSizesAndPositions();
            base.OnPaint(e);
        }

        public void Initialize()
        {
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            //this.playbackProgressBar = new ProgressBar();
            this.prevButton = new CustomButton();
            this.nextButton = new CustomButton();
            this.playPauseButton = new CustomButton();
            this.multimediaCover = new PictureBox();
            this.volumeButton = new CustomButton();
            // 
            // prev
            // 
            this.prevButton.Margin = new Padding(0);
            this.prevButton.Name = "prev";
            this.prevButton.Image = prevImg;
            this.prevButton.IconSize = iconSize;
            this.prevButton.TabStop = false;
            // 
            // playPouse
            // 
            //System.Resources.ResourceManager rm = new System.Resources.ResourceManager("resources", typeof(Listen2MeApplication).Assembly);
            //object icon = rm.GetObject("play-solid");
            this.playPauseButton.Margin = new Padding(0);
            this.playPauseButton.Name = "playPouse";
            this.playPauseButton.Image = playImg;
            this.playPauseButton.IconSize = iconSize;
            this.playPauseButton.TabStop = false;
            // 
            // next
            // 
            this.nextButton.Margin = new Padding(0);
            this.nextButton.Name = "next";
            this.nextButton.Image = nextImg;
            this.nextButton.IconSize = iconSize;
            this.nextButton.TabStop = false;
            //
            // volumeButton
            //
            this.volumeButton.Margin = new Padding(0);
            this.volumeButton.Name = "next";
            this.volumeButton.Image = volumeImg;
            this.volumeButton.IconSize = iconSize;
            this.volumeButton.TabStop = false;
            // 
            // multimediaCover
            // 
            this.multimediaCover.BackColor = Color.Transparent;
            this.multimediaCover.Location = new Point(0, 0);
            this.multimediaCover.Margin = new Padding(0);
            this.multimediaCover.Name = "multimediaCover";
            this.multimediaCover.Size = new Size(50, 50);
            this.multimediaCover.SizeMode = PictureBoxSizeMode.CenterImage;
            this.multimediaCover.TabStop = false;
            this.multimediaCover.WaitOnLoad = true;
            //
            // playbackProgressBar
            //
            //this.playbackProgressBar.Location = new Point(0, 0);
            //this.playbackProgressBar.Name = "playbackProgressBar";
            //this.playbackProgressBar.Size = new Size(50, 50);
            // 
            // Listen2MeMediaController
            // 
            this.Name = "Listen2MeMediaController";
            this.FormBorderStyle = FormBorderStyle.None;
            //this.Controls.Add(playbackProgressBar);
            this.Controls.Add(multimediaCover);
            this.Controls.Add(prevButton);
            this.Controls.Add(playPauseButton);
            this.Controls.Add(nextButton);
            this.Controls.Add(volumeButton);
            ((ISupportInitialize)(this.multimediaCover)).EndInit();
            this.ResumeLayout(false);

            this.playPauseButton.LeftClick += OnPlayPouse;
            this.prevButton.LeftClick += OnPrev;
            this.nextButton.LeftClick += OnNext;
            this.volumeButton.LeftClick += OnVolume;
            this.volumeButton.MouseWheel += OnVolumeChange;
            //this.multimediaCover.MouseHover += ShowMusicTooltip;
            //this.multimediaCover.MouseHover += ShowMusicTooltip;
            //this.customButton.LeftClick += OnLeftClick;

            MediaController.Instance.CurrentMediaChanged += OnMediaChanged;
            MediaController.Instance.PlaybackInfoChanged += OnPlaybackInfoChanged;
            MediaController.Instance.CurrentSessionChanged += OnCurrentSessionChanged;
            //MediaController.Instance.TimelinePropertiesChanged += OnTimelinePropertiesChanged;

            UpdateButtonsState(MediaController.Instance.GetCurrentPlaybackInfo());
            //UpdatePlaybackPosition();
            OnMediaChanged(new object(), MediaController.Instance.GetCurrentMediaInfo());
            UpdateSizesAndPositions();
        }

        private void SetButtonsState(bool state)
        {
            this.playPauseButton.Enabled = state;
            this.prevButton.Enabled = state;
            this.nextButton.Enabled = state;
            this.playPauseButton.Image = state ? playImg : grayedPlayImg;
            this.prevButton.Image = state ? prevImg : grayedPrevImg;
            this.nextButton.Image = state ? nextImg : grayedNextImg;
        }

        private void DisableButtons()
        {
            SetButtonsState(false);
        }

        private void EnableButtons()
        {
            SetButtonsState(true);
        }

        private void UpdateButtonsState(GlobalSystemMediaTransportControlsSessionPlaybackInfo pbInfo)
        {
            if (pbInfo == null)
            {
                return;
            }

            if (pbInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed)
            {
                DisableButtons();
                return;
            }

            EnableButtons();

            if (pbInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
            {
                this.playPauseButton.Image = pauseImg;
                return;
            }

            this.playPauseButton.Image = playImg;
        }

        private void UpdateMediaInfo(MediaController.MediaInfo newInfo)
        {
            Image img = newInfo.Thumbnail;
            if (img == null)
                return;

            int height = __taskbar.Size.Height;
            //int height = 300;
            int canvasMin = Math.Min(height, height);
            int imageMin = Math.Min(img.Width, img.Height);
            Bitmap newImg = ImageHelper.ResizeImage(img, (float)imageMin / (float)canvasMin);

            if(!IsAnimating)
            {
                PictureBox animatorBox = new PictureBox();
                IsAnimating = true;
                animatorBox.Size = this.multimediaCover.Size;
                animatorBox.Image = this.multimediaCover.Image;
                this.Controls.Add(animatorBox);
                animatorBox.BringToFront();
                StartPictureBoxAnimation(animatorBox);
            }

            songInfo = "Song info";
            this.multimediaCover.Size = new Size(newImg.Size.Height, newImg.Size.Height);
            this.multimediaCover.Image = newImg;
        }

        private void StartPictureBoxAnimation(PictureBox box)
        {
            AnimationUtil.AnimateValueLowPass(new Point(0, 0), new Point(0, box.Size.Height), 70,
                (Action<Point>)delegate (Point pt) { this.Invoke((Action<Point>)delegate (Point pt) {
                    box.Location = new Point(pt.X, pt.Y);
                }, pt); },
                (Action)delegate () { this.Invoke((Action)delegate () {
                    box.Dispose();
                    IsAnimating = false;
                }); });
        }

        //private void UpdatePlaybackPosition()
        //{
        //    TimeSpan timeSpan = MediaController.Instance.CurrentPositon();
        //    if (timeSpan == TimeSpan.Zero)
        //        return;

        //    this.playbackProgressBar.Value = (int)Math.Floor(timeSpan.Milliseconds * 100 / timeSpan.TotalMilliseconds);
        //}

        private void OnMediaChanged(object sender, MediaController.MediaInfo newInfo)
        {
            this.Invoke((Action)delegate () { this.UpdateMediaInfo(newInfo); });
        }

        private void OnPlaybackInfoChanged(object sender, GlobalSystemMediaTransportControlsSessionPlaybackInfo pbInfo)
        {
            this.Invoke((Action)delegate () { this.UpdateButtonsState(pbInfo); });
        }

        private void OnCurrentSessionChanged(object sender, CurrentSessionChangedEventArgs e)
        {
            OnMediaChanged(new object(), MediaController.Instance.GetCurrentMediaInfo());
        }

        //private void OnTimelinePropertiesChanged(object sender, TimelinePropertiesChangedEventArgs e)
        //{
        //    this.Invoke((Action)delegate () { this.UpdatePlaybackPosition(); });
        //}

        private void OnPlayPouse(object obj, ClickEventArgs args)
        {
            MediaController.Instance.PlayPause();
        }

        private void OnPrev(object obj, ClickEventArgs args)
        {
            MediaController.Instance.Previous();
        }

        private void OnNext(object obj, ClickEventArgs args)
        {
            MediaController.Instance.Next();
        }

        private void OnVolume(object obj, ClickEventArgs args)
        {
            Win32ApiHelper.Mute(Handle);
        }

        private void OnVolumeChange(object obj, MouseEventArgs args)
        {
            if (args.Delta > 0)
                Win32ApiHelper.VolumeUp(Handle);
            else
                Win32ApiHelper.VolumeDown(Handle);
        }

        //private void ShowMusicTooltip(object obj, EventArgs args)
        //{
        //    Form toolTipForm = new Form();

        //}

        private void UpdateSizesAndPositions()
        {
            const int button_width = 26;
            int width = 0;
            int height = __taskbar.Size.Height;
            //int height = 300;

            width += multimediaCover.Size.Width;

            prevButton.Size = new Size(button_width, height);
            prevButton.Location = new Point(width, 0);
            width += prevButton.Size.Width;

            playPauseButton.Size = new Size(button_width, height);
            playPauseButton.Location = new Point(width, 0);
            width += playPauseButton.Size.Width;

            nextButton.Size = new Size(button_width, height);
            nextButton.Location = new Point(width, 0);
            width += nextButton.Size.Width;

            volumeButton.Size = new Size(button_width, height);
            volumeButton.Location = new Point(width, 0);
            width += volumeButton.Size.Width;

            //playbackProgressBar.Size = new Size(width - multimediaCover.Size.Width, multimediaCover.Size.Height);
            //playbackProgressBar.Location = new Point(multimediaCover.Size.Width, 0);

            this.Size = new Size(width, height);
            this.Location = new Point(__taskbar.Size.Width - this.Width, 0);
        }

        private Image playImg = Image.FromFile("./Resources/play.png");
        private Image pauseImg = Image.FromFile("./Resources/pause.png");
        private Image nextImg = Image.FromFile("./Resources/step-forward.png");
        private Image prevImg = Image.FromFile("./Resources/step-backward.png");
        private Image volumeImg = Image.FromFile("./Resources/volume.png");

        private Image grayedPlayImg = Image.FromFile("./Resources/play-darkgray.png");
        private Image grayedPauseImg = Image.FromFile("./Resources/pause-darkgray.png");
        private Image grayedNextImg = Image.FromFile("./Resources/step-forward-darkgray.png");
        private Image grayedPrevImg = Image.FromFile("./Resources/step-backward-darkgray.png");
        private Image grayedVolumeImg = Image.FromFile("./Resources/volume-darkgray.png");

        private CustomButton prevButton;
        private CustomButton nextButton;
        private CustomButton playPauseButton;
        private CustomButton volumeButton;
        //private ProgressBar playbackProgressBar;
        private PictureBox multimediaCover;
        private static Win32Window __taskbar = Win32Window.FindWindowByRelationTree(new List<string> { "Shell_TrayWnd", "ReBarWindow32" });
        private bool IsAnimating = false;
        private int iconSize = 12;
        private string songInfo = "";
    }
}