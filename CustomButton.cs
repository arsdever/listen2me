using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace application
{
    class CustomButton : UserControl
    {
        public CustomButton()
        {
            BackColor = Color.Transparent;
            InitializeComponents();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            isHover = true;
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHover = false;
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool newHoverState = !(Math.Min(e.Location.X, e.Location.Y) < 0);
            if (isHover != newHoverState)
            {
                isHover = newHoverState;
                //HoverChanged?.Invoke(this, isHover);
                Refresh();
            }
        }



        protected override void OnMouseDown(MouseEventArgs e)
        {
            IsDown = true;
            clickStarted = DateTime.Now;
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            clickFinished = DateTime.Now;
            IsDown = false;

            ClickEventArgs args = new ClickEventArgs(
                new System.Windows.Point(e.Location.X, e.Location.Y),
                new System.Windows.Duration(clickFinished - clickStarted)
                );

            if (isHover)
                LeftClick?.Invoke(this, args);

            Refresh();
        }

        private void InitializeComponents()
        {
            ImageChanged += OnImageChanged;
            IconSizeChanged += OnIconSizeChanged;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Image == null)
                return;

            float canvasMin = IconSize;// * Image.VerticalResolution / e.Graphics.DpiX;
            float imageMin = (float)Image.Height;
            Bitmap newImg = ImageHelper.ResizeImage(Image, (float)imageMin / (float)canvasMin);

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            PointF offset = new PointF((e.ClipRectangle.Width - newImg.Width) / 2.0f, (e.ClipRectangle.Height - newImg.Height) / 2.0f);

            e.Graphics.DrawImage(newImg, offset);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Color clr;
            if (IsDown && isHover)
            {
                clr = MouseDownBackColor;
            }
            else if (IsDown || isHover)
            {
                clr = MouseOverBackColor;
            }
            else
            {
                clr = BackColor;
            }

            e.Graphics.Clear(clr);
        }

        private void OnImageChanged(object sender, Image img)
        {
            Refresh();
        }
        private void OnIconSizeChanged(object sender, int size)
        {
            Refresh();
        }

        public event EventHandler<ClickEventArgs> LeftClick;
        public event EventHandler<Image> ImageChanged;
        public event EventHandler<int> IconSizeChanged;
        //public event EventHandler<bool> HoverChanged;

        public Image Image
        {
            get { return __image; }
            set
            {
                if (__image != value)
                {
                    __image = value;
                    ImageChanged?.Invoke(this, __image);
                }
            }
        }
        public int IconSize
        {
            get { return __iconSize; }
            set
            {
                if (__iconSize != value)
                {
                    __iconSize = value;
                    IconSizeChanged?.Invoke(this, __iconSize);
                }
            }
        }

        private Image __image;
        private int __iconSize = 16;

        public Color MouseDownBackColor = Color.FromArgb(32, 255, 255, 255);
        public Color MouseOverBackColor = Color.FromArgb(50, 255, 255, 255);

        private bool isHover = false;
        private bool IsDown = false;
        private DateTime clickStarted;
        private DateTime clickFinished;
    }
}
