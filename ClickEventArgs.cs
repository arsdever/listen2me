using System.Windows;

namespace application
{
    public class ClickEventArgs
    {
        public Point Location;
        public Duration ClickDuration;

        public ClickEventArgs(Point location, Duration clickDuration)
        {
            Location = location;
            ClickDuration = clickDuration;
        }
    }
}