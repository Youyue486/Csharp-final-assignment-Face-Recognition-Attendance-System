using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Data
{
    class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        /// 添加考勤记录
        public void AddAttendanceRecord(AttendanceRecord attendance)
        {
            _context.AttendanceRecords.Add(attendance);
            _context.SaveChanges();
        }
        /// 根据用户ID获取考勤记录
        public ICollection<AttendanceRecord> GetAttendanceRecordsByUserId(int Userid)
        {
            return _context.AttendanceRecords
                .Where(a => a.UserId == Userid)
                .ToList();
        }
        /// 根据用户获取考勤记录
        public ICollection<AttendanceRecord> GetAttendanceRecordsByUser(User user)
        {
            return _context.AttendanceRecords
                .Where(a => a.UserId == user.Id)
                .ToList();
        }
        /// 获取所有考勤记录
        public ICollection<AttendanceRecord> GetAllAttendanceRecords()
        {
            return _context.AttendanceRecords
                .ToList();
        }
        /// 删除考勤记录
        void IAttendanceRepository.DeleteAttendanceRecord(AttendanceRecord attendance)
        {
            _context.AttendanceRecords.Remove(attendance);
            _context.SaveChanges();
        }
    }
}
