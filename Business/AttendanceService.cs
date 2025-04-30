using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    public class AttendanceService : IAttendanceService
    {
        private readonly AppDbContext _context;

        public AttendanceService(AppDbContext context)
        {
            _context = context;
        }
        public void ClockIn(int userId)
        {
            // 获取用户信息
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("用户不存在");
            }

            // 检查用户是否有分组
            if (user.Group == null || user.Group.Schedule == null)
            {
                throw new Exception("用户未分配分组或分组未设置考勤时间表");
            }

            // 获取当前时间
            var now = DateTime.Now;

            // 检查是否在考勤时间内
            var schedule = user.Group.Schedule;
            if (now.TimeOfDay < schedule.WorkStartTime || now.TimeOfDay > schedule.WorkEndTime)
            {
                throw new Exception("不在考勤时间内");
            }

            // 检查是否在考勤窗口内
            if (now.Minute < schedule.CheckInWindow)
            {
                throw new Exception("不在考勤窗口内");
            }

            // 创建考勤记录
            var attendanceRecord = new AttendanceRecord
            {
                UserId = userId,
                User = user, // 修复：设置所需的成员 'User'
                CheckTime = now,
                CheckType = CheckType.CheckIn // 假设 CheckType 是 CheckIn
            };
            _context.AttendanceRecords.Add(attendanceRecord);
            _context.SaveChanges();
        }
    }
}
