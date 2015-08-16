using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MoviePlayer.Model
{
    public class MovieFileList : ObservableCollection<MovieFile>
    {
        public MovieFileList()
        {
            ThumbnaileFolderName = "Thumbnailes";
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(this, new object());
        }

        private string _MovieFolder;
        /// <summary>
        /// MovieFolder
        /// </summary>
        public string MovieFolder
        {
            get
            {
                return _MovieFolder;
            }
            set
            {
                _MovieFolder = value;

                this.Clear();

                if (Directory.Exists(_MovieFolder))
                {

                    var list1 = Directory.GetFiles(this.MovieFolder, "*.wmv");
                    var list2 = Directory.GetFiles(this.MovieFolder, "*.mp4");
                    var list3 = Directory.GetFiles(this.MovieFolder, "*.flv");

                    foreach (string fileName in list1.Concat(list2).Concat(list3).ToArray())
                    {
                        this.Add(new MovieFile()
                        {
                            FullPath = fileName,
                            FileName = Path.GetFileName(fileName),
                            ThumbFileName = Path.GetFileNameWithoutExtension(fileName) + ".jpg",
                            Time = TimeSpan.Zero,
                        });
                    }
                }

                GetFileInfo();
            }
        }
        public string ThumbnaileFolderName { get; set; }

        /// <summary>
        /// 再生時間とタイトルのサムネイルを取得
        /// </summary>
        public void GetFileInfo()
        {
            Task.Run(async () =>
            {
                foreach (var s in this)
                {
                    try
                    {
                        var me = new MediaPlayer()
                        {
                            ScrubbingEnabled = true,
                            Volume = 0
                        };

                        me.Open(new Uri(s.FullPath, UriKind.Absolute));

                        // 読み込みが完了するまで待機
                        while (me.DownloadProgress < 1.0 && me.NaturalVideoWidth == 0)
                        {
                            Thread.Sleep(100);
                        }

                        me.Pause();

                        double pos = me.NaturalDuration.TimeSpan.TotalMilliseconds / 2;

                        me.Position = TimeSpan.FromSeconds(pos);

                        // リサイズ後のサイズを計算
                        var ratio = (double)100 / me.NaturalVideoWidth;

                        int width = (int)(me.NaturalVideoWidth * ratio);
                        int height = (int)(me.NaturalVideoHeight * ratio);

                        // 描画用のVisual
                        var visual = new DrawingVisual();

                        using (var context = visual.RenderOpen())
                        {
                            context.DrawVideo(me, new System.Windows.Rect(0, 0, width, height));
                        }

                        // レンダリングするビットマップ
                        var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);

                        // レンダリング
                        bitmap.Render(visual);

                        var bitmapImage = new BitmapImage();
                        var bitmapEncoder = new PngBitmapEncoder();
                        bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmap));

                        using (var stream = new MemoryStream())
                        {
                            bitmapEncoder.Save(stream);
                            stream.Seek(0, SeekOrigin.Begin);

                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                            bitmapImage.Freeze();// BitmapImageの操作はUIで行われる。Freezeすることで、例外:「DependencySource は、DependencyObject と同じ Thread 上で作成する必要があります。」を回避する。
                        }

                        s.Image = bitmapImage;
                    }
                    catch
                    {
                    }
                }

                ////
                //// Shell32
                ////
                //Shell32.Folder folder = null;
                //Type wshell = Type.GetTypeFromProgID("Shell.Application");
                //object wshInstance = Activator.CreateInstance(wshell);
                //string someDirectory = this.MovieFolder;

                //foreach (var s in this)
                //{

                //    // 再生時間取得
                //    try
                //    {
                //        string someFile = Path.GetFileName(s.FileName);

                //        folder =
                //        (Shell32.Folder)wshell.InvokeMember("NameSpace",
                //        System.Reflection.BindingFlags.InvokeMethod, null, wshInstance, new object[] { someDirectory });

                //        s.Shell32 = new List<Shell32Info>();
                //        Shell32.FolderItem folderitem = folder.ParseName(someFile);

                //        for (int i = 0; i < 1000; i++)
                //        {

                //            string name = folder.GetDetailsOf(null, i);
                //            if (string.IsNullOrEmpty(name))
                //                continue;

                //            string value = folder.GetDetailsOf(folderitem, i);

                //            s.Shell32.Add(new Shell32Info()
                //            {
                //                Id = i,
                //                Property = name,
                //                Value = value,
                //            });

                //            // 再生時間の表示。ただし7上。XPでは 27 ではなく 21 となる。
                //            if (i == 27)
                //            {
                //                TimeSpan ts = TimeSpan.Zero;
                //                TimeSpan.TryParse(value, out ts);
                //                s.Time = ts;

                //                break;
                //            }

                //        }
                //    }
                //    catch { }


                //    // サムネイル作成

                //    var thumbnailFolder = this.MovieFolder + "\\" + this.ThumbnaileFolderName + "\\" + s.FileName;
                //    if (!Directory.Exists(thumbnailFolder))
                //        Directory.CreateDirectory(thumbnailFolder);

                //    var thumbnailFileName = thumbnailFolder + "\\" + Path.GetFileNameWithoutExtension(s.FileName) + ".jpg";

                //    int pos = s.Time.Seconds / 2;

                //    //Processオブジェクトを作成
                //    System.Diagnostics.Process p = new System.Diagnostics.Process();

                //    //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
                //    p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
                //    //出力を読み取れるようにする
                //    p.StartInfo.UseShellExecute = false;
                //    //ウィンドウを表示しないようにする
                //    p.StartInfo.CreateNoWindow = true;
                //    // コマンドライン文字列作成
                //    string sCmd = string.Format("ffmpeg\\ffmpeg.exe -ss {0} -i \"{1}\" -vframes 1 -f image2 \"{2}\"", pos, s.FullPath, thumbnailFileName);

                //    //コマンドラインを指定（"/c"は実行後閉じるために必要）
                //    p.StartInfo.Arguments = @"/c " + sCmd;

                //    //開始
                //    p.Start();

                //    //プロセス終了まで待機する
                //    p.WaitForExit();

                //    if (File.Exists(thumbnailFileName))
                //    {
                //        BitmapImage bmpImage = new BitmapImage();
                //        FileStream stream = File.OpenRead(thumbnailFileName);
                //        bmpImage.BeginInit();
                //        bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                //        bmpImage.StreamSource = stream;
                //        bmpImage.EndInit();
                //        bmpImage.Freeze();// BitmapImageの操作はUIで行われる。Freezeすることで、例外:「DependencySource は、DependencyObject と同じ Thread 上で作成する必要があります。」を回避する。
                //        stream.Close();

                //        //App.Current.Dispatcher.Invoke(() => s.Image = bmpImage);
                //        s.Image = bmpImage;
                //    }
                //    }

                await Task.Delay(30);

            });

        }
    }

}
