using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using NavigationService = Csharp_final_assignment_Face_Recognition_Attendance_System.Business.NavigationService;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly INavigationService _navigator;
        public HomeViewModel(INavigationService navigator) => _navigator = navigator;

        [RelayCommand]
        public void Login()
        { 
            _navigator.NavigateTo<LoginViewModel>();
        }
    }
}
