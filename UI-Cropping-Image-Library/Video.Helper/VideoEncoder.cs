using FFMpegCore;
using System;

namespace Video.Helper
{
    public class VideoEncoder
    {
        private string path;
        public VideoEncoder(string path)
        {
            this.path = path;
        }
        public void CropVideoAndEncoder(string outputPath, CropOutputSize cropOutputSize)
        {
            try
            {
                string cropArg = CreateCropArg(cropOutputSize);
                var processor = FFMpegArguments.FromFileInput(path).OutputToFile(outputPath, false,
                  filterOptions => filterOptions.WithCustomArgument(cropArg));
                var arg = processor.Arguments;
                processor.ProcessSynchronously();
            }
            catch (Exception ex)
            {
                throw new Exception("VideoEncoder.CropVideoAndEncoder报错");
            }

        }

        private string CreateCropArg(CropOutputSize cropOutputSize)
        {
            var cropArg = $"crop={cropOutputSize.OutWidth}:{cropOutputSize.OutHeight}";
            if (!cropOutputSize.OutWidth.HasValue || !cropOutputSize.OutHeight.HasValue)
                throw new Exception("OutWidth,OutHeight不能为空");
            if (!cropOutputSize.X.HasValue)
                cropArg += cropOutputSize.X;
            if (!cropOutputSize.Y.HasValue)
                cropArg += cropOutputSize.Y;
            return $"-vf \"{cropArg}\"";
        }
    }
    public class CropOutputSize
    {
        public double? OutWidth { get; set; }
        public double? OutHeight { get; set; }
        //position
        public double? X { get; set; }
        public double? Y { get; set; }
    }
}
