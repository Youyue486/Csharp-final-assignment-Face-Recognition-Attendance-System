using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    // Change the base class from ObservableObject to ObservableValidator
    public partial class InputDialogViewModel : ObservableValidator
    {
        private readonly IAdminService _adminService;
        private readonly IDlibFaceRecognitionService _faceRecognitionService;

        [ObservableProperty]
        private string _name = "";
        [ObservableProperty]
        private string _employeeNumber = "";
        [ObservableProperty]
        private ICollection<Group> _groups;
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
        private Group? _selectedGroup;

        private string ImageFilePath;
        private byte[] features;

        public InputDialogViewModel(IAdminService adminService,IDlibFaceRecognitionService dlibFaceRecognitionService)
        {
            _adminService = adminService;
            _groups = _adminService.GetAllGroups();
            _selectedGroup = _groups.FirstOrDefault();
            _faceRecognitionService = dlibFaceRecognitionService;
        }

        [RelayCommand]
        public void Confirm()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ValidationException("Name cannot be empty.");
            }
            
            if (string.IsNullOrWhiteSpace(EmployeeNumber))
            {
                throw new ValidationException("Employee Number cannot be empty.");
            }
            if (Age != null && (Age < 18 || Age > 70))
            {
                throw new ValidationException("Age must be between 18 and 70.");
            }
            if (features == null) {
                MessageBox.Show("选择合适的照片");
                return;
            }
            // Fix for CS8604: Ensure Password is not null before calling Encoding.UTF8.GetBytes
            //byte[]? passwordBytes = Password != null ? Encoding.UTF8.GetBytes(Password) : null;

            UserRole userRole = IsAdmin ? UserRole.Admin : UserRole.Normal;

            _adminService.AddUser(
                Name,
                features,
                int.Parse(EmployeeNumber),
                SelectedGroup?.GroupName,
                userRole,
                Age,
                Email,
                PhoneNumber
            );
            CloseDialog?.Invoke();
        }
        private bool ComputeImage()
        {
            if (ImageFilePath == null)
            {
                return false;
            }
            var mat=TransformationTools.ImageToMat(ImageFilePath);
            if (mat == null)
            {
                return false;
            }
            var res=_faceRecognitionService.DetectFace(mat);
            if (res.Item2 == null) { 
                return false;
            }
            features = TransformationTools.FaceEncodingToBytes(res.Item2);
            return true;
        }

        [RelayCommand]
        public void Cancel()
        {
            CloseDialog?.Invoke();
        }
        [RelayCommand]
        public void OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "选择文件",
                Filter = "文本文件|*.txt|所有文件|*.*",
                InitialDirectory = Directory.GetCurrentDirectory()
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImageFilePath = openFileDialog.FileName;
                // 在这里可以对选择的文件进行处理，例如读取文件内容等
                if (ComputeImage()==false) {
                    MessageBox.Show("图片不合理");
                }
            }
        }
        // 添加一个事件以供外部绑定关闭弹窗逻辑
        public event Action? CloseDialog;

    }
}
