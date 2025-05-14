using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class AddGroupViewModel : ObservableObject
    {
        private IAdminService adminService;

        [Required]
        [MaxLength(50)]
        public string GroupName { get; set; } = string.Empty;
        [ObservableProperty]
        public WorkDay selectedWorkDays = WorkDay.周一 | WorkDay.周二 | WorkDay.周三 | WorkDay.周四 | WorkDay.周五; //工作日（按位存储）
        [ObservableProperty]
        public TimeSpan selectedWorkStartTime = new TimeSpan(9, 0, 0);//上班打卡时间
        [ObservableProperty]
        public TimeSpan selectedWorkEndTime = new TimeSpan(18, 0, 0);//下班打卡时间
        [ObservableProperty]
        public string groupDescription = string.Empty;
        [ObservableProperty]
        public List<TimeSpan> timeOptions = new();


        public AddGroupViewModel(IAdminService adminService)
        {
            this.adminService = adminService;
            for (int hour = 0; hour < 24; hour++)
            {
                TimeOptions.Add(new TimeSpan(hour, 0, 0));   // 00:00, 01:00, ..., 23:00
                TimeOptions.Add(new TimeSpan(hour, 30, 0));  // 00:30, 01:30, ..., 23:30
            }
        }

        [RelayCommand]
        public void Confirm()
        {
            if (string.IsNullOrWhiteSpace(GroupName))
            {
                throw new ValidationException("组名不能为空");
            }
            // Validate the work start and end times
            if (SelectedWorkStartTime >= SelectedWorkEndTime)
            {
                throw new ValidationException("上班时间需早于下班时间");
            }
            adminService.AddGroup(GroupName, SelectedWorkDays, SelectedWorkStartTime, SelectedWorkEndTime,  GroupDescription);
            CloseDialog?.Invoke();
        }

        [RelayCommand]
        public void Cancel()
        {
            // Logic to close the dialog or perform any other cancellation actions
            CloseDialog?.Invoke();
        }

        public event Action? CloseDialog;
    }
}
