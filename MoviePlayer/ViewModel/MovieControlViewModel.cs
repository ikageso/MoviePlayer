using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MoviePlayer.Common;
using MoviePlayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MoviePlayer.ViewModel
{
    public class MovieControlViewModel : ViewModelBase
    {
        private DispatcherTimer _Timer = new DispatcherTimer();

        public MovieControlViewModel()
        {
            MessengerInstance.Register<MovieChangeMessage>(this, this.MovieChange);

            _Timer.Tick += new EventHandler(Timer_Tick);
        }

        #region property
        private MovieFile _Movie;
        /// <summary>
        /// Movie
        /// </summary>
        public MovieFile Movie
        {
            get
            {
                return _Movie;
            }
            set
            {
                _Movie = value;
                RaisePropertyChanged("Movie");
            }
        }


        private MediaElement _MovieObj;
        /// <summary>
        /// MediaElementObject
        /// </summary>
        public MediaElement MovieObj
        {
            get
            {
                return _MovieObj;
            }
            set
            {
                _MovieObj = value;
                RaisePropertyChanged("MovieObj");
            }
        }

        private double _MoviePos;
        /// <summary>
        /// MoviePos
        /// </summary>
        public double MoviePos
        {
            get
            {
                return _MoviePos;
            }
            set
            {
                _MoviePos = value;
                RaisePropertyChanged("MoviePos");

                if (MovieObj != null)
                {
                    TimeSpan ts = new TimeSpan(0, 0, 0, 0, (int)_MoviePos);
                    MovieObj.Position = ts;
                }
            }
        }

        private double _MovieMax;
        /// <summary>
        /// MovieMax
        /// </summary>
        public double MovieMax
        {
            get
            {
                return _MovieMax;
            }
            set
            {
                _MovieMax = value;
                RaisePropertyChanged("MovieMax");
            }
        }

        private double _VolumePos;
        /// <summary>
        /// VolumePos
        /// </summary>
        public double VolumePos
        {
            get
            {
                return _VolumePos;
            }
            set
            {
                _VolumePos = value;
                RaisePropertyChanged("VolumePos");

                if (MovieObj != null)
                    MovieObj.Volume = _VolumePos;
            }
        }

        private bool _IsPlay;
        /// <summary>
        /// IsPlay
        /// </summary>
        public bool IsPlay
        {
            get
            {
                return _IsPlay;
            }
            set
            {
                _IsPlay = value;
                RaisePropertyChanged("IsPlay");
            }
        }
        #endregion

        #region method
        /// <summary>
        /// MovieChange
        /// </summary>
        /// <param name="message"></param>
        public void MovieChange(MovieChangeMessage message)
        {
            Movie = message.Movie;

            if(Movie == null)
            {
                MovieObj.MediaOpened -= new System.Windows.RoutedEventHandler(MovieObj_MediaOpened);
                return;
            }

            if (MovieObj != null)
            {
                MovieObj.Close();
                MovieObj = null;
            }

            MovieObj = new MediaElement()
            {
                LoadedBehavior = MediaState.Manual,
                ScrubbingEnabled = true,
                Stretch = System.Windows.Media.Stretch.Uniform,
            };
            MovieObj.MediaOpened += new System.Windows.RoutedEventHandler(MovieObj_MediaOpened);

            MovieObj.Source = new Uri(Movie.FullPath);
            MovieObj.Pause();

        }
        #endregion

        #region event

        /// <summary>
        /// MovieObj_MediaOpened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovieObj_MediaOpened(object sender, RoutedEventArgs e)
        {
            MovieMax = MovieObj.NaturalDuration.TimeSpan.TotalMilliseconds;
            MoviePos = 0;
            MovieObj.Volume = VolumePos;

            // Update the position slider every second.
            _Timer.Interval = new TimeSpan(0, 0, 1);
            _Timer.Start();
        }

        /// <summary>
        /// Timer_Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Timer_Tick(object sender, EventArgs e)
        {
            MoviePos = MovieObj.Position.TotalMilliseconds;
        }
        #endregion

        #region command
        private RelayCommand _PlayCommand;
        /// <summary>
        /// Play
        /// </summary>
        public RelayCommand PalyCommand
        {
            get
            {
                if (_PlayCommand == null)
                    _PlayCommand = new RelayCommand(() =>
                    {
                        MovieObj.Play();
                        IsPlay = true;
                    }
                    , () =>
                     {
                         if (MovieObj == null || IsPlay)
                             return false;

                         return true;
                     }
                    );

                return _PlayCommand;
            }
        }

        private RelayCommand _StopCommand;
        /// <summary>
        /// Stop
        /// </summary>
        public RelayCommand StopCommand
        {
            get
            {
                if (_StopCommand == null)
                {
                    _StopCommand = new RelayCommand(() =>
                    {
                        MovieObj.Stop();
                        IsPlay = false;
                    }
                    , () =>
                     {
                         if (MovieObj == null || !IsPlay)
                             return false;

                         return true;
                     }
                    );
                }

                return _StopCommand;
            }
        }
        #endregion
    }
}
