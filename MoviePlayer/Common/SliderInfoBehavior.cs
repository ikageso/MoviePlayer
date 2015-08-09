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

namespace MoviePlayer.Common
{
    public class SliderInfoBehavior : Behavior<Slider>
    {
        private TimeSpan _OldTime = TimeSpan.Zero;

        public static readonly DependencyProperty ThumbToolTipProperty = DependencyProperty.Register("ThumbToolTip", typeof(Popup), typeof(SliderInfoBehavior));
        public Popup ThumbToolTip
        {
            get { return (Popup)this.GetValue(ThumbToolTipProperty); }
            set { this.SetValue(ThumbToolTipProperty, value); }
        }

        public static readonly DependencyProperty ThumbToolTipBlockProperty = DependencyProperty.Register("ThumbToolTipBlock", typeof(TextBlock), typeof(SliderInfoBehavior));
        public TextBlock ThumbToolTipBlock
        {
            get { return (TextBlock)this.GetValue(ThumbToolTipBlockProperty); }
            set { this.SetValue(ThumbToolTipBlockProperty, value); }
        }

        public static readonly DependencyProperty ThumbToolTipBlockBorderProperty = DependencyProperty.Register("ThumbToolTipBlockBorder", typeof(Border), typeof(SliderInfoBehavior));
        public Border ThumbToolTipBlockBorder
        {
            get { return (Border)this.GetValue(ThumbToolTipBlockBorderProperty); }
            set { this.SetValue(ThumbToolTipBlockBorderProperty, value); }
        }

        public static readonly DependencyProperty ThumbMovieProperty = DependencyProperty.Register("ThumbMovie", typeof(MediaElement), typeof(SliderInfoBehavior));
        public MediaElement ThumbMovie
        {
            get { return (MediaElement)this.GetValue(ThumbMovieProperty); }
            set { this.SetValue(ThumbMovieProperty, value); }
        }

        public static readonly DependencyProperty ThumbPointProperty = DependencyProperty.Register("ThumbPoint", typeof(double), typeof(SliderInfoBehavior));
        public double ThumbPoint
        {
            get { return (double)this.GetValue(ThumbPointProperty); }
            set { this.SetValue(ThumbPointProperty, value); }
        }



        // 要素にアタッチされたときの処理。大体イベントハンドラの登録処理をここでやる
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseMove += slider_MouseMove;
            this.AssociatedObject.MouseLeave += slider_MouseLeave;
        }

        // 要素にデタッチされたときの処理。大体イベントハンドラの登録解除をここでやる
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseMove -= slider_MouseMove;
            this.AssociatedObject.MouseLeave -= slider_MouseLeave;

        }

        private void slider_MouseMove(object sender, MouseEventArgs e)
        {
            if (ThumbMovie == null)
                return;

            if (!ThumbToolTip.IsOpen) { ThumbToolTip.IsOpen = true; }

            var slider = this.AssociatedObject;

            Point currentPos = e.GetPosition(slider);
            Track _track = slider.Template.FindName("PART_Track", slider) as Track;

            var value = _track.ValueFromPoint(currentPos);
            var ts  = new TimeSpan(0, 0, 0, 0, (int)value);

            if ((_OldTime - ts).Duration() > new TimeSpan(0, 0, 0, 1))
            {

                _OldTime = ts;

                ThumbPoint = value;

                //ThumbToolTipBlock.Text = ts.ToString();
                ThumbMovie.Position = ts;
                ThumbMovie.Play();
                ThumbMovie.Pause();

            }

            ThumbToolTip.HorizontalOffset = currentPos.X - (ThumbToolTipBlockBorder.ActualWidth / 2);
            ThumbToolTip.VerticalOffset = (ThumbMovie.ActualHeight + slider.ActualHeight) * -1 -5;
        }

        private void slider_MouseLeave(object sender, MouseEventArgs e)
        {
            ThumbToolTip.IsOpen = false;
        }
    }
}
