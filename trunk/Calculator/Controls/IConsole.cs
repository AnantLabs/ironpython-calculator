using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Calculator.Controls
{
    public interface IConsole
    {
        void Clear();
        void Write(string Text);
        void WriteLine(string Text);

        void BufferedWrite(string Text);
        void WriteBuffer();

        void WriteImage(Uri location);

        void LoadRtf(string path);
        void AppendRtf(string path);
        void SaveRtf(string path);
        void SaveTxt(string path);

        Color DefaultFontColor { get; set; }
        Color ErrorFontColor { get; set; }
        Color OkFontColor { get; set; }
        Color WarningFontColor { get; set; }
        Color BackgroundColor { get; set; }
        Color CurrentForeground { get; set; }
    }
}
