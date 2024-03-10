using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CroppingImageLibrary.SampleApp
{
    public static class GlobalExceptionHandle
    {
        public static void WriteLogToTBox(this TextBox box, LogLevel level, string error)
        {
            if (level == LogLevel.ERROR)
                box.Foreground = Brushes.Red;
            if (level == LogLevel.WARNING)
                box.Foreground = Brushes.Yellow;
            if (level == LogLevel.INFO)
                box.Foreground = Brushes.Green;
            box.AppendText($"[{level.ToString()}]:{error}\r\n");
        }
    }
    public enum LogLevel
    {
        ERROR, WARNING, INFO
    }
}
