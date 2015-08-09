using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using MoviePlayer.Model;

namespace MoviePlayer.Common
{
    public class MovieChangeMessage
    {
        public MovieChangeMessage(MovieFile movie)
        {
            Movie = movie;
        }
        public MovieFile Movie { get; private set; }
    }
}
