using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs;
using Csharp_final_assignment_Face_Recognition_Attendance_System.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class UserListViewModel : ObservableObject
    {
        private readonly IAdminService _adminService;

        [ObservableProperty]
        ICollection<UserDTO> users = [];

        [ObservableProperty]
        private string searchText = string.Empty;

        #region 设置员工状态属性
        private UserStatusType _selectedStatus = 0;
        public UserStatusType SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged();
                    UpdateUserDTOs();
                }
            }
        }

        public bool IsOnDuty
        {
            get => SelectedStatus.HasFlag(UserStatusType.在班);
            set { SetFlag(UserStatusType.在班, value); OnPropertyChanged(nameof(IsOnDuty)); }
        }
        public bool IsOffDuty
        {
            get => SelectedStatus.HasFlag(UserStatusType.下班);
            set { SetFlag(UserStatusType.下班, value); OnPropertyChanged(nameof(IsOffDuty)); }
        }
        public bool IsOvertime
        {
            get => SelectedStatus.HasFlag(UserStatusType.加班);
            set { SetFlag(UserStatusType.加班, value); OnPropertyChanged(nameof(IsOvertime)); }
        }
        public bool IsOnBusinessTrip
        {
            get => SelectedStatus.HasFlag(UserStatusType.出差);
            set { SetFlag(UserStatusType.出差, value); OnPropertyChanged(nameof(IsOnBusinessTrip)); }
        }
        public bool IsAbsent
        {
            get => SelectedStatus.HasFlag(UserStatusType.旷班);
            set { SetFlag(UserStatusType.旷班, value); OnPropertyChanged(nameof(IsAbsent)); }
        }
        public bool IsLate
        {
            get => SelectedStatus.HasFlag(UserStatusType.迟到);
            set { SetFlag(UserStatusType.迟到, value); OnPropertyChanged(nameof(IsLate)); }
        }
        public bool IsLeaveEarly
        {
            get => SelectedStatus.HasFlag(UserStatusType.早退);
            set { SetFlag(UserStatusType.早退, value); OnPropertyChanged(nameof(IsLeaveEarly)); }
        }
        public bool IsOnLeave
        {
            get => SelectedStatus.HasFlag(UserStatusType.请假);
            set { SetFlag(UserStatusType.请假, value); OnPropertyChanged(nameof(IsOnLeave)); }
        }

        private void SetFlag(UserStatusType flag, bool isSet)
        {
            if (isSet)
                SelectedStatus |= flag;
            else
                SelectedStatus &= ~flag;
        }
        #endregion

        #region 设置排序方式
        public enum SortType
        {
            [Description("按姓名")]
            ByName,

            [Description("按小组")]
            ByGroup
        }
        public ICollection<SortType> SortOptions => Enum.GetValues(typeof(SortType)).Cast<SortType>().ToList();

        private SortType _selectedSortType = SortType.ByName;
        public SortType SelectedSortType
        {
            get => _selectedSortType;
            set
            {
                if (_selectedSortType != value)
                {
                    _selectedSortType = value;
                    OnPropertyChanged();
                    UpdateUserDTOs();
                }
            }
        }
        #endregion

        public UserListViewModel(IAdminService adminService)
        {
            _adminService = adminService;
            LoadUserDTOs();
        }

        private void UpdateUserDTOs()
        {
            LoadUserDTOs();
            if (SelectedStatus == 0)
            {
                if (SelectedSortType == SortType.ByGroup)
                    Users = Users.OrderBy(u => u.GroupName)
                                 .ToList();
                else if (SelectedSortType == SortType.ByName)
                    Users = Users.OrderBy(u => u.Name)
                                 .ToList();
            }
            else
            {
                if (SelectedSortType == SortType.ByGroup)
                    Users = _adminService.FindUserDTOsByUserStatuses(Users, SelectedStatus)
                                         .OrderBy(u => u.GroupName)
                                         .ToList();
                else if (SelectedSortType == SortType.ByName)
                    Users = _adminService.FindUserDTOsByUserStatuses(Users, SelectedStatus)
                                         .OrderBy(u => u.Name)
                                         .ToList();
            }
            SearchUser();
        }

        private void LoadUserDTOs()
        {
            Users = _adminService.GetAllUsersDTO();
        }

        #region 命令
        [RelayCommand]
        public void AddUser()
        {
            var dialog = new InputDialog();
            var viewModel = new InputDialogViewModel(_adminService);
            dialog.DataContext = viewModel;
            viewModel.CloseDialog += () =>
            {
                dialog.Close();
            };
            dialog.ShowDialog();
            Users = _adminService.GetAllUsersDTO();//优化TODO:这里每次添加用户后都要重新加载一次列表
        }

        [RelayCommand]
        public void EditUserGroup(UserDTO user)
        {
            var dialog = new EditUserGroup();
            var viewModel = new EditUserGroupViewModel(_adminService, user);
            dialog.DataContext = viewModel;
            viewModel.CloseDialog += () =>
            {
                dialog.Close();
            };
            dialog.ShowDialog();
            Users = _adminService.GetAllUsersDTO();
        }

        [RelayCommand]
        public void DeleteUser(UserDTO user)
        {
            if (user == null)
                return;
            _adminService.DeleteUser(user.Name);
            Users = _adminService.GetAllUsersDTO();
            Users = _adminService.GetAllUsersDTO();
        }

        [RelayCommand]
        public void SearchUser()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return;
            }
            Users = _adminService.FindUserDTOsByNameAllowDuplicated(Users, SearchText);
        }

        #endregion
    }
}
