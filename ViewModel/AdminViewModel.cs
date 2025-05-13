using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs;
using Csharp_final_assignment_Face_Recognition_Attendance_System.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class AdminViewModel : ObservableObject
    {
        private readonly IAdminService _adminService;
        private readonly IDlibFaceRecognitionService _faceRecognitionService;
        [ObservableProperty]
        private UserStatusType? selectedStatusFilter;
        partial void OnSelectedStatusFilterChanged(UserStatusType? value)
        {
            LoadUserDTOs(value);
        }
        [ObservableProperty]
        ICollection<UserDTO> users;

        public AdminViewModel(IAdminService adminService,IDlibFaceRecognitionService faceRecognitionService)
        {
            _adminService = adminService;
            _faceRecognitionService = faceRecognitionService;
            Users = adminService.GetAllUsersDTO();
        }

        private void LoadUserDTOs(UserStatusType? value)
        {
            if (value == null)
                Users = _adminService.GetAllUsersDTO();
            else
            {
                Users = _adminService.GetUserDTOsByUserStatuses(value);
            }
        }

        [RelayCommand]
        public void AddUser()
        {
            var dialog = new InputDialog();
            var viewModel = new InputDialogViewModel(_adminService,_faceRecognitionService);
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
    }
}
