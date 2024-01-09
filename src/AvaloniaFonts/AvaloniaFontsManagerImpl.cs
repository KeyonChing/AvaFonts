using Avalonia.Media;
using Avalonia.Platform;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Avalonia.Benchmarks
{
    public class AvaloniaFontsManagerImpl : IFontManagerImpl
    {
        List<string> AppFonts;
        public AvaloniaFontsSettings FontsSettings { set; get; }
        public List<FontInfo> InstalledFont { set; get; }

        public AvaloniaFontsManagerImpl()
        {
            AppFonts = new List<string>();
            InstalledFont = new List<FontInfo>();

            FontsSettings = new AvaloniaFontsSettings()
            {
                DefaultFontDirectory = AppDomain.CurrentDomain.BaseDirectory + "/AvaFonts",
                DefaultFontName = "",
            };

            GetInstalledFonts();
            FontsSettings.DefaultFontName = InstalledFont[0].FontName;
        }
        public AvaloniaFontsManagerImpl(AvaloniaFontsSettings settings)
        {
            AppFonts = new List<string>();
            InstalledFont = new List<FontInfo>();

            if (settings != null)
            {
                FontsSettings = settings;
                if (FontsSettings.DefaultFontDirectory == null || FontsSettings.DefaultFontDirectory.Trim() == "")
                    FontsSettings.DefaultFontDirectory = AppDomain.CurrentDomain.BaseDirectory + "/AvaFonts";
            }
            else
            {
                FontsSettings = new AvaloniaFontsSettings()
                {
                    DefaultFontDirectory = AppDomain.CurrentDomain.BaseDirectory + "/AvaFonts",
                    DefaultFontName = "",
                };
            }

            GetInstalledFonts();


            if (FontsSettings.DefaultFontName == null || FontsSettings.DefaultFontName.Trim() == "")
                FontsSettings.DefaultFontName = InstalledFont[0].FontName;
        }

        private void GetInstalledFonts()
        {
            try
            {
                DirectoryInfo FontDi = new DirectoryInfo(FontsSettings.DefaultFontDirectory);
                FileInfo[] fis = FontDi.GetFiles();
                foreach (FileInfo file in fis)
                {
                    if (file.Extension.ToLower() == ".ttf")
                    {
                        InstalledFont.Add(new FontInfo() { FontName = System.IO.Path.GetFileNameWithoutExtension(file.FullName), FontPath = file.FullName, FontWeight = GetFontWeight(file), FontStyle = GetFontStyle(file), RealFontName = GetFontName(file), SubFonts = new List<FontInfo>() });
                    }
                }

                DirectoryInfo[] dis = FontDi.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    if (di.Name.ToLower().EndsWith(".ttf"))
                    {
                        InstalledFont.Add(new FontInfo() { FontName = di.Name.TrimEnd(".ttf".ToArray()), SubFonts = GetSubFonts(di) });
                    }
                }
            }
            catch { }
        }



        private List<FontInfo> GetSubFonts(DirectoryInfo di)
        {
            try
            {
                List<FontInfo> SubFonts = new List<FontInfo>();


                FileInfo[] fis = di.GetFiles();
                foreach (FileInfo file in fis)
                {
                    if (file.Extension.ToLower() == ".ttf")
                    {
                        SubFonts.Add(new FontInfo() { FontName = di.Name, FontPath = file.FullName, FontWeight = GetFontWeight(file), FontStyle = GetFontStyle(file), RealFontName = GetFontName(file), SubFonts = new List<FontInfo>() });
                    }
                }

                return SubFonts;
            }
            catch { return new List<FontInfo>(); }
        }

        private string GetFontName(FileInfo file)
        {
            try
            {
                return new Skia.GlyphTypefaceImpl(SKTypeface.FromFile(file.FullName), FontSimulations.None).FamilyName;
            }
            catch { return ""; }
        }
        private FontStyle GetFontStyle(FileInfo file)
        {
            try
            {
                return new Skia.GlyphTypefaceImpl(SKTypeface.FromFile(file.FullName), FontSimulations.None).Style;
            }
            catch { return FontStyle.Normal; }
        }

        private FontWeight GetFontWeight(FileInfo file)
        {
            try
            {
                return new Skia.GlyphTypefaceImpl(SKTypeface.FromFile(file.FullName), FontSimulations.None).Weight;
            }
            catch { return FontWeight.Normal; }
        }


        public string GetDefaultFontFamilyName()
        {
            try
            {
                return FontsSettings.DefaultFontName;
            }
            catch { return ""; }
        }

        public string[] GetInstalledFontFamilyNames(bool checkForUpdates = false)
        {
            try
            {
                return InstalledFont.Select(x => x.FontName).ToArray();

            }
            catch { return null; }
        }
 private IGlyphTypeface GetFont(string fontName, FontStyle style, FontWeight weight)
 {
     try
     {
         SKTypeface skTypeface;

         FontInfo fontif = InstalledFont.Find(x => x.FontName == fontName);
         if (fontif.SubFonts.Count > 0)
         {
             var f = fontif.SubFonts.Find(x => (x.FontWeight == weight && x.FontStyle == style));
             if (f != null)//是否全部符合
             {
                 skTypeface = SKTypeface.FromFile(f.FontPath);
                 return new Skia.GlyphTypefaceImpl(skTypeface, FontSimulations.None);
             }
             else
             {//第二侧重字体粗细，是否符合当前粗细
                 f = fontif.SubFonts.Find(x => (x.FontWeight == weight));
                 if (f != null)
                 {
                     skTypeface = SKTypeface.FromFile(f.FontPath);
                     return new Skia.GlyphTypefaceImpl(skTypeface, style == FontStyle.Normal ? FontSimulations.None : FontSimulations.Oblique);
                 }
                 else
                 {//第三侧重字体样式，是否符合当前样式
                     f = fontif.SubFonts.Find(x => (x.FontStyle == style && (x.FontWeight == FontWeight.Normal || x.FontWeight == FontWeight.Regular)));
                     if (f != null)
                     {
                         skTypeface = SKTypeface.FromFile(f.FontPath);
                         return new Skia.GlyphTypefaceImpl(skTypeface, weight == FontWeight.Bold ? FontSimulations.Bold : FontSimulations.None);
                     }
                     else
                     {//最后，全部不匹配，返回normal,若还没有，跳Exception
                         f = fontif.SubFonts.Find(x => (x.FontWeight == FontWeight.Normal || x.FontWeight == FontWeight.Regular));
                         skTypeface = SKTypeface.FromFile(f.FontPath);
                         return new Skia.GlyphTypefaceImpl(skTypeface, FontSimulations.None);
                     }
                 }
             }
         }
         else
         {
             skTypeface = SKTypeface.FromFile(fontif.FontPath);

             FontSimulations res = FontSimulations.None;
             if (weight != FontWeight.Normal && style != FontStyle.Normal)
                 res = FontSimulations.Oblique | FontSimulations.Bold;
             else if (style != FontStyle.Normal)
                 res = FontSimulations.Oblique;
             else if (weight != FontWeight.Normal)
                 res = FontSimulations.Bold;
             else
                 res = FontSimulations.None;


             return new Skia.GlyphTypefaceImpl(skTypeface, res);
         }
     }
     catch
     {
         return null;
     }
 }
        public bool TryCreateGlyphTypeface(string familyName, FontStyle style, FontWeight weight, FontStretch stretch, [NotNullWhen(true)] out IGlyphTypeface glyphTypeface)
        {
            SKTypeface skTypeface;
            try
            {
                switch (familyName)
                {
                    case FontFamily.DefaultFontFamilyName:
                    case "Inter":
                        glyphTypeface = GetFont(FontsSettings.DefaultFontName, style, weight);
                        break;
                    default:
                        glyphTypeface = GetFont(familyName, style, weight);
                        if (glyphTypeface == null)
                            glyphTypeface = GetFont(FontsSettings.DefaultFontName, style, weight);
                        break;
                }

                return true;
            }
            catch
            {
                glyphTypeface = null;
                return false;
            }
        }

      

        public bool TryCreateGlyphTypeface(Stream stream, [NotNullWhen(true)] out IGlyphTypeface glyphTypeface)
        {
            SKTypeface skTypeface;
            try
            {
                skTypeface = SKTypeface.FromStream(stream);
                glyphTypeface = new Skia.GlyphTypefaceImpl(skTypeface, FontSimulations.None);

                return true;
            }
            catch
            {
                glyphTypeface = null;
                return false;
            }
        }

        public bool TryMatchCharacter(int codepoint, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, CultureInfo culture, out Typeface typeface)
        {
            typeface = new Typeface(FontsSettings.DefaultFontName, fontStyle, fontWeight);

            return true;
        }
    }
}
