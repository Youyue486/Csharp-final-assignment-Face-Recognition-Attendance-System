using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Core
{
    public class Models
    {
        public class User
        {
            public int Id { get; init; }

            [Required]
            public int EmployeeNumber { get; init; }
            
            [Required]
            public required string Name { get; set; }

            [Range(18, 70)]
            public int? Age { get; set; }

            public string? Email { get; set; }

            [Phone]
            public string? PhoneNumber { get; set; }

            [Required]
            public required byte[] Password { get; set; }

            public bool IsAdmin { get; set; } = false;

            public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = [];

            //导航属性
            public Group? Group { get; set; }
            public int? GroupId { get; set; }
            public ICollection<UserStatus> Statuses { get; set; } = [];
            public ICollection<Request> Requests { get; set; } = [];
        }
        public class Group
        {
            public int Id { get; set; }

            [Required, MaxLength(50)]
            public required string GroupName { get; set; }

            //导航属性
            public ICollection<User> Users { get; set; } = [];
            public GroupSchedule? Schedule { get; set; }
            public ICollection<HolidayAdjustment> HolidayAdjustments { get; set; } = [];
        }
        public class GroupSchedule
        {
            public int Id { get; set; }

            [Required]
            public TimeSpan WorkStartTime { get; set; }//上班打卡时间

            [Required]
            public TimeSpan WorkEndTime { get; set; }//下班打卡时间

            [Required]
            public int CheckInWindow { get; set; }//上班打卡时间窗(分钟)

            [Required]
            public WorkDay WorkDays { get; set; } //工作日（按位存储）

            //导航属性
            public int? GroupId { get; set; }
            public Group? Group { get; set; }
        }
        public class HolidayAdjustment
        {
            public int Id { get; set; }

            [Required]
            public required string AdjustmentName { get; set; }

            [Required]
            public DateTime StartDate { get; set; }

            [Required]
            public DateTime EndDate { get; set; }

            public TimeSpan? WorkStartTime { get; set; } // null表示当天放假
            public TimeSpan? WorkEndTime { get; set; }
            public int? CheckInWindowStart { get; set; }
            public int? CheckInWindowEnd { get; set; }

            //导航属性
            public int GroupId { get; set; }
            public Group? Group { get; set; }
        }
        public class AttendanceRecord
        {
            public int Id { get; set; }

            [Required]
            public DateTime CheckTime { get; set; }//精确到秒

            [Required]
            public CheckType CheckType { get; set; }

            //导航属性
            [Required]
            public required User User { get; set; }

            [Required]
            public required int UserId { get; set; }
        }
        public class UserStatus
        {
            public int Id { get; set; }

            [Required]
            public DateTime StartTime { get; set; }

            public DateTime? EndTime { get; set; }//null表示状态持续中

            [Required]
            public required UserStatusType StatusType { get; set; } // Enum type

            //导航属性
            [Required]
            public required User User { get; set; }

            [Required]
            public int UserId { get; set; }
        }
        public class Request
        {
            public int Id{ get; set; }

            [MaxLength(1000)]
            public string? Content { get; set; }

            [Required]
            public DateTime StartDate { get; set; }

            [Required]
            public DateTime EndDate { get; set; }

            [Required]
            public RequestType RequestType { get; set; } // Enum type

            [Required]
            public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending; // Enum type

            public DateTime SubmitTime { get; set; }

            public DateTime? ProcessTime { get; set; } // null表示未处理

            //导航属性
            [Required]
            public required User User { get; set; }
            public int UserId { get; set; }
        }
    }
}
