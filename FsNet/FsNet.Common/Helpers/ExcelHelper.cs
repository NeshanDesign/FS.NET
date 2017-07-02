using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FsNet.Common.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FsNet.Common.Helpers
{
    //Ugly helper :(
    public class ExcelHelper
    {
        public static Stream WriteToStream(ArrayList rows, string fileName)
        {
            var stream = new MemoryStream();

            try
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Report");
                    if (rows.Count > 0)
                    {
                        var maxWidth = new List<float> { 0.0f };
                        for (var rowIdx = 1; rowIdx <= rows.Count + 1; rowIdx++)
                        {
                            var columnCount = ((string[])rows[0]).ToArray().Count() + 1;
                            var curRow = ((string[])rows[0]).ToArray();
                            if (rowIdx == 1)
                            {
                                for (var colIdx = 0; colIdx < columnCount; colIdx++)
                                {
                                    var range = worksheet.Cells[rowIdx, colIdx];
                                    range.Value = curRow[colIdx];

                                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(248, 0, 85, 170));
                                    range.Style.Font.Color.SetColor(Color.White);

                                    range.Style.Font.Name = "Tahoma";
                                    range.Style.Font.Color.SetColor(Color.White);
                                    range.Style.Font.Size = 9;

                                    var width = range.Style.Font.Size * range.Value.ToString().Length * .109f;
                                    if (colIdx >= maxWidth.Count) maxWidth.Add(.0f);
                                    maxWidth[colIdx] = maxWidth[colIdx] - .000001f > width
                                        ? maxWidth[colIdx]
                                        : width;
                                }
                            }
                            else
                            {
                                for (int colIdx = 0; colIdx < columnCount; colIdx++)
                                {
                                    var val = "";
                                    if (colIdx != columnCount - 1)
                                    {
                                        val = curRow[colIdx];
                                    }
                                    var range = worksheet.Cells[rowIdx, colIdx];

                                    range.Value = val ?? "";
                                    range.Style.Font.Name = "Tahoma";
                                    range.Style.Font.Size = 9;
                                    range.Style.HorizontalAlignment = val != null &&
                                                                      val.Replace("/", "")
                                                                         .Replace(".", "")
                                                                         .Replace(":", "")
                                                                         .All(char.IsDigit)
                                        ? ExcelHorizontalAlignment.Center
                                        : ExcelHorizontalAlignment.Right;

                                    var width = range.Style.Font.Size * range.Value.ToString().Length * .119f;
                                    if (colIdx >= maxWidth.Count) maxWidth.Add(.0f);
                                    maxWidth[colIdx] = maxWidth[colIdx] - .000001f < width && width < 140
                                        ? width
                                        : width < 140 ? maxWidth[colIdx] : 140;

                                    if (rowIdx % 2 == 0 && rowIdx != 0)
                                    {
                                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(245, 200, 200, 200));
                                    }
                                }
                            }
                        }
                        for (var i = 0; i < maxWidth.Count; i++)
                        {
                            if (i == 0)
                            {
                                worksheet.Column(i + 1).Width = 10;
                                worksheet.Column(i + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            }
                            else
                                worksheet.Column(i + 1).Width = maxWidth[i];
                            worksheet.Column(i + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                    }

                    worksheet.View.RightToLeft = true;
                    package.Workbook.Properties.Company = "Mdkh.ir";
                    package.Workbook.Properties.Author = "mdkh";

                    package.SaveAs(stream);

                }
            }
            catch (Exception ex)
            {
               Logger.Error(ex);
            }

            return stream;
        }

        public static string Write(ArrayList rows, string fileName)
        {
            string _fileName = fileName;
            try
            {
                var count = 1;
                var tempFileName = _fileName;
                while (File.Exists(tempFileName))
                {
                    if (_fileName.LastIndexOf(".", System.StringComparison.Ordinal) > -1)
                    {
                        tempFileName =
                            _fileName.Substring(0, _fileName.LastIndexOf(".", System.StringComparison.Ordinal)) +" ("+
                            count++ + ")" +
                            _fileName.Substring(_fileName.LastIndexOf(".", System.StringComparison.Ordinal), _fileName.Length - _fileName.LastIndexOf(".", System.StringComparison.Ordinal));}
                    else
                        _fileName += count++;
                }
                _fileName = tempFileName;
     //           using (var sw = new System.IO.StreamWriter(_fileName))
     //           {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Report");
                        if (rows.Count > 0)
                        {
                            var maxWidth = new List<float> {0.0f};
                            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
                            {
                                var columnCount = ((string[]) rows[rowIdx]).ToArray().Count();
                                var curRow = ((string[]) rows[rowIdx]).ToArray();
                                if (rowIdx == 0)
                                {
                                    for (var colIdx = 0; colIdx < columnCount; colIdx++)
                                    {
                                        var range = worksheet.Cells[rowIdx + 1, colIdx + 1];
                                        try
                                        {
                                            range.Value = curRow[colIdx];
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e);
                                        }

                                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(248, 0, 85, 170));
                                        range.Style.Font.Color.SetColor(Color.White);

                                        range.Style.Font.Name = "Tahoma";
                                        range.Style.Font.Color.SetColor(Color.White);
                                        range.Style.Font.Size = 9;

                                        var width = range.Style.Font.Size*range.Value.ToString().Length*.109f;
                                        if (colIdx >= maxWidth.Count) maxWidth.Add(.0f);
                                        maxWidth[colIdx] = maxWidth[colIdx] - .000001f > width
                                            ? maxWidth[colIdx]
                                            : width;
                                    }
                                }
                                else
                                {
                                    for (int colIdx = 0; colIdx < columnCount; colIdx++)
                                    {
                                        var val = "";
                                        val = curRow[colIdx];
                                        var range = worksheet.Cells[rowIdx + 1, colIdx + 1];

                                        range.Value = val ?? "";
                                        range.Style.Font.Name = "Tahoma";
                                        range.Style.Font.Size = 9;
                                        range.Style.HorizontalAlignment = val != null &&
                                                                          val.Replace("/", "")
                                                                              .Replace(".", "")
                                                                              .Replace(":", "")
                                                                              .All(char.IsDigit)
                                            ? ExcelHorizontalAlignment.Center
                                            : ExcelHorizontalAlignment.Right;

                                        var width = range.Style.Font.Size*range.Value.ToString().Length*.119f;
                                        if (colIdx >= maxWidth.Count) maxWidth.Add(.0f);
                                        maxWidth[colIdx] = maxWidth[colIdx] - .000001f < width && width < 140
                                            ? width
                                            : width < 140 ? maxWidth[colIdx] : 140;

                                        if (rowIdx%2 == 0 && rowIdx != 0)
                                        {
                                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(245, 200, 200, 200));
                                        }
                                    }
                                }
                            }
                            for (var i = 0; i < maxWidth.Count; i++)
                            {
                                if (i == 0)
                                {
                                    worksheet.Column(i + 1).Width = 10;
                                    worksheet.Column(i + 1).Style.HorizontalAlignment =
                                        ExcelHorizontalAlignment.CenterContinuous;
                                }
                                else
                                    worksheet.Column(i + 1).Width = maxWidth[i];
                                worksheet.Column(i + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }
                        }

                        worksheet.View.RightToLeft = true;
                        package.Workbook.Properties.Company = "mdkh.ir";
                        package.Workbook.Properties.Author = "mdkh";

                        var fs = new FileStream(_fileName, FileMode.CreateNew, FileAccess.ReadWrite);
                        package.SaveAs(fs);
                        try
                        {
                            fs.Flush(true);
                            fs.Dispose();
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e);
                        }
                    }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }

            var result = _fileName.Split(new[] { @"/", @"\" }, StringSplitOptions.RemoveEmptyEntries);

            return result[result.Length - 1];
        }

        public static List<string[]> Read(string fileName, int columnCount)
        {
            if (!File.Exists(fileName))
                return null;

            try
            {
                var newFile = new FileInfo(fileName);

                using (var package = new ExcelPackage(newFile))
                {
                    var result = new List<string[]>();

                    var worksheet = package.Workbook.Worksheets[1];

                    var rowIdx = 2;
                    while (true)
                    {
                        var rowExists = worksheet.Cells[rowIdx, 1].Value != null && !string.IsNullOrEmpty(worksheet.Cells[rowIdx, 1].Value.ToString());

                        if (!rowExists)
                            break;
                        var currentRow = new string[columnCount + 1];
                        for (int colIdx = 1; colIdx <= columnCount; colIdx++)
                        {
                            var range = worksheet.Cells[rowIdx, colIdx];
                            currentRow[colIdx] = range.Value != null ? range.Value.ToString().Trim() : "";
                        }

                        currentRow[0] = rowIdx.ToString();
                        result.Add(currentRow);
                        rowIdx++;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}
