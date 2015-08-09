using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MoviePlayer.Model
{
    public class MovieFile : INotifyPropertyChanged
    {
        #region property
        private string _FileName;
        /// <summary>
        /// FileName
        /// </summary>
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        private string _FullPath;
        /// <summary>
        /// FullPath
        /// </summary>
        public string FullPath
        {
            get
            {
                return _FullPath;
            }
            set
            {
                _FullPath = value;
                RaisePropertyChanged("FullPath");
            }
        }

        private string _ThumbFileName;
        /// <summary>
        /// ThumbFileName
        /// </summary>
        public string ThumbFileName
        {
            get
            {
                return _ThumbFileName;
            }
            set
            {
                _ThumbFileName = value;
                RaisePropertyChanged("ThumbFileName");
            }
        }

        private BitmapImage _Image;
        /// <summary>
        /// Image
        /// </summary>
        public BitmapImage Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                RaisePropertyChanged("Image");
            }
        }

        private List<Shell32Info> _Shell32;
        /// <summary>
        /// Shell32
        /// </summary>
        public List<Shell32Info> Shell32
        {
            get
            {
                return _Shell32;
            }
            set
            {
                _Shell32 = value;
                RaisePropertyChanged("Shell32");
            }
        }

        private TimeSpan _Time;
        /// <summary>
        /// Time
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                return _Time;
            }
            set
            {
                _Time = value;
                RaisePropertyChanged("Time");
            }
        }

        #endregion

        #region PropertyChanged
        /// <summary>
        /// プロパティの変更があったときに発行される。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// PropertyChangedイベントを発行する。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var h = this.PropertyChanged;
            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

}
