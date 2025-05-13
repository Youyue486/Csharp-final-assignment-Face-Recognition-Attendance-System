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
using System.Threading.Channels;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    // Change the base class from ObservableObject to ObservableValidator
    public partial class InputDialogViewModel : ObservableValidator
    {
        private readonly IAdminService _adminService;

        [ObservableProperty]
        private string _name = "";
        [ObservableProperty]
        private string? _password;
        [ObservableProperty]
        private string? _confirmPassword;
        [ObservableProperty]
        private string _employeeNumber = "";
        [ObservableProperty]
        private ICollection<GroupDTO> _groupDTOs;
        [ObservableProperty]
        private int? _age;
        [ObservableProperty]
        private bool _isAdmin = false;
        [ObservableProperty]
        private bool _isNotAdmin = true;
        [ObservableProperty]
        private string? _email;
        [ObservableProperty, Phone]
        private string? _phoneNumber;

        // Rename the backing field to follow the "lowerCamel" pattern and make it readonly
        [ObservableProperty]
        private GroupDTO? _selectedGroupDTO;

        public InputDialogViewModel(IAdminService adminService)
        {
            _adminService = adminService;
            _groupDTOs = _adminService.GetAllGroupsDTO();
            _selectedGroupDTO = _groupDTOs.FirstOrDefault();
        }

        [RelayCommand]
        public void Confirm()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ValidationException("Name cannot be empty.");
            }
            if (Password != ConfirmPassword)
            {
                throw new ValidationException("Passwords do not match.");
            }
            if (string.IsNullOrWhiteSpace(EmployeeNumber))
            {
                throw new ValidationException("Employee Number cannot be empty.");
            }
            if (Age != null && (Age < 18 || Age > 70))
            {
                throw new ValidationException("Age must be between 18 and 70.");
            }

            // Fix for CS8604: Ensure Password is not null before calling Encoding.UTF8.GetBytes
            byte[]? passwordBytes = Password != null ? Encoding.UTF8.GetBytes(Password) : null;

            UserRole userRole = IsAdmin ? UserRole.Admin : UserRole.Normal;

            _adminService.AddUser(
                Name,
                passwordBytes,
                int.Parse(EmployeeNumber),
                SelectedGroupDTO?.Name,
                userRole,
                Age,
                Email,
                PhoneNumber
            );
            CloseDialog?.Invoke();
        }

        [RelayCommand]
        public void Cancel()
        {
            CloseDialog?.Invoke();
        }

        // 添加一个事件以供外部绑定关闭弹窗逻辑
        public event Action? CloseDialog;

    }
}
