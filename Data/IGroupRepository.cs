﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;
///////////checked!///////////////
namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Data
{
    interface IGroupRepository
    {
        // 添加组
        void AddGroup(Group group);
        // 获取所有组
        ICollection<Group> GetAllGroups();
        // 根据名字获取组
        Group GetGroupByName(string Name);
        // 更新组
        void UpdateGroup(Group group);
        // 删除组
        void DeleteGroupByName(string name);
    }
}
