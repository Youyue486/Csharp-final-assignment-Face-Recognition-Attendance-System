using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class EditGroupViewModel : ObservableObject
    {
        private readonly IAdminService adminService;

        //小组属性
        int groupId = 0;
        [ObservableProperty]
        public string groupName= string.Empty;
        [ObservableProperty]
        public WorkDay selectedWorkDays; //工作日（按位存储）
        [ObservableProperty]
        public TimeSpan selectedWorkStartTime = new TimeSpan(9, 0, 0);//上班打卡时间
        [ObservableProperty]
        public TimeSpan selectedWorkEndTime = new TimeSpan(18, 0, 0);//下班打卡时间
        [ObservableProperty]
        public string groupDescription = string.Empty;
        [ObservableProperty]
        public List<TimeSpan> timeOptions = new();
        // 现有的属性和构造函数保持不变...

        public bool IsMondayChecked
        {
            get => (SelectedWorkDays & WorkDay.周一) != 0;
            set => SetWorkDay(value, WorkDay.周一);
        }

        public bool IsTuesdayChecked
        {
            get => (SelectedWorkDays & WorkDay.周二) != 0;
            set => SetWorkDay(value, WorkDay.周二);
        }

        public bool IsWednesdayChecked
        {
            get => (SelectedWorkDays & WorkDay.周三) != 0;
            set => SetWorkDay(value, WorkDay.周三);
        }

        public bool IsThursdayChecked
        {
            get => (SelectedWorkDays & WorkDay.周四) != 0;
            set => SetWorkDay(value, WorkDay.周四);
        }

        public bool IsFridayChecked
        {
            get => (SelectedWorkDays & WorkDay.周五) != 0;
            set => SetWorkDay(value, WorkDay.周五);
        }

        public bool IsSaturdayChecked
        {
            get => (SelectedWorkDays & WorkDay.周六) != 0;
            set => SetWorkDay(value, WorkDay.周六);
        }

        public bool IsSundayChecked
        {
            get => (SelectedWorkDays & WorkDay.周日) != 0;
            set => SetWorkDay(value, WorkDay.周日);
        }

        private void SetWorkDay(bool isChecked, WorkDay day)
        {
            if (isChecked)
                SelectedWorkDays |= day;
            else
                SelectedWorkDays &= ~day;

            // 通知所有相关的属性已更改
            OnPropertyChanged(nameof(IsMondayChecked));
            OnPropertyChanged(nameof(IsTuesdayChecked));
            OnPropertyChanged(nameof(IsWednesdayChecked));
            OnPropertyChanged(nameof(IsThursdayChecked));
            OnPropertyChanged(nameof(IsFridayChecked));
            OnPropertyChanged(nameof(IsSaturdayChecked));
            OnPropertyChanged(nameof(IsSundayChecked));
        }

        public EditGroupViewModel(IAdminService adminService, GroupDTO groupDTO)
        {
            this.adminService = adminService;
            groupId = groupDTO.Id;
            groupName = groupDTO.Name;
            selectedWorkDays = groupDTO.WorkDays?? 0;
            selectedWorkStartTime = groupDTO.WorkStartTime ?? new TimeSpan(9, 0, 0);
            selectedWorkEndTime = groupDTO.WorkEndTime ?? new TimeSpan(18, 0, 0);
            groupDescription = groupDTO.Description ?? string.Empty;
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
            adminService.UpdateGroup(groupId, GroupName, SelectedWorkDays, SelectedWorkStartTime, SelectedWorkEndTime, GroupDescription);
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
