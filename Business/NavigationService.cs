using CommunityToolkit.Mvvm.ComponentModel;
using Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _provider;
        private readonly MainViewModel _mainVM;

        public NavigationService(IServiceProvider provider, MainViewModel mainVM)
        {
            _provider = provider;
            _mainVM = mainVM;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ObservableObject
        {
            var vm = _provider.GetRequiredService<TViewModel>();
            _mainVM.CurrentViewModel = vm;
        }
    }
}
