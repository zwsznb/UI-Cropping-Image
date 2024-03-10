using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CroppingImageLibrary.SampleApp
{
    public static class GlobalExceptionHandle
    {
        public static void WriteLogToTBox(this RichTextBox box, LogLevel level, string message)
        {
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = $"[{level.ToString()}]:{message}\r";
            if (level == LogLevel.ERROR)
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            if (level == LogLevel.WARNING)
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Yellow);
            if (level == LogLevel.INFO)
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
        }
    }
    public enum LogLevel
    {
        ERROR, WARNING, INFO
    }
}
