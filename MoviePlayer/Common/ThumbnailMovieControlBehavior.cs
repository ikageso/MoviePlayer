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
    public class ThumbnailMovieControlBehavior : Behavior<ThumbnailMovieControl>
    {
        public static readonly DependencyProperty ThumbToolTipProperty = DependencyProperty.Register("ThumbToolTip", typeof(Popup), typeof(ThumbnailMovieControlBehavior));
        public Popup ThumbToolTip
        {
            get { return (Popup)this.GetValue(ThumbToolTipProperty); }
            set { this.SetValue(ThumbToolTipProperty, value); }
        }

        //public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(MediaElement), typeof(ThumbnailMovieControlBehavior));
        //public MediaElement Movie
        //{
        //    get { return (MediaElement)this.GetValue(MovieProperty); }
        //    set { this.SetValue(MovieProperty, value); }
        //}

        public static readonly DependencyProperty ThumbPointProperty = DependencyProperty.Register("ThumbPoint", typeof(double), typeof(ThumbnailMovieControlBehavior));
        public double ThumbPoint
        {
            get { return (double)this.GetValue(ThumbPointProperty); }
            set { this.SetValue(ThumbPointProperty, value); }
        }

        public static readonly DependencyProperty SliderPointProperty = DependencyProperty.Register("SliderPoint", typeof(double), typeof(ThumbnailMovieControlBehavior));
        public double SliderPoint
        {
            get { return (double)this.GetValue(SliderPointProperty); }
            set { this.SetValue(SliderPointProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeave += thumbnailMovieControl_MouseLeave;
            this.AssociatedObject.PreviewMouseLeftButtonUp += AssociatedObject_PreviewMouseLeftButtonUp;
        }

        private void AssociatedObject_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SliderPoint = ThumbPoint;

            //ThumbToolTip.IsOpen = false;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeave -= thumbnailMovieControl_MouseLeave;
        }

        private void thumbnailMovieControl_MouseLeave(object sender, MouseEventArgs e)
        {
            ThumbToolTip.IsOpen = false;
        }
    }
}
