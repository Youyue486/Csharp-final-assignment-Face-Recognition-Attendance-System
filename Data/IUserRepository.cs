using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Data
{
    interface IUserRepository
    {
        // 获取用户信息
        User GetById(int Userid);
        public User GetByName(string name);
        // 获取所有用户
        ICollection<User> GetAllUsers();
        // 添加用户
        void Add(User user);
        // 更新用户信息
        void Update(User user);
        // 删除用户
        void DeleteById(int Userid);
    }
}
