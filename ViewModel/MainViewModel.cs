using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
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
        [ObservableProperty]
        private ObservableObject? _currentViewModel;
        private readonly INavigationService _navigator;

        public MainViewModel(INavigationService navigation)
        { 
            _navigator = navigation;
            _navigator.CurrentViewModelChanged += () =>
            {
                CurrentViewModel = _navigator.CurrentViewModel;
            };
            _navigator.NavigateTo<HomeViewModel>();
        }
    }
}
