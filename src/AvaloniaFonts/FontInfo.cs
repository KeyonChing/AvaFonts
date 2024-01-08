using Avalonia.Media;
using System.Collections.Generic;

namespace Avalonia.Benchmarks
{
    public class FontInfo
    {
        public string FontName { set; get; }
        public string FontPath { set; get; }
        public string RealFontName { set; get; }
        public FontWeight FontWeight { set; get; }
        public List<FontInfo> SubFonts { set; get; }
        public FontStyle FontStyle { set; get; }
    }
}