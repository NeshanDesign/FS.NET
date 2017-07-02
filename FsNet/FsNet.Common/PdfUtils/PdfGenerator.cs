using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FsNet.Common.PdfUtils;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace FsNet.Common.PdfUtils
{
    //todo: refactoring
    /// <summary>
    /// this is the main method, only for creating a table in pdf with raw data
    /// this method needs refactoring
    /// some default padding & margins are hardcoded
    /// </summary>
    public class PdfHelper
    {
        public static string Create(IEnumerable<string> headers,
            IEnumerable<IEnumerable<string>> items, 
            string filePath, 
            string fileName, 
            string title = null)
        {
            Contract.Assert(headers != null);
            Contract.Assert(items != null);

            var headerList = headers as IList<string> ?? headers.ToList();
            var table = new PdfPTable(headerList.Count())
            {
                RunDirection = PdfWriter.RUN_DIRECTION_RTL, WidthPercentage = 100, LockedWidth = false
            };
            var columnMaxWidths = new int[headerList.Count()];
            var _path = string.Empty;
            for (var i = 0; i < columnMaxWidths.Length; i++)
            {
                columnMaxWidths[i] = 0;
            }
            var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.ttf");
            var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


            if (string.IsNullOrEmpty(title))
            {
                var tblTitle = new PdfPCell(new Phrase(title))
                {
                    PaddingTop = 10,
                    PaddingBottom = 10,
                    Colspan = headerList.Count()
                };

                table.AddCell(tblTitle);
            }

            int index = 0;
            foreach (var hd in headerList)
            {
                var font = new Font(baseFont, 7f, Font.BOLD, new BaseColor(Color.White));
                var cell = new PdfPCell(new Phrase(hd, font))
                {
                    BackgroundColor = new BaseColor(25, 121, 255, 255),
                    PaddingTop = 5,
                    PaddingRight = 3,
                    PaddingBottom = 5,
                    PaddingLeft = 2,
                    HorizontalAlignment = 1

                };
                table.AddCell(cell);
                var textWidth = font.GetCalculatedBaseFont(true).GetWidth(hd);
                columnMaxWidths[index] = columnMaxWidths[index] > textWidth ? columnMaxWidths[index] : textWidth;
                index++;
            }

            int rowNumber = 1;
            foreach (var row in items)
            {
                var bgColor = rowNumber % 2 == 0
                    ? new BaseColor(Color.FromArgb(255, 214, 215, 216))
                    : new BaseColor(Color.White);

                var index2 = 0;
                foreach (var cell in row)
                {
                    var font = new Font(baseFont, 6f, Font.NORMAL, new BaseColor(Color.Black));
                    var newCell = new PdfPCell(new Phrase(cell, font))
                    {
                        BackgroundColor = bgColor,
                        Padding = 2,
                        HorizontalAlignment = 1,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    };
                    table.AddCell(newCell);
                    var textWidth = string.IsNullOrEmpty(cell) ? 6 : font.GetCalculatedBaseFont(true).GetWidth(cell);
                    columnMaxWidths[index2] = columnMaxWidths[index2] > textWidth ? columnMaxWidths[index2] : textWidth;
                    index2++;
                }
                rowNumber++;
            }

            using (var memStream = new MemoryStream())
            {
                var doc = new Document(PageSize.A4);
                doc.AddHeader("default", "Default header");
                doc.AddCreator("mdkh.ir");
                doc.AddCreationDate();
                doc.AddTitle("Default Title");
                doc.AddAuthor("mdkh");
                PdfWriter.GetInstance(doc, memStream);
                doc.SetPageSize(PageSize.A4.Rotate());
                var tableWidth = doc.PageSize.Height - (doc.RightMargin + doc.LeftMargin);
                var total = columnMaxWidths.Sum();
                var relativeWidths = new float[columnMaxWidths.Count()];
                for (int i = 0; i < relativeWidths.Length; i++)
                {
                    var widthShare = (columnMaxWidths[i]) / (total * 1.0f);
                    //because of rtl direction
                    relativeWidths[relativeWidths.Length - 1 - i] = Convert.ToSingle((widthShare > .35f ? .3f : widthShare) * tableWidth);
                }
                table.SetWidths(relativeWidths);

                doc.Open();
                doc.Add(table);
                var ms2 = new MemoryStream();
                memStream.CopyTo(ms2);
                doc.Close();
                var fileContent = memStream.ToArray();
                var pgNumber = new PageNumber()
                {
                    TextTemplate = "{{current}}/{{total}}",
                    PrintLocation = PageSection.Bottom,
                    Alignment = Alignment.Center,
                    FontOption = new FontOption(baseFont),
                    Direction = Direction.LeftToRight
                };
                AddPageNumbers(fileContent, ms2, pgNumber);

                var content = ms2.ToArray();
               _path = Path.Combine(filePath , (fileName.ToLowerInvariant().EndsWith(".pdf") ? fileName : fileName + ".pdf"));
                using (var fs = File.Create(_path))
                {
                    fs.Write(content, 0, (int)content.Length);
                }
            }

            return _path;
        }

        protected static void AddPageNumbers(byte[] fileContent, MemoryStream memoryStream, PageNumber pagingOptions)
        {
            var reader = new PdfReader(fileContent);
            var totalPages = reader.NumberOfPages;
            using (var stamper = new PdfStamper(reader, memoryStream))
            {
                var n = reader.NumberOfPages;
                for (var i = 1; i <= n; i++)
                {
                    ApplyLocation(stamper, pagingOptions, i, totalPages);
                }
            }
        }

        static void PrintPageNumber(PdfStamper stamper, PageNumber pagingOptions, int currrent, int total)
        {
            float left = 0f, bottom = 0f;

            var font = pagingOptions.FontOption.Font;
            var text = pagingOptions.GetText(currrent, total);
            var textPhrase = new Phrase(text, font);
            var textWidth = font.GetCalculatedBaseFont(true).GetWidthPoint(text, font.Size);
            var pageBox = stamper.Reader.GetPageSize(currrent);

            if (pagingOptions.Alignment == Alignment.Center && (pagingOptions.PrintLocation == PageSection.Bottom || pagingOptions.PrintLocation == PageSection.Top))
                left = (pageBox.Height + textWidth) / 2;
            if (pagingOptions.Alignment == Alignment.Right && (pagingOptions.PrintLocation == PageSection.Bottom || pagingOptions.PrintLocation == PageSection.Top))
                left = pageBox.Height - textWidth - 15;
            if (pagingOptions.Alignment == Alignment.Left && (pagingOptions.PrintLocation == PageSection.Bottom || pagingOptions.PrintLocation == PageSection.Top))
                left = pageBox.Width - 15;

            if (pagingOptions.Alignment == Alignment.Center && (pagingOptions.PrintLocation == PageSection.Right || pagingOptions.PrintLocation == PageSection.Left))
                bottom = (pageBox.Width + textWidth) / 2;
            if (pagingOptions.Alignment == Alignment.Right && (pagingOptions.PrintLocation == PageSection.Right || pagingOptions.PrintLocation == PageSection.Left))
                bottom = textWidth + 15f;
            if (pagingOptions.Alignment == Alignment.Left && (pagingOptions.PrintLocation == PageSection.Right || pagingOptions.PrintLocation == PageSection.Left))
                bottom = pageBox.Width - textWidth - 15;

            if (pagingOptions.PrintLocation == PageSection.Bottom)
                bottom = 15f;
            if (pagingOptions.PrintLocation == PageSection.Top)
                bottom = pageBox.Width - 15;
            if (pagingOptions.PrintLocation == PageSection.Left)
                left = 15f;
            if (pagingOptions.PrintLocation == PageSection.Right)
                left = pageBox.Height - 15;

            ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_CENTER, textPhrase, bottom, left, 0,
                pagingOptions.Direction == Direction.RightToLeft
                    ? PdfWriter.RUN_DIRECTION_RTL
                    : PdfWriter.RUN_DIRECTION_DEFAULT, 1);

        }
        static void ApplyLocation(PdfStamper stamper, PageNumber pagingOptions, int currrent, int total)
        {
            var font = pagingOptions.FontOption.Font;
            var text = pagingOptions.GetText(currrent, total);
            var textPhrase = new Phrase(text, font);
            var textWidth = font.GetCalculatedBaseFont(true).GetWidthPoint(text, font.Size);
            var pageBox = stamper.Reader.GetPageSize(currrent);
            switch (pagingOptions.PrintLocation)
            {
                case PageSection.Bottom:
                    CreateOnBottom(stamper, pagingOptions, currrent, pageBox, textWidth, textPhrase);
                    break;

                case PageSection.Top:
                    CreateOnTop(stamper, pagingOptions, currrent, pageBox, textWidth, textPhrase);
                    break;

                case PageSection.Left:
                    CreateOnLeft(stamper, pagingOptions, currrent, pageBox, textWidth, textPhrase);
                    break;

                case PageSection.Right:
                    CreateOnRight(stamper, pagingOptions, currrent, pageBox, textWidth, textPhrase);
                    break;
            }
        }

        private static void CreateOnRight(PdfStamper stamper, PageNumber pagingOptions, int currrent, Rectangle pageBox,
            float textWidth, Phrase textPhrase)
        {
            if (pagingOptions.Alignment == Alignment.Center)
            {
                var top = (pageBox.Width + textWidth) / 2;
                var location = pageBox.Height - 15;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_CENTER, textPhrase, location, top,
                    -90,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
            else if (pagingOptions.Alignment == Alignment.Right)
            {
                var top = textWidth + 15f;
                var location = pageBox.Height - 15;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_JUSTIFIED_ALL, textPhrase, location,
                    top, -90,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
            else if (pagingOptions.Alignment == Alignment.Left)
            {
                var top = pageBox.Width - textWidth - 15;
                var location = pageBox.Height - 15;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_LEFT, textPhrase,
                    location, top, - 90,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
        }

        private static void CreateOnLeft(PdfStamper stamper, PageNumber pagingOptions, int currrent, Rectangle pageBox,
            float textWidth, Phrase textPhrase)
        {
            if (pagingOptions.Alignment == Alignment.Center)
            {
                var top = (pageBox.Width + textWidth) / 2;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_CENTER, textPhrase, 15, top, 90,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
            else if (pagingOptions.Alignment == Alignment.Right)
            {
                var top = pageBox.Width - textWidth;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_RIGHT, textPhrase, 15, top, 90,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
            else if (pagingOptions.Alignment == Alignment.Left)
            {
                var top = 15f;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_LEFT, textPhrase, 15, top, 90,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
        }

        private static void CreateOnTop(PdfStamper stamper, PageNumber pagingOptions, int currrent, Rectangle pageBox,
            float textWidth, Phrase textPhrase)
        {
            if (pagingOptions.Alignment == Alignment.Center)
            {
                var location = (pageBox.Height + textWidth) / 2;
                var top = pageBox.Width - 15;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_CENTER, textPhrase, location, top,
                    0,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
            else if (pagingOptions.Alignment == Alignment.Right)
            {
                var location = pageBox.Height - textWidth - 15;
                var top = pageBox.Width - 15;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_RIGHT, textPhrase, location, top, 0,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
            else if (pagingOptions.Alignment == Alignment.Left)
            {
                var location = textWidth + 15;
                var top = pageBox.Width - 15;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_LEFT, textPhrase, location, top, 0,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
        }

        private static void CreateOnBottom(PdfStamper stamper, PageNumber pagingOptions, int currrent, Rectangle pageBox,
            float textWidth, Phrase textPhrase)
        {
            if (pagingOptions.Alignment == Alignment.Center)
            {
                var location = (pageBox.Height + textWidth) / 2;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_CENTER, textPhrase, location, 10f,
                    0,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
            else if (pagingOptions.Alignment == Alignment.Right)
            {
                var location = pageBox.Height - textWidth - 15;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_RIGHT, textPhrase, location, 10f, 0,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
            else if (pagingOptions.Alignment == Alignment.Left)
            {
                var location = textWidth + 15;
                ColumnText.ShowTextAligned(stamper.GetUnderContent(currrent), Element.ALIGN_LEFT, textPhrase, location, 10f, 0,
                    pagingOptions.Direction == Direction.RightToLeft
                        ? PdfWriter.RUN_DIRECTION_RTL
                        : PdfWriter.RUN_DIRECTION_DEFAULT, 1);
            }
        }
    }
}