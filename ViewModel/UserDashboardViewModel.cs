using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;
using Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public enum Section
    {
        Attendance,
        Application
    }
    public partial class UserDashboardViewModel : ObservableObject
    {
        private Section _currentSection = Section.Attendance;
        public Section CurrentSection
        {
            get => _currentSection;
            set => SetProperty(ref _currentSection, value);
        }

        public ICommand ShowAttendanceCommand { get; }
        public ICommand ShowApplicationCommand { get; }

        private readonly IUserService _userService;
        private readonly int _currentUserId;

        // 数据集合
        public ObservableCollection<AttendanceRecord> AttendanceRecords { get; } = new();

        // 申请表单字段
        [ObservableProperty]
        public DateTime leaveStartDate = DateTime.Today;
        [ObservableProperty]
        public DateTime leaveEndDate = DateTime.Today.AddDays(1);
        [ObservableProperty]
        public string leaveReason = string.Empty;
        [ObservableProperty]
        public DateTime travelStartDate = DateTime.Today;
        [ObservableProperty]
        public DateTime travelEndDate = DateTime.Today.AddDays(1);
        [ObservableProperty]
        public string travelReason = string.Empty;

        // 命令系统
        public RelayCommand LoadAttendanceCommand { get; }
        public RelayCommand ExportExcelCommand { get; }
        public RelayCommand SubmitLeaveCommand { get; }
        public RelayCommand SubmitTravelCommand { get; }

        public UserDashboardViewModel(IUserService userService)
        {
            ShowAttendanceCommand = new RelayCommand(() => CurrentSection = Section.Attendance);
            ShowApplicationCommand = new RelayCommand(() => CurrentSection = Section.Application);
            _userService = userService;
            LoadAttendanceCommand = new RelayCommand(async () => await LoadAttendanceAsync());
            ExportExcelCommand = new RelayCommand(async () => await ExportExcelAsync());
            SubmitLeaveCommand = new RelayCommand(async () => await SubmitLeaveAsync());
            SubmitTravelCommand = new RelayCommand(async () => await SubmitTravelAsync());

            LoadAttendanceCommand.Execute(null);
        }

        private async Task LoadAttendanceAsync()
        {
            var records = await Task.Run(() =>
                _userService.GetUserAttendanceRecords(
                    _currentUserId,
                    DateTime.Today.AddMonths(-1),
                    DateTime.Today));

            AttendanceRecords.Clear();
            foreach (var record in records)
            {
                AttendanceRecords.Add(record);
            }
        }

        private async Task ExportExcelAsync()
        {
            var path = await Task.Run(() =>
                _userService.ExportUserAttendance(
                    _currentUserId,
                    DateTime.Today.AddMonths(-1),
                    DateTime.Today));

            // 这里可以添加打开文件资源管理器的代码
        }

        private async Task SubmitLeaveAsync()
        {
            var success = await Task.Run(() =>
                _userService.SubmitRequest(
                    _currentUserId,
                    RequestType.BusinessTrip,
                    LeaveReason,
                    LeaveStartDate,
                    LeaveEndDate));

            // 处理提交结果
        }

        private async Task SubmitTravelAsync()
        {
            var success = await Task.Run(() =>
                _userService.SubmitRequest(
                    _currentUserId,
                    RequestType.Vacation,
                    TravelReason,
                    TravelStartDate,
                    TravelEndDate));

            // 处理提交结果
        }

        //public event PropertyChangedEventHandler? PropertyChanged;
        //protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        //    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
