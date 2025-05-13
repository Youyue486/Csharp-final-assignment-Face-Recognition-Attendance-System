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
    public class GroupDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<UserDTO> Members { get; set; } = [];
        public int MemberCount { get; set; } = 0;
        public WorkDay? WorkDays { get; set; }
        public TimeSpan? WorkStartTime { get; set; }
        public TimeSpan? WorkEndTime { get; set; }
        //TODO public ICollection<HolidayAdjustment> HolidayAdjustments { get; set; } = [];
    }
}
