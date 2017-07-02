using iTextSharp.text;

namespace FsNet.Common.PdfUtils
{
    public class FontStyle
    {
        public FontStyle() { }
        public FontStyle(BaseColor color) : this(color, -1, FontWeight.NotInitialized) { }
        public FontStyle(BaseColor color, float size) : this(color, size, FontWeight.NotInitialized) { }

        public FontStyle(BaseColor color, float size, FontWeight fontWeight)
        {
            Color = color;
            if (size > 0) Size = size;
            if (fontWeight != FontWeight.NotInitialized) Weight = fontWeight;
        }
        public BaseColor Color { get; set; } = new BaseColor(System.Drawing.Color.Black);
        public FontWeight Weight { get; set; } = FontWeight.Normal;

        public float Size { get; set; } = 9f;
    }
}