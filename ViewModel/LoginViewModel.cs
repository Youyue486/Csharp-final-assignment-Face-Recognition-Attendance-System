using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigator;
        public LoginViewModel(INavigationService navigator) => _navigator = navigator;
        //TODO: 需要实现登录功能
        [RelayCommand]
        public void Login()
        { 
            _navigator.NavigateTo<EmployeeLoginViewModel>();
        }
    }
}
