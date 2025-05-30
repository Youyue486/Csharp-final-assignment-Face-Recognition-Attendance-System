using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    public interface INavigationService
    {
        public void NavigateTo<T>();
        public event Action CurrentViewModelChanged;
        ObservableObject CurrentViewModel { get; set; }
    }
}
