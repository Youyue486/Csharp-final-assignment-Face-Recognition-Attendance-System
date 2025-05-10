using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Data
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;
        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }
        // 添加组
        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
            _context.SaveChanges();
        }
        // 获取所有组
        public ICollection<Group> GetAllGroups()
        {
            return _context.Groups.ToList();
        }
        // 根据ID获取组  
        public Group GetGroupByName(string name)
        {
            Group? group = _context.Groups
                .FirstOrDefault(g => g.GroupName == name);
            // 检查组是否存在
            if (group == null)
            {
                throw new Exception("组不存在");
            }
            return group;
        }
        // 更新组
        public void UpdateGroup(Group group)
        {
            _context.Groups.Update(group);
            _context.SaveChanges();
        }
        // 删除组
        public void DeleteGroupByName(string name)
        {
            var group = GetGroupByName(name);
            if (group != null)
            {
                _context.Groups.Remove(group);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("组不存在");
            }
        }
    }
}
