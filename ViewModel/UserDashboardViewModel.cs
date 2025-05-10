using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    class UserDashboardViewModel
    {
        private readonly IUserService _userService;
        private readonly int _currentUserId;

        // 数据集合
        public ObservableCollection<AttendanceRecord> AttendanceRecords { get; } = new();

        // 申请表单字段
        public DateTime LeaveStartDate { get; set; } = DateTime.Today;
        public DateTime LeaveEndDate { get; set; } = DateTime.Today.AddDays(1);
        public string LeaveReason { get; set; } = string.Empty;

        public DateTime TravelStartDate { get; set; } = DateTime.Today;
        public DateTime TravelEndDate { get; set; } = DateTime.Today.AddDays(1);
        public string TravelReason { get; set; } = string.Empty;

        // 命令系统
        public RelayCommand LoadAttendanceCommand { get; }
        public RelayCommand ExportExcelCommand { get; }
        public RelayCommand SubmitLeaveCommand { get; }
        public RelayCommand SubmitTravelCommand { get; }

        public UserDashboardViewModel(IUserService userService, int userId)
        {
            _userService = userService;
            _currentUserId = userId;

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
                    RequestType.Leave,
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
                    RequestType.Travel,
                    TravelReason,
                    TravelStartDate,
                    TravelEndDate));

            // 处理提交结果
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
