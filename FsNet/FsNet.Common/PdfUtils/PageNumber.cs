using System;
using System.Globalization;
using iTextSharp.text;
using FsNet.Common.PdfUtils;

namespace FsNet.Common.PdfUtils
{
    public class PageNumber
    {
        private string _template = "{{current}}/{{total}}";

        public PageNumber()
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
        public string TextTemplate
        {
            get => _template;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (!_template.ToLowerInvariant().Contains("{{current}}"))
                        throw new FormatException("template must contains {{current}} keyword to print current page number.");
                    _template = value;
                }
                else
                {
                    _template = "{{current}}/{{total}}";
                }
            }
        }

        public string GetText(int current, int total)
        {
            var text = TextTemplate
                .Replace("{{current}}", current.ToString(CultureInfo.InvariantCulture))
                .Replace("{{total}}", total.ToString(CultureInfo.InvariantCulture));
            return text;
        }

     

        public FontOption FontOption { get; set; }
       
    }
}