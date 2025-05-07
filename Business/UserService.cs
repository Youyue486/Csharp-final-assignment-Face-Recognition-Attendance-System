using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    class UserService : IUserService
    {
        private readonly AttendanceDbContext _context;
        public UserService(AttendanceDbContext context) => _context = context;

        public string Authenticate(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.EmployeeNumber.ToString() == username);

            if (user == null || !user.Password.SequenceEqual(Hash(password)))
                return null;

            return user.Name;
        }

        public List<AttendanceRecord> GetAttendanceRecords(DateTime from, DateTime to)
        {
            return _context.AttendanceRecords
                .Include(r => r.User)
                .Where(r => r.CheckTime >= from && r.CheckTime <= to)
                .OrderBy(r => r.CheckTime)
                .Select(r => new AttendanceRecord
                {
                    CheckTime = r.CheckTime,
                    CheckType = r.CheckType,
                    User = new User
                    {
                        EmployeeNumber = r.User.EmployeeNumber,
                        Name = r.User.Name
                    }
                })
                .ToList();
        }


        public string ExportAttendanceToExcel(List<AttendanceRecord> records)
        {
            if (records == null || records.Count == 0)
                throw new ArgumentException("没有可导出的考勤记录");
            // 使用 EPPlus 导出 Excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // 构造文件名和路径
            var fileName = $"Attendance_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            var outputPath = Path.Combine(Path.GetTempPath(), fileName);
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("考勤明细");

                // 更新表头（移除了设备编号列）
                sheet.Cells[1, 1].Value = "工号";
                sheet.Cells[1, 2].Value = "姓名";
                sheet.Cells[1, 3].Value = "打卡时间";
                sheet.Cells[1, 4].Value = "打卡类型";

                for (int i = 0; i < records.Count; i++)
                {
                    var row = i + 2;
                    var record = records[i];

                    sheet.Cells[row, 1].Value = record.User?.EmployeeNumber;
                    sheet.Cells[row, 2].Value = record.User?.Name;
                    sheet.Cells[row, 3].Value = record.CheckTime.ToString("yyyy-MM-dd HH:mm:ss");
                    sheet.Cells[row, 4].Value = GetCheckTypeDisplay(record.CheckType);
                }
                // 自动列宽
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                package.SaveAs(new FileInfo(outputPath));
            }
            return outputPath;
        }

        private string GetCheckTypeDisplay(CheckType type)
        {
            return type switch
            {
                CheckType.CheckIn => "上班打卡",
                CheckType.CheckOut => "下班打卡",
                _ => "未知状态"
            };
        }

        public bool SubmitLeaveRequest(string reason)
        {  //应该是申请，这边直接通过了
           
        }

        public bool SubmitTravelRequest(string reason)
        {
            
        }

        private string Hash(string password) => password; // TODO: 替换为安全哈希
    }
}
