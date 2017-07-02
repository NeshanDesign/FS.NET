using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FsNet.Common.PdfUtils
{
    public class FontOption
    {
        public FontOption(string fontName)
            : this(fontName, false, new FontStyle())
        {

        }

        public FontOption(string fontName, bool embedIntoFile)
            : this(fontName, embedIntoFile, new FontStyle())
        {

        }

        public FontOption(string fontName, FontStyle fontStyle)
            : this(fontName, false, fontStyle)
        {

        }

        public FontOption(string fontName, bool embedIntoFile, FontStyle fontStyle)
        {
            if (embedIntoFile)
            {
                var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), fontName.ToLowerInvariant() + ".ttf");
                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font = new Font(baseFont, fontStyle.Size, (int)fontStyle.Weight, fontStyle.Color);
            }
            else
            {
                Font = FontFactory.GetFont(fontName, fontStyle.Size, (int)fontStyle.Weight, fontStyle.Color);
            }
        }

        public FontOption(BaseFont font)
            : this(font, new FontStyle())
        {

        }

        public FontOption(BaseFont font, FontStyle fontStyle)
        {
            Font = new Font(font, fontStyle.Size, (int)fontStyle.Weight, fontStyle.Color);
        }



        public Font Font { get; set; }
    }
}