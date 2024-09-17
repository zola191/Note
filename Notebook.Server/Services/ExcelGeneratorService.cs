using Notebook.Server.Domain;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Notebook.Server.Services
{
    public class ExcelGeneratorService : IExcelGeneratorService
    {
        public byte[] GenerateExcel(List<NoteChangeLog> noteChangeLogs, string email)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {

                var ws = package.Workbook.Worksheets.Add("Logs");

                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 12;
                ws.Columns.Width = 30;

                //имя таблицы
                ws.Cells[1, 1].Value = $"Отчет по изменениям {email}";

                ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[1, 1].Style.Font.Size = 20;
                ws.Cells[1, 1].Style.Font.Bold = true;
                ws.Cells["A1:F1"].Merge = true;

                //header таблицы
                ws.Cells[2, 1].Value = "№";
                ws.Cells[2, 2].Value = "Дата изменения";
                ws.Cells[2, 3].Value = "Автор изменения";
                ws.Cells[2, 4].Value = "Наименование изменяемого поля";
                ws.Cells[2, 4].Style.WrapText = true;

                ws.Cells[2, 5].Value = "Было";
                ws.Cells[2, 6].Value = "Cтало";
                ws.Cells[2, 1, 2, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[2, 1, 2, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[2, 1, 2, 6].Style.Font.Italic = true;

                var emailPattern = @"Пользователь\s(\S+)\s";
                var fieldPattern = @"(\d+)\.\sПоле\s(\S+)\sбыло\sзначение\s(\S+),\sстало\s(\S+)";

                var step = 0;

                for (int i = 0; i < noteChangeLogs.Count; i++)
                {
                    var emailMatch = Regex.Match(noteChangeLogs[i].Log, emailPattern);
                    var mailOfTheAuthorOfTheChanges = emailMatch.Groups[1].Value;
                    var fieldMatches = Regex.Matches(noteChangeLogs[i].Log, fieldPattern);

                    ws.Cells[3 + step, 1].Value = i + 1;
                    ws.Cells[3 + step, 2].Value = $"Изменение от {noteChangeLogs[i].ChangedAt}";
                    ws.Cells[3 + step, 2].Style.WrapText = true;


                    ws.Cells[3 + step, 3].Value = $"{noteChangeLogs[i].Email}";

                    for (int j = 0; j < fieldMatches.Count; j++)
                    {
                        ws.Cells[j + 3 + step, 4].Value = fieldMatches[j].Groups[2].Value;

                        ws.Cells[j + 3 + step, 5].Value = fieldMatches[j].Groups[3].Value;
                        ws.Cells[j + 3 + step, 5].Style.Font.Color.SetColor(Color.Red);
                        ws.Cells[j + 3 + step, 6].Value = fieldMatches[j].Groups[4].Value;
                        ws.Cells[j + 3 + step, 6].Style.Font.Color.SetColor(Color.FromArgb(68, 114, 196));
                    }

                    ws.Cells[$"A{3 + step}:A{3 + step + fieldMatches.Count - 1}"].Merge = true;
                    ws.Cells[$"B{3 + step}:B{3 + step + fieldMatches.Count - 1}"].Merge = true;
                    ws.Cells[$"C{3 + step}:C{3 + step + fieldMatches.Count - 1}"].Merge = true;

                    ws.Cells[$"A{3 + step}:C{3 + step + fieldMatches.Count - 1}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[$"A{3 + step}:C{3 + step + fieldMatches.Count - 1}"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    step += fieldMatches.Count;

                }

                var filledCells = ws.Cells[ws.Dimension.Address];
                filledCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                filledCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                filledCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                filledCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                return package.GetAsByteArray();
            }
        }
    }
}
