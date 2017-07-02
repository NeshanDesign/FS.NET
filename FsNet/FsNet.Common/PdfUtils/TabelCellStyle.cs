using System.Text;
using iTextSharp.text;

namespace FsNet.Common.PdfUtils
{
    public class TabelCellStyle
    {
        public Direction Direction { get; set; } = Direction.Bidiractional;
        public BaseColor BackgroundColor { get; set; } = BaseColor.WHITE;
        public FontOption FontOption { get; set; } = new FontOption("tahoma");
    }
}
