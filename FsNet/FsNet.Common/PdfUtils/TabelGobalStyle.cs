namespace FsNet.Common.PdfUtils
{
    public class TabelGobalStyle
    {
        public TabelGobalStyle(int columnCount)
        {
            HasAlternateRow = false;
        }

        public TabelGobalStyle(TabelRowStyle headerStyle)
        {

        }

        public TabelGobalStyle(TabelRowStyle headerStyle, TabelRowStyle generalRowStyle)
        {

        }

        public TabelGobalStyle(TabelRowStyle headerStyle, TabelRowStyle generalRowStyle, TabelRowStyle alternateRowStyle)
        {

        }

        public TabelGobalStyle(TabelRowStyle headerStyle, TabelRowStyle generalRowStyle, TabelRowStyle alternateRowStyle, Direction dDirection)
        {

        }

        public Direction Direction { get; set; } = Direction.Bidiractional;

        public TabelRowStyle TitleStyle { get; set; }
        public TabelRowStyle HeaderStyle { get; set; }
        public TabelRowStyle GeneralRowStyle { get; set; }
        public TabelRowStyle AlternateRowStyle { get; set; }
        public bool HasAlternateRow { get; internal set; }
    }
}