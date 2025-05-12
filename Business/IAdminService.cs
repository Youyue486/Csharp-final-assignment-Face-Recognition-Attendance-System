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
    public interface IAdminService
    {
        #region 浏览所有用户
        //增加用户
        public void AddUser(string name, byte[]? password, int EmployNumber, string? GroupId, UserRole userRole = UserRole.Normal, int? Age = 18, string? Email = null, string? Phone = null);
        
        //删除用户
        public void DeleteUser(string name);
        
        //修改用户信息
        public void UpdateUser(int id, string name, byte[]? password, int EmployNumber, string? GroupName, int? Age = 18, UserRole? userRole = UserRole.Normal, string? Email = null, string? Phone = null);
        public void UpdateUserGroup(string name, string groupName);
        //查询用户信息
        public UserDTO UserInfomation(int id);
        public ICollection<UserDTO> GetAllUsersDTO();//默认按字典序
        public ICollection<User> GetUsersByGroup(Group group);
        public ICollection<UserDTO> GetUserDTOsByUserStatuses(UserStatusType? userStatusType);//可以根据多个状态查询 

        //TODO导出考勤
        #endregion

        #region 浏览所有组
        //增
        public void AddGroup(string groupName, string? groupDescription = null);
        //删
        public void DeleteGroupByName(string gruopName);
        public void DeleteSchdule(string groupName);
        public void DeleteHolidayAdjustment(string groupName);
        //改
        public void SetSchdule(string groupName, GroupSchedule schedule);
        public void SetHolidayAdjustment(string groupNamest, HolidayAdjustment adjustment);
        //查
        public ICollection<Group> GetAllGroups();
        #endregion

        #region 用户请求
        public void GetAllUsersRequest();
        public void AcceptUserRequest(int requestId);
        public void RejectUserRequest(int requestId);
        #endregion

        #region 报表
        //TODO 换成可以按时间段查询
        public double GetUserAttendanceRate(string userName);
        public double GetGroupAttendanceRate(string groupName);
        public double GetAverageUsersAttendanceRate();
        #endregion
    }
}
