using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs;
using Csharp_final_assignment_Face_Recognition_Attendance_System.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class AdminViewModel : ObservableObject
    {
        private readonly IAdminService _adminService;
        private ObservableObject _currentView = App.ServiceProvider.GetRequiredService<UserListViewModel>();
        public ObservableObject CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        public AdminViewModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [RelayCommand]
        void SwitchView(string viewName)
        {
            switch (viewName)
            {
                case "UserList":
                    CurrentView = App.ServiceProvider.GetRequiredService<UserListViewModel>();
                    break;
                case "AttendanceRule":
                    CurrentView = App.ServiceProvider.GetRequiredService<AttendanceRuleViewModel>();
                    break;
                case "UsersRequests":
                    CurrentView = App.ServiceProvider.GetRequiredService<UsersRequestsViewModel>();
                    break;
            }
        }    
    }
}
