using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace application
{
    public static class AnimationUtil
    {
        public static void AnimateValueLinear(Point begin, Point end, int durationMS, Action<Point> onValueChange, Action onFinish)
        {
            new Task(() => {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                while (stopWatch.ElapsedMilliseconds < durationMS)
                {
                    double timepoint = (double)stopWatch.ElapsedMilliseconds / (double)durationMS;
                    int currentPointX = (int)((end.X - begin.X) * timepoint + begin.X);
                    int currentPointY = (int)((end.Y - begin.Y) * timepoint + begin.Y);
                    Point currentPoint = new Point(currentPointX, currentPointY);
                    onValueChange(currentPoint);
                }

                onFinish?.Invoke();
            }).Start();
        }
        public static void AnimateValueLowPass(Point begin, Point end, int slowness, Action<Point> onValueChange, Action onFinish)
        {
            new Task(() => {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                double currentPointX = begin.X;
                double currentPointY = begin.Y;

                while (stopWatch.ElapsedMilliseconds < 500)
                {
                    currentPointX += ((double)end.X - (double)currentPointX) / (double)slowness;
                    currentPointY += ((double)end.Y - (double)currentPointY) / (double)slowness;
                    Point currentPoint = new Point((int)currentPointX, (int)currentPointY);
                    onValueChange(currentPoint);
                }

                onFinish?.Invoke();
            }).Start();
        }
    }
}
