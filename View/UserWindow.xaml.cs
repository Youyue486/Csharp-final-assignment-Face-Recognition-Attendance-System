using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel;
using System.Windows;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.View
{
    public partial class UserWindow : Window
    {
        public UserWindow(IUserService userService, int userId)
        {
            InitializeComponent();
            DataContext = new UserDashboardViewModel(userService, userId);
        }
    }
}