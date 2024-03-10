using FFMpegCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Video.Helper
{
    public class FFmpegConfig
    {
        public string BinaryFolder { get; set; }
        public string TemporaryFilesFolder { get; set; }

        public FFmpegConfig() { }
        public static FFmpegConfig LoadConfigFile()
        {
            if (!File.Exists("ffmpeg.config.json"))
            {
                return new FFmpegConfig();
            }
            return JsonSerializer.Deserialize<FFmpegConfig>(File.ReadAllText("ffmpeg.config.json"));
        }
    }
}
