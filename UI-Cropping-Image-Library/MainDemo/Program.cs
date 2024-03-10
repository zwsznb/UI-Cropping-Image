using FFMpegCore;
using System;
using System.Threading.Tasks;
using Yolov5Net.Scorer.Models;
using Yolov5Net.Scorer;
using System.Drawing;
using Yolo.Detect;
using Video.Helper;
using System.IO;

namespace MainDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var path = "W:/project/testmp4/output1.mp4";
            var outputPath = "W:/project/testmp4/test.mp4";
            var tmp = "W:/project/ffmpeg/ffmpeg-master-latest-win64-gpl/tmp";
            //FFMpegArguments.FromFileInput(path).OutputToFile(outputPath, false,
            //  filterOptions => filterOptions.WithCustomArgument("-vf \"crop=400:400\""))
            //  .ProcessSynchronously();

            //ffmpeg -i inputfile.mkv -vf "select=eq(n\,0)" -q:v 3 output_image.jpg
            //FFMpegArguments.FromFileInput(path).OutputToFile($"{tmp}/{DateTime.Now.ToString("yyMMddHHmmss")}.jpg", false,
            //filterOptions => filterOptions.WithCustomArgument("-vframes 1")).
            //ProcessSynchronously();
            // Console.WriteLine(FFMpegArguments.FromFileInput(path).OutputToFile($"{tmp}/{DateTime.Now.ToString("yyMMddHHmmss")}.jpg", false,
            //filterOptions => filterOptions.WithCustomArgument("-vframes 1")).Arguments);
            //Console.ReadKey();
            //using(var image = Image.FromFile("W:/IDE背景/10682.jpg"))
            //{
            //    using(var scorer = new YoloScorer<YoloCocoP5Model>("yolov5n.onnx"))
            //    {
            //        var predictions = scorer.Predict(image);

            //        foreach (var prediction in predictions) // draw predictions
            //        {
            //            var score = Math.Round(prediction.Score, 2);

            //            var (x, y) = (prediction.Rectangle.Left - 3, prediction.Rectangle.Top - 23);

            //            using (Graphics g = Graphics.FromImage(image))
            //            {
            //                Color customColor = Color.FromArgb(50, Color.Yellow);
            //                SolidBrush shadowBrush = new SolidBrush(customColor);
            //                var width = Convert.ToInt32(prediction.Rectangle.Width);
            //                var height = Convert.ToInt32(prediction.Rectangle.Height);
            //                g.FillRectangles(shadowBrush, new Rectangle[] {
            //                    new Rectangle { X=Convert.ToInt32(x),
            //                        Y=Convert.ToInt32(y),
            //                        Width=width,
            //                        Height=height} 
            //                });

            //            }
            //        }

            //        image.Save($"{tmp}/result.jpg");
            //    }
            //}
            //ImageDetection detection = new ImageDetection("W:/IDE背景/10682.jpg");
            //var a = detection.DetectSpecificObj("person");
            //VideoEncoder encoder = new VideoEncoder(path);
            //encoder.CropVideoAndEncoder($"{tmp}/{DateTime.Now.ToString("yyMMddHHmmss")}.mp4", new CropOutputSize
            //{
            //    OutHeight = 400,
            //    OutWidth = 400
            //});
            VideoDecoder decoder = new VideoDecoder(path);
            var firstFramePath = decoder.GetVideoFirstFramePath();
            ImageDetection detection = new ImageDetection(firstFramePath);
            var personPos = detection.DetectSpecificObj("person");
            VideoEncoder encoder = new VideoEncoder(path);
            encoder.CropVideoAndEncoder($"{tmp}/{DateTime.Now.ToString("yyMMddHHmmss")}.mp4", personPos);
            //删除文件
            File.Delete(firstFramePath);
        }
    }
}
