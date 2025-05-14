using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class UsersRequestsViewModel : ObservableObject
    {
        private IAdminService adminService;
        [ObservableProperty]
        private ICollection<Request> usersRequests;

        public UsersRequestsViewModel(IAdminService adminService)
        {
            this.adminService = adminService;
            usersRequests = adminService.LoadAllUsersPendingRequests();
        }
        public void UpdateRequests()
        {
            UsersRequests = adminService.LoadAllUsersPendingRequests();
        }

        [RelayCommand]
        public void AcceptRequest(int requestId)
        {
            adminService.AcceptUserRequest(requestId);
            UpdateRequests();
        }
        [RelayCommand]
        public void RejectRequest(int requestId)
        {
            adminService.RejectUserRequest(requestId);
            UpdateRequests();
        }
    }
}
