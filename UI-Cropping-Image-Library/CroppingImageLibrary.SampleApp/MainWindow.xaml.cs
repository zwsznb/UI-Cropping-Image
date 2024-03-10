using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CroppingImageLibrary.Services;
using Microsoft.Win32;
using Video.Helper;

namespace CroppingImageLibrary.SampleApp
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private CroppingWindow _croppingWindow;
        // private BitmapImage bitmapImage;
        private Bitmap sourceBitmap;
        private bool isVideo;
        private string sourcePath;
        private string videoPath;

        public MainWindow()
        {
            InitializeComponent();
            Topmost = true;
        }

        private void Button_LoadImage(object sender, RoutedEventArgs e)
        {
            if (_croppingWindow != null)
                return;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            //op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                sourcePath = op.FileName;
                if (!sourcePath.EndsWith(".mp4"))
                {
                    sourceBitmap = new Bitmap(sourcePath);
                    isVideo = false;
                }
                else
                {
                    VideoDecoder decoder = new VideoDecoder(sourcePath);
                    videoPath = sourcePath;
                    sourcePath = decoder.GetVideoFirstFramePath();
                    sourceBitmap = new Bitmap(sourcePath);
                    isVideo = true;
                }
                _croppingWindow = new CroppingWindow(new BitmapImage(new Uri(sourcePath)));
                _croppingWindow.Closed += (a, b) => _croppingWindow = null;
                _croppingWindow.Height = new BitmapImage(new Uri(sourcePath)).Height;
                _croppingWindow.Width = new BitmapImage(new Uri(sourcePath)).Width;

                _croppingWindow.Show();
            }
        }

        private void Button_SaveImage(object sender, RoutedEventArgs e)
        {
            var cropArea = _croppingWindow.CropTool.CropService.GetCroppedArea();
            LogTxt.WriteLogToTBox(LogLevel.INFO, "保存中...");
            if (!isVideo)
                SaveToImage(cropArea);
            else
            {
                SaveToVideo(cropArea);
            }
            LogTxt.WriteLogToTBox(LogLevel.INFO, "保存成功");
            //close croppingWindow
            _croppingWindow.Close();
            TempFileInit();

        }

        private void SaveToVideo(CropArea cropArea)
        {
            var tmp = "W:/project/ffmpeg/ffmpeg-master-latest-win64-gpl/tmp";
            VideoEncoder encoder = new VideoEncoder(videoPath);
            encoder.CropVideoAndEncoder($"{tmp}/{DateTime.Now.ToString("yyMMddHHmmss")}.mp4",
                new CropOutputSize
                {
                    OutWidth = cropArea.CroppedRectAbsolute.Width,
                    OutHeight = cropArea.CroppedRectAbsolute.Height,
                    X = cropArea.CroppedRectAbsolute.X,
                    Y = cropArea.CroppedRectAbsolute.Y
                });
        }

        private void SaveToImage(CropArea cropArea)
        {
            Rectangle cropRect = new Rectangle((int)cropArea.CroppedRectAbsolute.X,
            (int)cropArea.CroppedRectAbsolute.Y, (int)cropArea.CroppedRectAbsolute.Width,
               (int)cropArea.CroppedRectAbsolute.Height);

            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(sourceBitmap, new Rectangle(0, 0, target.Width, target.Height),
                    cropRect,
                    GraphicsUnit.Pixel);
            }

            //save image to file
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "TestCropping",          // Default file name
                DefaultExt = ".png",                  // Default file extension
                Filter = "Image png (.png)|*.png" // Filter files by extension
            };

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result != true)
                return;

            // Save document
            string filename = dlg.FileName;
            target.Save(filename);
        }

        private void TempFileInit()
        {
            try
            {

                File.Delete(videoPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除文件失败");
            }
            finally
            {
                videoPath = null;
            }
        }
    }
}