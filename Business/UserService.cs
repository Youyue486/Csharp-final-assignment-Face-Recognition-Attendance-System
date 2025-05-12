using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;  //装epplus
using OfficeOpenXml.Table;
using OfficeOpenXml.Style;
using OfficeOpenXml.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private AppDbContext _context;

        public UserService(IUserRepository userRepository, AppDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
            //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        public User? Authenticate(int employeeNumber, string password)
        {
            var user = _context.Users
                .Include(u => u.Group)
                .FirstOrDefault(u => u.EmployeeNumber == employeeNumber);

            return user != null && VerifyPassword(password, user.Password)
                ? user
                : null;
        }

        public List<AttendanceRecord> GetUserAttendanceRecords(int userId, DateTime start, DateTime end)
        {
            return _context.AttendanceRecords
                .Include(r => r.User)
                .Where(r => r.UserId == userId &&
                           r.CheckTime >= start &&
                           r.CheckTime <= end)
                .OrderBy(r => r.CheckTime)
                .ToList();
        }

        public string ExportUserAttendance(int userId, DateTime start, DateTime end)
        {
            var records = GetUserAttendanceRecords(userId, start, end);

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("考勤记录");

            // 表头
            sheet.Cells[1, 1].Value = "工号";
            sheet.Cells[1, 2].Value = "姓名";
            sheet.Cells[1, 3].Value = "打卡时间";
            sheet.Cells[1, 4].Value = "打卡类型";

            // 数据填充
            for (int i = 0; i < records.Count; i++)
            {
                var row = i + 2;
                var record = records[i];
                sheet.Cells[row, 1].Value = record.User.EmployeeNumber;
                sheet.Cells[row, 2].Value = record.User.Name;
                sheet.Cells[row, 3].Value = record.CheckTime.ToString("yyyy-MM-dd HH:mm:ss");
                sheet.Cells[row, 4].Value = record.CheckType.ToString();
            }

            var fileName = $"Attendance_{userId}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            var path = Path.Combine(Path.GetTempPath(), fileName);
            package.SaveAs(new FileInfo(path));
            return path;
        }

        public bool SubmitRequest(int userId, RequestType type, string content, DateTime startDate, DateTime endDate)
        {
            var request = new Request
            {
                UserId = userId,
                User =  _userRepository.GetById(userId), 
                RequestType = type,
                Content = content,
                SubmitTime = DateTime.Now,
                StartDate = startDate,
                EndDate = endDate,
                RequestStatus = RequestStatus.Pending
            };

            _context.Requests.Add(request);
            return _context.SaveChanges() > 0;
        }

        private static bool VerifyPassword(string inputPassword, byte[] storedHash)
        {
            using var sha256 = SHA256.Create();
            var inputHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
            return inputHash.SequenceEqual(storedHash);
        }
    }
}
