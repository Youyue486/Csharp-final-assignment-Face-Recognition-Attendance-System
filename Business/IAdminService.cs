using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    interface IAdminService
    {
        #region 浏览所有用户
        //增加用户
        void AddUser(string name, byte[] password, int EmployNumber, int Age = 18, UserRole userRole = UserRole.Normal, string? Email = null, string? Phone = null);
        
        //删除用户
        void DeleteUser(int id);
        
        //修改用户信息
        void UpdateUser(int id, string name, string password, int EmployNumber, int Age = 18, UserRole userRole = UserRole.Normal, string? Email = null, string? Phone = null);
        
        //查询用户信息
        UserDTO UserInfomation(int id);
        ICollection<User>? GetAllUsers();//默认按字典序
        ICollection<User>? GetUsersByGroup(Group group);
        ICollection<User>? GetUsersByUserStatuses(UserStatusType userStatusType);//可以根据多个状态查询 

        //TODO导出考勤
        #endregion

        #region 浏览所有组
        //增
        void AddGroup(string groupName, string? groupDescription = null);
        //删
        void DeleteGroupByName(string gruopName);
        void DeleteSchdule(int groupId);
        void DeleteHolidayAdjustment(int groupId);
        //改
        void SetSchdule(int groupId, GroupSchedule schedule);
        void SetHolidayAdjustment(int groupId, HolidayAdjustment adjustment);
        //查
        void GetAllGroups();
        #endregion

        #region 用户请求
        void GetAllUsersRequest();
        void AcceptUserRequest(int requestId);
        void RejectUserRequest(int requestId);
        #endregion

        #region 报表

        public double GetUserAttendanceRate(int userId, DateTime startDate, DateTime endDate);
        public double GetGroupAttendanceRate(int groupId, DateTime startDate, DateTime endDate);
        public double GetAllUsersAttendanceRate(DateTime startDate, DateTime endDate);
        #endregion
    }
}
