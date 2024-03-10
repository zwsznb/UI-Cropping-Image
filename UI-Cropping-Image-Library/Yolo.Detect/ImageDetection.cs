

using System;
using System.Drawing;
using Video.Helper;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;

namespace Yolo.Detect
{
    public class ImageDetection
    {
        private string path;
        public ImageDetection(string path)
        {
            this.path = path;
        }
        public CropOutputSize DetectSpecificObj(string name, Action<CropOutputSize> CustomDetectSize = null)
        {
            if (path == null) throw new Exception("检测图片不能为空");
            CropOutputSize cropOutputSize = null;
            using (var image = Image.FromFile(path))
            {
                using (var scorer = new YoloScorer<YoloCocoP5Model>("yolov5n.onnx"))
                {
                    var predictions = scorer.Predict(image);

                    foreach (var prediction in predictions) // draw predictions
                    {
                        if (!prediction.Label.Name.Equals(name))
                            continue;
                        var score = Math.Round(prediction.Score, 2);

                        var (x, y) = (prediction.Rectangle.Left - 3, prediction.Rectangle.Top - 23);
                        cropOutputSize = new CropOutputSize
                        {
                            OutHeight = prediction.Rectangle.Height,
                            OutWidth = prediction.Rectangle.Width,
                            X = prediction.Rectangle.X,
                            Y = prediction.Rectangle.Y
                        };
                        break;
                    }

                }
            }
            if (cropOutputSize == null) throw new Exception("未检测到指定对象");
            if (CustomDetectSize != null) CustomDetectSize(cropOutputSize);
            return cropOutputSize;
        }
    }
}
