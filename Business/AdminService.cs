using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
using Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    class AdminService : IAdminService
    {
        [Required]
        private IUserRepository _userRepository;
        private IGroupRepository _groupRepository;
        public AdminService(IUserRepository userRepository, IGroupRepository groupRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }
        
        #region 浏览所有用户  DONE
        //增加用户
        public void AddUser(string name, byte[] password, int EmployNumber, int Age = 18, UserRole userRole = UserRole.Normal, string? Email = null, string? Phone = null)
        {
            User user = new User()
            {
                Name = name,
                Password = password,
                EmployeeNumber = EmployNumber,
                Age = Age,
                userRole = UserRole.Normal,
                Email = Email,
                PhoneNumber = Phone
            };
            _userRepository.Add(user);
        }
        
        //删除用户
        public void DeleteUser(int id)
        {
            _userRepository.DeleteById(id);
        }
        
        //修改用户信息
        public void UpdateUser(int id, string name, string password, int EmployNumber, int Age = 18, UserRole userRole = UserRole.Normal, string? Email = null, string? Phone = null)
        {
            throw new NotImplementedException();
        }

        //查询用户信息
        public UserDTO UserInfomation(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            UserDTO userDTO = new UserDTO()
            {
                Name = user.Name,
                EmployeeNumber = user.EmployeeNumber,
                Age = user.Age,
                userRole = UserRole.Normal,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AttendanceRecords = user.AttendanceRecords,
                AttendenceRate = user.AttendenceRate,
                Group = user.Group,
                GroupId = user.GroupId,
                Statuses = user.Statuses,
                Requests = user.Requests
            };
            return userDTO;
        }
        public ICollection<User>? GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
        public ICollection<User>? GetUsersByUserStatuses(UserStatusType Statuses)
        {
            ICollection<User> users = _userRepository.GetAllUsers();
            ICollection<User>? selectedUsers = users.Where(user => (user.Statuses & Statuses) != 0) as ICollection<User>;
            return selectedUsers;
        }
        public ICollection<User>? GetUsersByGroup(Group group)
        {
            ICollection<User> users = _userRepository.GetAllUsers();
            ICollection<User>? selectedUsers = users.Where(user => user.Group == group) as ICollection<User>;
            return selectedUsers;
        }
        //TODO导出考勤
        #endregion

        #region 浏览所有组  TODO
        //增
        public void AddGroup(string groupName, string? groupDescription = null)
        {
            Group group = new Group()
            {
                GroupName = groupName,
                groupDescription = groupDescription
            };
            _groupRepository.AddGroup(group);
        }
        //删
        public void DeleteGroupByName(string groupName)
        {
            _groupRepository.DeleteGroupByName(groupName);
        }
        public void DeleteSchdule(int groupId)
        {
            throw new NotImplementedException();
        }
        public void DeleteHolidayAdjustment(int groupId)
        {
            throw new NotImplementedException();
        }
        //改
        public void SetSchdule(int groupId, GroupSchedule schedule)
        {
            throw new NotImplementedException();
        }     
        public void SetHolidayAdjustment(int groupId, HolidayAdjustment adjustment)
        {
            throw new NotImplementedException();
        }
        //查
        public void GetAllGroups()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 用户请求  TODO
        public void GetAllUsersRequest()
        {
            throw new NotImplementedException();
        }
        public void AcceptUserRequest(int requestId)
        {
            throw new NotImplementedException();
        }
        public void RejectUserRequest(int requestId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 报表  TODO
        public double GetUserAttendanceRate(int userId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
        public double GetGroupAttendanceRate(int groupId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
        public double GetAllUsersAttendanceRate(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        #endregion

        
    }
}
