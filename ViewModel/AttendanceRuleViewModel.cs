using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs;
using Csharp_final_assignment_Face_Recognition_Attendance_System.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class AttendanceRuleViewModel : ObservableObject
    {
        private readonly IAdminService _adminService;

        [ObservableProperty]
        private ICollection<GroupDTO> groupsDTO;

        [ObservableProperty]
        private string searchText = "";

        #region 设置组工作日属性
        private WorkDay _selectedWorkDay = 0;
        public WorkDay SelectedWorkDay
        {
            get => _selectedWorkDay;
            set
            {
                if (_selectedWorkDay != value)
                {
                    _selectedWorkDay = value;
                    OnPropertyChanged();
                    LoadGroupsDTO();
                }
            }
        }
        public bool IsMonday
        {
            get => SelectedWorkDay.HasFlag(WorkDay.周一);
            set { SetFlag(WorkDay.周一, value); OnPropertyChanged(nameof(IsMonday)); }
        }
        public bool IsTusday
        {
            get => SelectedWorkDay.HasFlag(WorkDay.周二);
            set { SetFlag(WorkDay.周二, value); OnPropertyChanged(nameof(IsTusday)); }
        }
        public bool IsWednesday
        {
            get => SelectedWorkDay.HasFlag(WorkDay.周三);
            set { SetFlag(WorkDay.周三, value); OnPropertyChanged(nameof(IsWednesday)); }
        }
        public bool IsThursday
        {
            get => SelectedWorkDay.HasFlag(WorkDay.周四);
            set { SetFlag(WorkDay.周四, value); OnPropertyChanged(nameof(IsThursday)); }
        }
        public bool IsFriday
        {
            get => SelectedWorkDay.HasFlag(WorkDay.周五);
            set { SetFlag(WorkDay.周五, value); OnPropertyChanged(nameof(IsFriday)); }
        }
        public bool IsSaturday
        {
            get => SelectedWorkDay.HasFlag(WorkDay.周六);
            set { SetFlag(WorkDay.周六, value); OnPropertyChanged(nameof(IsSaturday)); }
        }
        public bool IsSunday
        {
            get => SelectedWorkDay.HasFlag(WorkDay.周日);
            set { SetFlag(WorkDay.周日, value); OnPropertyChanged(nameof(IsSunday)); }
        }

        private void SetFlag(WorkDay flag, bool isSet)
        {
            if (isSet)
            {
                SelectedWorkDay |= flag;
            }
            else
            {
                SelectedWorkDay &= ~flag;
            }
        }
        #endregion

        #region 设置组排序方式

        public enum SortType
        {
            [Description("按组名")]
            ByGroupName,

            [Description("按人数")]
            ByUserCount
        }
        public ICollection<SortType> SortOptions => Enum.GetValues(typeof(SortType)).Cast<SortType>().ToList();

        private SortType _selectedSortType = SortType.ByGroupName;
        public SortType SelectedSortType
        {
            get => _selectedSortType;
            set
            {
                if (_selectedSortType != value)
                {
                    _selectedSortType = value;
                    OnPropertyChanged(nameof(SelectedSortType));
                    LoadGroupsDTO();
                }
            }
        }
        #endregion

        private void LoadGroupsDTO()
        {
            // Explicitly specify the type parameters for OrderBy and OrderByDescending to resolve CS0411
            if (SelectedSortType == SortType.ByGroupName)
            {
                GroupsDTO = _adminService.GetGroupsDTOByWorkDay(SelectedWorkDay).OrderBy<GroupDTO, string>(g => g.Name).ToList();
            }
            else if (SelectedSortType == SortType.ByUserCount)
            {
                GroupsDTO = _adminService.GetGroupsDTOByWorkDay(SelectedWorkDay).OrderByDescending<GroupDTO, int>(g => g.Members.Count).ToList();
            }
            SearchGroup();
        }

        public AttendanceRuleViewModel(IAdminService adminService)
        {
            _adminService = adminService;
            GroupsDTO = adminService.GetAllGroupsDTO();
            LoadGroupsDTO();
        }

        [RelayCommand]
        void AddGroup()
        {
            var dialog = new AddGroup();
            var dialogViewModel = new AddGroupViewModel(_adminService);
            dialog.DataContext = dialogViewModel;
            dialogViewModel.CloseDialog += () =>
            {
                dialog.Close();
                LoadGroupsDTO();
            };
            dialog.ShowDialog();
        }

        [RelayCommand]
        void SearchGroup()
        {
            if (SearchText == "")
            {
                return;
            }
            var group = GroupsDTO.FirstOrDefault(g => g.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            if (group != null)
            {
                GroupsDTO = [group];
            }
            else
            {
                GroupsDTO = [];
            }
        }

        [RelayCommand]
        void DeleteGroup(GroupDTO group)
        {
            if (group == null)
            {
                return;
            }
            _adminService.DeleteGroupByName(group.Name);
            LoadGroupsDTO();
        }

        [RelayCommand]
        void EditGroup(GroupDTO group)
        {
            if (group == null)
            {
                return;
            }
            var dialog = new EditGroup();
            var dialogViewModel = new EditGroupViewModel(_adminService, group);
            dialog.DataContext = dialogViewModel;
            dialogViewModel.CloseDialog += () =>
            {
                dialog.Close();
                LoadGroupsDTO();
            };
            dialog.ShowDialog();
        }
    }
}