using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    interface IUserService
    {
        public string Authenticate(string username, string password);
        public List<AttendanceRecord> GetAttendanceRecords(DateTime from, DateTime to);
        public string ExportAttendanceToExcel(List<AttendanceRecord> records);
        public bool SubmitLeaveRequest(string reason);
        public bool SubmitTravelRequest(string reason);
    }
}
