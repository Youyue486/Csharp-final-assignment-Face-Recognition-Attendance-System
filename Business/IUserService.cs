using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    public interface IUserService
    {
        // 用户认证（通过工号+密码）
        User? Authenticate(int employeeNumber, string password);

        // 获取用户完整考勤记录（包含导航属性）
        List<AttendanceRecord> GetUserAttendanceRecords(int userId, DateTime start, DateTime end);

        // 导出当前用户考勤记录
        string ExportUserAttendance(int userId, DateTime start, DateTime end);

        // 申请提交（统一入口）
        bool SubmitRequest(int userId, RequestType type, string content, DateTime startDate, DateTime endDate);
    }
}
