using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs
{
    public class UserDTO
    {
        [Required]
        public int EmployeeNumber { get; set; }

        [Required]
        public required string Name { get; set; }

        [Range(18, 70)]
        public int? Age { get; set; }

        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public UserRole userRole { get; set; } = UserRole.Normal;

        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = [];

        public double AttendenceRate { get; set; } = 1.0;

        public string? GroupName { get; set; }
        public int? GroupId { get; set; }
        public UserStatusType Statuses { get; set; }
        public ICollection<Request> Requests { get; set; } = [];
    }
}
