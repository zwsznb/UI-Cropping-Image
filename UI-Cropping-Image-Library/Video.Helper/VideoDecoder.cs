

using FFMpegCore;
using System;

namespace Video.Helper
{
    public class VideoDecoder
    {
        private string path;
        private FFmpegConfig config = FFmpegConfig.LoadConfigFile();
        private string tempPath;
        public VideoDecoder(string path)
        {
            this.path = path;
        }
        public string GetVideoFirstFramePath()
        {
            tempPath = $"{config.TemporaryFilesFolder}/{DateTime.Now.ToString("yyMMddHHmmss")}.jpg";
            try
            {
                FFMpegArguments.FromFileInput(path).OutputToFile(tempPath, false,
                filterOptions => filterOptions.WithCustomArgument("-vframes 1")).
                ProcessSynchronously();
            }
            catch (Exception e)
            {
                throw new Exception("获取第一帧失败");
            }
            return tempPath;
        }
    }
}
