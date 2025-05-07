using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigation;

        public MainViewModel(INavigationService navigation)
        { 
            _navigation = navigation;
        }
        [RelayCommand]
        private void OpenAdmin() => _navigation.NavigateTo(nameof(AdminViewModel));

    }
}
