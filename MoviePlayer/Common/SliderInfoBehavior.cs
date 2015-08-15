using MoviePlayer.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Threading;

namespace MoviePlayer.Common
{
    public class SliderInfoBehavior : Behavior<Slider>
    {
        private TimeSpan _OldTime = TimeSpan.Zero;
        private DispatcherTimer _PopupCloseTimer = null;

        public static readonly DependencyProperty ThumbToolTipProperty = DependencyProperty.Register("ThumbToolTip", typeof(Popup), typeof(SliderInfoBehavior));
        public Popup ThumbToolTip
        {
            get { return (Popup)this.GetValue(ThumbToolTipProperty); }
            set { this.SetValue(ThumbToolTipProperty, value); }
        }

        public static readonly DependencyProperty ThumbMovieProperty = DependencyProperty.Register("ThumbMovie", typeof(MediaElement), typeof(SliderInfoBehavior));
        public MediaElement ThumbMovie
        {
            get { return (MediaElement)this.GetValue(ThumbMovieProperty); }
            set { this.SetValue(ThumbMovieProperty, value); }
        }

        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(MediaElement), typeof(SliderInfoBehavior));
        public MediaElement Movie
        {
            get { return (MediaElement)this.GetValue(MovieProperty); }
            set { this.SetValue(MovieProperty, value); }
        }

        public static readonly DependencyProperty ThumbPointProperty = DependencyProperty.Register("ThumbPoint", typeof(double), typeof(SliderInfoBehavior));
        public double ThumbPoint
        {
            get { return (double)this.GetValue(ThumbPointProperty); }
            set { this.SetValue(ThumbPointProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseMove += slider_MouseMove;

            _PopupCloseTimer = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Render, PupupTimr_tick, this.Dispatcher);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseMove -= slider_MouseMove;
        }

        private void PupupTimr_tick(object sender, EventArgs e)
        {
            if (_PopupCloseTimer != null)
                _PopupCloseTimer.Stop();

            ThumbToolTip.IsOpen = false;
        }

        private void slider_MouseMove(object sender, MouseEventArgs e)
        {
            if (ThumbMovie == null)
                return;

            if (!ThumbToolTip.IsOpen) { ThumbToolTip.IsOpen = true; }

            if (_PopupCloseTimer != null)
            {
                _PopupCloseTimer.Stop();
                _PopupCloseTimer.Start();
            }

            var slider = this.AssociatedObject;

            Point currentPos = e.GetPosition(slider);
            Track _track = slider.Template.FindName("PART_Track", slider) as Track;

            var value = _track.ValueFromPoint(currentPos);
            var ts = new TimeSpan(0, 0, 0, 0, (int)value);

            if ((_OldTime - ts).Duration() > new TimeSpan(0, 0, 0, 1))
            {
                _OldTime = ts;

                ThumbPoint = value;

                ThumbMovie.Position = ts;
            }

            ThumbToolTip.HorizontalOffset = currentPos.X - (ThumbMovie.ActualWidth / 2);
            ThumbToolTip.VerticalOffset = (ThumbMovie.ActualHeight + slider.ActualHeight) * -1 - 10;
        }
    }
}
