using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Data;
using Notebook.Server.Dto;
using Notebook.Server.Enum;
using OfficeOpenXml;

namespace Notebook.Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExcelGeneratorService generatorService;

        public AdminService(IMapper mapper, ApplicationDbContext dbContext, IExcelGeneratorService generatorService)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.generatorService = generatorService;
        }

        public async Task DeleteNoteAsync(int id)
        {
            var existingNote = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);
            if (existingNote != null)
            {
                dbContext.Remove(existingNote);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(string email)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (existingUser != null)
            {
                dbContext.Users.Remove(existingUser);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var existingUsers = await dbContext.Users.Include(f => f.Roles).Include(f => f.Notes).ToListAsync();
            var userModels = mapper.Map<List<UserModel>>(existingUsers);
            return userModels;
        }

        public async Task<List<NoteChangeLogModel>> GetChangeLogsAsync(string email)
        {
            var noteChangeLogs = await dbContext.NoteChangeLogs.Where(f => f.Email == email).ToListAsync();
            var noteChangeLogsModel = mapper.Map<List<NoteChangeLogModel>>(noteChangeLogs);
            return noteChangeLogsModel;
        }

        public async Task<NoteModel> GetCurrenNoteAsync(int id)
        {
            var existingNote = await dbContext.Notebooks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingNote != null)
            {
                var noteModel = mapper.Map<NoteModel>(existingNote);
                return noteModel;
            }
            return null;
        }

        public async Task<UserModel> GetCurrentUserAsync(string email)
        {
            var existingUser = await dbContext.Users.Include(f => f.Roles).Include(f => f.Notes).FirstOrDefaultAsync(f => f.Email == email);
            var userModel = mapper.Map<UserModel>(existingUser);
            return userModel;
        }

        public async Task<byte[]> GetExcelFileLogsAsync(LogFileByPeriodRequest request)
        {
            var noteChangeLog = await dbContext.NoteChangeLogs
                .Where(f => f.Email == request.Email)
                .Where(f => f.ChangedAt >= request.StartDate && f.ChangedAt <= request.EndDate)
                .OrderBy(f => f.ChangedAt)
                .AsNoTracking()
                .ToListAsync();

            if (noteChangeLog.Count == 0)
            {
                return null;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // выделить в отдельный сервис

            // генерация pdf
            // легенда --> резюме --> подготовка к собеседованиям

            return generatorService.GenerateExcel(noteChangeLog, request.Email);

            //using (var package = new ExcelPackage())
            //{

            //    var ws = package.Workbook.Worksheets.Add("Logs");

            //    ws.Cells.Style.Font.Name = "Times New Roman";
            //    ws.Cells.Style.Font.Size = 12;
            //    ws.Columns.Width = 30;

            //    //имя таблицы
            //    ws.Cells[1, 1].Value = $"Отчет по изменениям {request.Email}";

            //    ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //    ws.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //    ws.Cells[1, 1].Style.Font.Size = 20;
            //    ws.Cells[1, 1].Style.Font.Bold = true;
            //    ws.Cells["A1:F1"].Merge = true;

            //    //header таблицы
            //    ws.Cells[2, 1].Value = "№";
            //    ws.Cells[2, 2].Value = "Дата изменения";
            //    ws.Cells[2, 3].Value = "Автор изменения";
            //    ws.Cells[2, 4].Value = "Наименование изменяемого поля";
            //    ws.Cells[2, 4].Style.WrapText = true;

            //    ws.Cells[2, 5].Value = "Было";
            //    ws.Cells[2, 6].Value = "Cтало";
            //    ws.Cells[2,1,2,6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //    ws.Cells[2,1,2,6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //    ws.Cells[2, 1, 2, 6].Style.Font.Italic = true;

            //    var emailPattern = @"Пользователь\s(\S+)\s";
            //    var fieldPattern = @"(\d+)\.\sПоле\s(\S+)\sбыло\sзначение\s(\S+),\sстало\s(\S+)";

            //    var step = 0;

            //    for (int i = 0; i < noteChangeLog.Count; i++)
            //    {
            //        var emailMatch = Regex.Match(noteChangeLog[i].Log, emailPattern);
            //        var email = emailMatch.Groups[1].Value;
            //        var fieldMatches = Regex.Matches(noteChangeLog[i].Log, fieldPattern);

            //        ws.Cells[3 + step, 1].Value = i + 1;
            //        ws.Cells[3 + step, 2].Value = $"Изменение от {noteChangeLog[i].ChangedAt}";
            //        ws.Cells[3 + step, 2].Style.WrapText = true;


            //        ws.Cells[3 + step, 3].Value = $"{noteChangeLog[i].Email}";

            //        for (int j = 0; j < fieldMatches.Count; j++)
            //        {
            //            ws.Cells[j + 3 + step, 4].Value = fieldMatches[j].Groups[2].Value;

            //            ws.Cells[j + 3 + step, 5].Value = fieldMatches[j].Groups[3].Value;
            //            ws.Cells[j + 3 + step, 5].Style.Font.Color.SetColor(Color.Red);
            //            ws.Cells[j + 3 + step, 6].Value = fieldMatches[j].Groups[4].Value;
            //            ws.Cells[j + 3 + step, 6].Style.Font.Color.SetColor(Color.FromArgb(68, 114, 196));
            //        }

            //        ws.Cells[$"A{3 + step}:A{3 + step + fieldMatches.Count - 1}"].Merge = true;
            //        ws.Cells[$"B{3 + step}:B{3 + step + fieldMatches.Count - 1}"].Merge = true;
            //        ws.Cells[$"C{3 + step}:C{3 + step + fieldMatches.Count - 1}"].Merge = true;

            //        ws.Cells[$"A{3 + step}:C{3 + step + fieldMatches.Count - 1}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //        ws.Cells[$"A{3 + step}:C{3 + step + fieldMatches.Count - 1}"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            //        step += fieldMatches.Count;

            //    }

            //    var filledCells = ws.Cells[ws.Dimension.Address];
            //    filledCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //    filledCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //    filledCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //    filledCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            //    return package.GetAsByteArray();
            //}
        }

        public async Task<List<RoleModel>> GetRolesAsync()
        {
            var existingRoles = await dbContext.Roles.ToListAsync();
            var result = mapper.Map<List<RoleModel>>(existingRoles);
            return result;
        }

        public async Task<bool> UpdateNoteAsync(int id, NoteRequest request)
        {
            var existingNote = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);
            if (existingNote != null)
            {
                existingNote.FirstName = request.FirstName;
                existingNote.MiddleName = request.FirstName;
                existingNote.LastName = request.LastName;
                existingNote.PhoneNumber = request.PhoneNumber;
                existingNote.Country = request.Country;
                existingNote.BirthDay = request.BirthDay;
                existingNote.Organization = request.Organization;
                existingNote.Position = request.Position;
                existingNote.Other = request.Other;

                dbContext.Update(existingNote);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<UserModel> UpdateUserAsync(UserModelRequest user)
        {
            var existingUser = await dbContext.Users.Include(f => f.Roles).Include(f => f.Notes).FirstOrDefaultAsync(f => f.Email == user.Email);

            var roles = user.RoleModels.Select(f => (RoleName)System.Enum.Parse(typeof(RoleName), f.RoleName));
            var rolesFromDb = await dbContext.Roles.Where(f => roles.Any(g => g == f.RoleName)).ToListAsync();

            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Roles = rolesFromDb;

                dbContext.Update(existingUser);

                await dbContext.SaveChangesAsync();

                var result = mapper.Map<UserModel>(existingUser);
                return result;
            }
            return null;
        }
    }
}
