using CommunityToolkit.Mvvm.ComponentModel;
using Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    public class NavigationService : INavigationService
    {
        public required ObservableObject _currentViewModel;


        public void NavigateTo<T>()
        {
            var viewModel = App.ServiceProvider.GetService<T>() as ObservableObject;
            if (viewModel == null)
            {
                throw new InvalidOperationException($"Unable to resolve service for type {typeof(T)}.");
            }
            CurrentViewModel = viewModel;
        }

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    CurrentViewModelChanged?.Invoke();
                }
            }
        }
        public event Action? CurrentViewModelChanged;
    }
}
