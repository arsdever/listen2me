using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Text;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Control;
using Windows.Storage.Streams;

namespace listen2me
{
    using MediaControlSessionManager = GlobalSystemMediaTransportControlsSessionManager;
    using MediaControlSession = GlobalSystemMediaTransportControlsSession;
    using MediaProperties = GlobalSystemMediaTransportControlsSessionMediaProperties;
    class MediaController
    {
        public struct MediaInfo
        {
            public string AlbumArtist;
            public string AlbumTitle;
            public int AlbumTrackCount;
            public string Artist;
            public IReadOnlyCollection<string> Genres;
            public Nullable<MediaPlaybackType> PlaybackType;
            public string Subtitle;
            public Image Thumbnail;
            public string Title;
            public int TrackNumber;

            public static MediaInfo FromMediaProperties(MediaProperties props)
            {
                MediaInfo info = new MediaInfo();
                info.AlbumArtist = props.AlbumArtist;
                info.AlbumTitle = props.AlbumTitle;
                info.AlbumTrackCount = props.AlbumTrackCount;
                info.Artist = props.Artist;
                info.Genres = props.Genres;
                info.PlaybackType = props.PlaybackType;
                info.Subtitle = props.Subtitle;

                IRandomAccessStreamReference tmbStrmRef;
                if ((tmbStrmRef = props?.Thumbnail) != null)
                {
                    using (Stream imgStrm = tmbStrmRef.OpenReadAsync().AsTask().GetAwaiter().GetResult().AsStreamForRead())
                    {
                        info.Thumbnail = Image.FromStream(imgStrm);
                    }
                }

                info.Title = props.Title;
                info.TrackNumber = props.TrackNumber;
                return info;
            }
        }
        //public struct PlaybackInfo
        //{
        //    public Nullable<MediaPlaybackAutoRepeatMode> AutoRepeatMode;
        //    public string Controls;
        //    public string IsShuffleActive;
        //    public string PlaybackRate;
        //    public string PlaybackStatus;
        //    public string PlaybackType;

        //    public static PlaybackInfo FromPlaybackChagedInfo(GlobalSystemMediaTransportControlsSessionPlaybackInfo info)
        //    {
        //        PlaybackInfo pbi = new PlaybackInfo();
        //        pbi.AutoRepeatMode = info.AutoRepeatMode;
        //        pbi.Controls = info.Controls;
        //        pbi.IsShuffleActive = info.IsShuffleActive;
        //        pbi.PlaybackRate = info.PlaybackRate;
        //        pbi.PlaybackStatus = info.PlaybackStatus;
        //        pbi.PlaybackType = info.PlaybackType;
        //        return pbi;
        //    }
        //}

        public static readonly MediaController Instance = new MediaController();

        private MediaControlSessionManager sessionManager = null;
        private MediaControlSession currentSession = null;
        private MediaProperties mediaProperties = null;

        private MediaController()
        {
            Initialize();
        }

        public void Initialize()
        {
            sessionManager = MediaControlSessionManager.RequestAsync().GetAwaiter().GetResult();

            sessionManager.CurrentSessionChanged += OnCurrentSessionChanged;
            sessionManager.SessionsChanged += OnSessionsChanged;
            // Register callbacks
        }

        private MediaControlSession CurrentSession()
        {
            MediaControlSession session = sessionManager?.GetCurrentSession();
            if (currentSession != session)
            {
                currentSession = session;
                if (currentSession == null)
                {
                    return null;
                }

                currentSession.MediaPropertiesChanged += OnMediaChanged;
                currentSession.PlaybackInfoChanged += OnPlaybackInfoChanged;
                //currentSession.TimelinePropertiesChanged += OnTimelinePropertiesChanged;
            }

            return currentSession;
        }

        private MediaProperties CurrentMediaProperties()
        {
            return CurrentSession()?.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
        }

        public MediaInfo GetCurrentMediaInfo()
        {
            mediaProperties = CurrentMediaProperties();
            if (mediaProperties == null)
                return new MediaInfo();

            return MediaInfo.FromMediaProperties(mediaProperties);
        }

        public GlobalSystemMediaTransportControlsSessionPlaybackInfo GetCurrentPlaybackInfo()
        {
            return CurrentSession()?.GetPlaybackInfo();
        }

        private void OnMediaChanged(MediaControlSession sender, MediaPropertiesChangedEventArgs args)
        {
            mediaProperties = sender.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
            CurrentMediaChanged(sender, MediaInfo.FromMediaProperties(mediaProperties));
        }

        private void OnPlaybackInfoChanged(MediaControlSession sender, PlaybackInfoChangedEventArgs args)
        {
            GlobalSystemMediaTransportControlsSessionPlaybackInfo playbackInfo = CurrentSession()?.GetPlaybackInfo();
            PlaybackInfoChanged?.Invoke(sender, playbackInfo);
        }

        private void OnCurrentSessionChanged(MediaControlSessionManager sender, CurrentSessionChangedEventArgs arg)
        {
            CurrentSessionChanged?.Invoke(sender, arg);
        }

        private void OnSessionsChanged(MediaControlSessionManager sender, SessionsChangedEventArgs arg)
        {
            SessionsChanged?.Invoke(sender, arg);
        }

        //private void OnTimelinePropertiesChanged(MediaControlSession sender, TimelinePropertiesChangedEventArgs arg)
        //{
        //    TimelinePropertiesChanged?.Invoke(sender, arg);
        //}

        public TimeSpan CurrentPositon()
        {
            GlobalSystemMediaTransportControlsSessionTimelineProperties tp = CurrentSession()?.GetTimelineProperties();
            if (tp == null)
                return new TimeSpan();

            return  tp.Position;
        }

        public void PlayPause()
        {
            GlobalSystemMediaTransportControlsSessionPlaybackInfo playbackInfo = CurrentSession()?.GetPlaybackInfo();
            if (playbackInfo == null)
                return;

            CurrentSession().TryTogglePlayPauseAsync().AsTask().GetAwaiter();
            //if (playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
        }

        public void Previous()
        {
            CurrentSession().TrySkipPreviousAsync().AsTask().GetAwaiter();
        }

        public void Next()
        {
            CurrentSession().TrySkipNextAsync().AsTask().GetAwaiter();
        }

        public delegate void MediaChangedHandler(MediaControlSession sender, MediaInfo newInfo);
        public delegate void PlaybackInfoChangedHandler(MediaControlSession sender, GlobalSystemMediaTransportControlsSessionPlaybackInfo newInfo);
        public delegate void CurrentSessionChangedHandler(MediaControlSessionManager sender, CurrentSessionChangedEventArgs e);
        public delegate void SessionsChangedHandler(MediaControlSessionManager sender, SessionsChangedEventArgs e);
        //public delegate void TimelineChangedHandler(MediaControlSession sender, TimelinePropertiesChangedEventArgs e);

        public event MediaChangedHandler CurrentMediaChanged;
        public event PlaybackInfoChangedHandler PlaybackInfoChanged;
        public event CurrentSessionChangedHandler CurrentSessionChanged;
        public event SessionsChangedHandler SessionsChanged;
        //public event TimelineChangedHandler TimelinePropertiesChanged;
    }
}
