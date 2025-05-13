using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Data
{
    public interface IAttendanceRepository
    {
        /// 添加考勤记录
        void AddAttendanceRecord(AttendanceRecord attendance);
        /// 根据用户ID获取考勤记录
        ICollection<AttendanceRecord> GetAttendanceRecordsByUserId(int Userid);
        /// 根据用户获取考勤记录
        ICollection<AttendanceRecord> GetAttendanceRecordsByUser(User user);
        /// 获取所有考勤记录
        ICollection<AttendanceRecord> GetAllAttendanceRecords();
        /// 删除考勤记录
        void DeleteAttendanceRecord(AttendanceRecord attendance);
    }
}
