using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Data
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        // 获取用户信息
        public User GetById(int Userid)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Id == Userid);
            if(user ==  null)
                throw new Exception("用户不存在");
            return user;
        }
        public User GetByName(string name)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Name == name);
            if (user == null)
                throw new Exception("用户不存在");
            return user;
        }
        // 获取所有用户
        public ICollection<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        // 添加用户
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        // 更新用户信息
        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        // 删除用户
        public void DeleteById(int Userid)
        {
            var user = _context.Users.Find(Userid);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            else
                throw new Exception("用户不存在");
        }
    }
}
