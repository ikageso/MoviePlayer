using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoviePlayer.Common;
using MoviePlayer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MoviePlayer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            MovieList = new MovieFileList();
            BindingOperations.EnableCollectionSynchronization(this.MovieList, new object());
        }

        private MovieFileList _MovieList;
        /// <summary>
        /// MovieList
        /// </summary>
        public MovieFileList MovieList
        {
            get
            {
                return _MovieList;
            }
            set
            {
                _MovieList = value;
                RaisePropertyChanged("MovieList");
            }
        }

        private MovieFile _SelectedItem;
        /// <summary>
        /// SelectedItem
        /// </summary>
        public MovieFile SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                _SelectedItem = value;
                RaisePropertyChanged("SelectedItem");

                // MediaElementに動画をセット
                MessengerInstance.Send<MovieChangeMessage>(new MovieChangeMessage(_SelectedItem));

            }
        }


        private RelayCommand _SelectFolderCommand;
        /// <summary>
        /// SelectFolderCommand
        /// </summary>
        public RelayCommand SelectFolderCommand
        {
            get
            {
                if (_SelectFolderCommand == null)
                    _SelectFolderCommand = new RelayCommand(() =>
                    {
                        //MovieList.MovieFolder = @"d:\home\Movies";


                        var dlg = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
                        dlg.Title = "フォルダの選択";
                        dlg.IsFolderPicker = true;
                        dlg.InitialDirectory = "C:";

                        dlg.AddToMostRecentlyUsedList = false;
                        dlg.AllowNonFileSystemItems = false;
                        dlg.DefaultDirectory = "C:";
                        dlg.EnsureFileExists = true;
                        dlg.EnsurePathExists = true;
                        dlg.EnsureReadOnly = false;
                        dlg.EnsureValidNames = true;
                        dlg.Multiselect = false;
                        dlg.ShowPlacesList = true;

                        if (dlg.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
                        {
                            MovieList.MovieFolder = dlg.FileName;
                        }

                    });

                return _SelectFolderCommand;
            }
        }
    }
}