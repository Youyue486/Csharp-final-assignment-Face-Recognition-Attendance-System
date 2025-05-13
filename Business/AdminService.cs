using CommunityToolkit.Mvvm.ComponentModel;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
using Csharp_final_assignment_Face_Recognition_Attendance_System.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    class AdminService : IAdminService
    {
        private IUserRepository _userRepository;
        private IGroupRepository _groupRepository;
        public AdminService(IUserRepository userRepository, IGroupRepository groupRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }
        
        #region 浏览所有用户  DONE!
        //增加用户
        public void AddUser(string name, byte[]? password, int EmployNumber, string? GroupName, UserRole userRole, int? Age = 18, string? Email = null, string? Phone = null)
        {
            Group? Group = _groupRepository.GetGroupByName(GroupName);
            int? GroupId = Group?.Id;
            User user = new User()
            {
                Name = name,
                Password = password,
                EmployeeNumber = EmployNumber,
                GroupId = GroupId,
                Age = Age,
                userRole = userRole,
                Email = Email,
                PhoneNumber = Phone
            };
            _userRepository.Add(user);
        }
        
        //删除用户
        public void DeleteUser(string name)
        {
            _userRepository.DeleteByName(name);
        }
        
        //修改用户信息
        public void UpdateUser(int id,
                               string name,
                               byte[]? password, 
                               int EmployNumber,
                               string? GroupName,
                               int? Age = 18, 
                               UserRole? userRole = UserRole.Normal, 
                               string? Email = null, 
                               string? Phone = null
            )
        {
            throw new NotImplementedException();
        }
        public void UpdateUserGroup(string name, string groupName)
        {
            User user = _userRepository.GetByName(name);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            Group? group = _groupRepository.GetGroupByName(groupName);
            if (group == null)
            {
                throw new Exception($"Group {groupName} not found");
            }
            user.GroupId = group.Id;
            _userRepository.Update(user);
        }

        //查询用户信息
        public UserDTO UserInfomation(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var group = _groupRepository.GetGroupById(user.GroupId);
            string? groupName = group != null ? group.GroupName : null;
            UserDTO userDTO = new UserDTO()
            {
                Name = user.Name,
                EmployeeNumber = user.EmployeeNumber,
                Age = user.Age,
                userRole = user.userRole,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AttendanceRecords = user.AttendanceRecords,
                AttendenceRate = user.AttendenceRate,
                GroupName = groupName,
                GroupId = user.GroupId,
                Statuses = user.Statuses,
                Requests = user.Requests
            };
            return userDTO;
        }
        public ICollection<UserDTO> GetAllUsersDTO()
        {
            var users = _userRepository.GetAllUsers();
            var usersDTO = new List<UserDTO>();
            foreach (var user in users)
            {
                usersDTO.Add(UserInfomation(user.Id));
            }
            return usersDTO;
        }

        public ICollection<UserDTO> FindUserDTOsByUserStatuses(ICollection<UserDTO> users, UserStatusType? Statuses)
        {
            if (Statuses == null) return GetAllUsersDTO();
            ICollection<UserDTO>? selectedUsers = users.Where(user => (user.Statuses & Statuses) == Statuses).ToList();
            ICollection<UserDTO>? selectedUsersDTO = [];
            return selectedUsersDTO;
        }
        public ICollection<User> GetUsersByGroup(Group group)
        {
            ICollection<User> users = _userRepository.GetAllUsers();
            var selectedUsers = users.Where(user => user.Group == group) as ICollection<User>;
            return selectedUsers == null?[]:selectedUsers;
        }
        public ICollection<UserDTO> FindUserDTOsByNameAllowDuplicated(ICollection<UserDTO> users, string name)
        {
            var selectedUsers = users.Where(u => u.Name.Contains(name)).ToList();
            return selectedUsers;
        }
        //TODO导出考勤
        #endregion

        #region 浏览所有组  DONE!
        //增
        public void AddGroup(string groupName, string? groupDescription = null)
        {
            Group group = new Group()
            {
                GroupName = groupName,
                GroupDescription = groupDescription
            };
            _groupRepository.AddGroup(group);
        }
        //删
        public void DeleteGroupByName(string groupName)
        {
            _groupRepository.DeleteGroupByName(groupName);
        }
        public void DeleteSchdule(string groupName)
        {
            Group? group = _groupRepository.GetGroupByName(groupName);
            if (group == null)
            {
                throw new Exception($"不存在组:{groupName}");
            }
            group.Schedule = null;
            _groupRepository.UpdateGroup(group);
        }
        public void DeleteHolidayAdjustment(string groupName)
        {
            //TODO
            throw new NotImplementedException();
        }
        //改
        public void SetSchdule(string groupName, GroupSchedule schedule)
        {
            Group? group = _groupRepository.GetGroupByName(groupName);
            if (group == null)
            {
                throw new Exception($"不存在组:{groupName}");
            }
            group.Schedule = schedule;
            _groupRepository.UpdateGroup(group);
        }     
        public void SetHolidayAdjustment(string groupName, HolidayAdjustment adjustment)
        {
            //TODO
            throw new NotImplementedException();
        }
        //查
        public GroupDTO GetGroupDTOByName(string groupName)
        {
            Group? group = _groupRepository.GetGroupByName(groupName);
            if (group == null)
            {
                throw new Exception($"不存在组:{groupName}");
            }
            var members = group.Users?
                               .Select(member => UserInfomation(member.Id))
                               .Where(dto => dto != null)
                               .ToList() ?? new List<UserDTO>();
            GroupDTO groupDTO = new GroupDTO()
            {
                Id = group.Id,
                Name = group.GroupName,
                Description = group.GroupDescription,
                Members = members,
                MemberCount = (group.Users == null) ? 0 :group.Users.Count,
                WorkDays = group.Schedule?.WorkDays,
                WorkStartTime = group.Schedule?.WorkStartTime,
                WorkEndTime = group.Schedule?.WorkEndTime
            };
            return groupDTO;
        }
        public ICollection<GroupDTO> GetAllGroupsDTO()
        {
            List<Group> groups = _groupRepository.GetAllGroups().ToList();
            ICollection<GroupDTO> groupDTOs = [];
            foreach (var group in groups)
            {
                groupDTOs.Add(GetGroupDTOByName(group.GroupName));
            }
            return groupDTOs;
        }
        public ICollection<GroupDTO> GetGroupsDTOByWorkDay(WorkDay workDay)
        {
            ICollection<Group> groups = _groupRepository.GetAllGroups();
            ICollection<GroupDTO> groupDTOs = [];
            foreach (Group group in groups)
            {
                if ((group.Schedule?.WorkDays & workDay) == workDay)
                {
                    groupDTOs.Add(GetGroupDTOByName(group.GroupName));
                }
            }
            return groupDTOs;
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

        #region 报表  TODO 换成可以按时间段查询
        public double GetUserAttendanceRate(string userName)
        {
            User user = _userRepository.GetByName(userName);
            if (user == null)
            {
                throw new Exception($"未找到用户:{userName}");
            }
            double attendanceRate = user.AttendenceRate;
            return attendanceRate;
        }
        public double GetGroupAttendanceRate(string groupName)
        {
            Group? group = _groupRepository.GetGroupByName(groupName);
            if (group == null)
            {
                throw new Exception($"未找到组:{groupName}");
            }
            double attendanceRate = 0;
            foreach(User user in group.Users)
            {
                attendanceRate += user.AttendenceRate;
            }
            attendanceRate /= group.Users.Count;
            return attendanceRate;
        }
        public double GetAverageUsersAttendanceRate()
        {
            ICollection<User> users = _userRepository.GetAllUsers();
            double attendanceRate = 0;
            foreach (User user in users)
            {
                attendanceRate += user.AttendenceRate;
            }
            attendanceRate /= users.Count;
            return attendanceRate;
        }
        #endregion
    }
}
