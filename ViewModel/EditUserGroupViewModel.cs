using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class EditUserGroupViewModel : ObservableObject
    {
        private IAdminService _adminService;
        [ObservableProperty]
        private ICollection<Group> _groups;
        [ObservableProperty]
        private Group? _selectedGroup;
        private readonly UserDTO _userDTO;

        public EditUserGroupViewModel(IAdminService adminService, UserDTO user)
        {
            _adminService = adminService;
            _groups = _adminService.GetAllGroups();
            _selectedGroup = _groups.FirstOrDefault();
            _userDTO = user;
        }

        [RelayCommand]
        public void Confirm()
        {
            if (null == SelectedGroup)
                return;
            _adminService.UpdateUserGroup(_userDTO.Name, SelectedGroup.GroupName);
            CloseDialog?.Invoke();
        }

        [RelayCommand]
        public void Cancel()
        {
            CloseDialog?.Invoke();
        }

        public event Action? CloseDialog;
    }
}
