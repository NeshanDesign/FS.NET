using iTextSharp.text;

namespace FsNet.Common.PdfUtils
{
    public class TabelRowStyle
    {
        public BaseColor BackgroundColor { get; set; } = BaseColor.WHITE;
        public FontOption FontOption { get; set; } = new FontOption("tahoma");
    }
}