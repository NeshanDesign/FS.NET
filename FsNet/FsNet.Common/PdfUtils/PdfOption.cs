namespace FsNet.Common.PdfUtils
{
    public class PdfOption
    {
        public Direction DocumentDirection { get; set; } = Direction.Bidiractional;
        public FontOption FontOption { get; set; }
        public PageHeaderOrFooter PageHeader { get; set; }
        public PageHeaderOrFooter PageFooter { get; set; }
        public PageNumber PageNumber { get; set; }
    }
}