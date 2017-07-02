using iTextSharp.text;
using FsNet.Common.PdfUtils;

namespace FsNet.Common.PdfUtils
{
    public class PageHeaderOrFooter
    {
        // private string _template = "{{current}}/{{total}}";

        public PageHeaderOrFooter()
        {
            FontOption = new FontOption("tahoma", new FontStyle(BaseColor.BLACK, 10f));
        }

        public bool EnablePaging { get; set; } = true;
        public bool SkipFirstPage { get; set; }
        public PageSection PrintLocation { get; set; } = PageSection.Bottom;
        public Alignment Alignment { get; set; } = Alignment.Center;
        public Direction Direction { get; set; } = Direction.RightToLeft;

        /// <summary>
        /// {{current}} for current page number & {{total}} for total page count
        /// default is {{current}}/{{total}}
        /// </summary>
        public string Text { get; set; }

        public FontOption FontOption { get; set; }
    }
}