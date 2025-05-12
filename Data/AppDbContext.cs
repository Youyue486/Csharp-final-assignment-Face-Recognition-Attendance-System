using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;
using Group = Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models.Group;


namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Data
{
    public class AppDbContext : DbContext
    {
        // 数据库表映射
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupSchedule> GroupSchedules { get; set; }
        public DbSet<HolidayAdjustment> HolidayAdjustments { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        //public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<Request> Requests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 数据库初始化方法
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            context.Database.EnsureCreated();

            // 添加初始测试数据
            if (!context.Groups.Any())
            {
                context.Groups.Add(new Group
                {
                    GroupName = "默认组",
                    Schedule = new GroupSchedule
                    {
                        WorkStartTime = new TimeSpan(9, 0, 0),
                        WorkEndTime = new TimeSpan(18, 0, 0),
                        CheckInWindow = 15,
                        WorkDays = WorkDay.Monday | WorkDay.Tuesday | WorkDay.Wednesday | WorkDay.Thursday | WorkDay.Friday
                    }
                });
                context.SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ------------------------ 实体关系配置 ------------------------
            // User ↔ Group（多对一）
            modelBuilder.Entity<User>()
                .HasOne(u => u.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.SetNull); // 删除Group时保留用户，设为NULL

            // Group ↔ GroupSchedule（一对一）
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Schedule)
                .WithOne(s => s.Group)
                .HasForeignKey<GroupSchedule>(s => s.GroupId)
                .OnDelete(DeleteBehavior.Cascade); // 删除Group时级联删除Schedule

            // Group ↔ HolidayAdjustment（一对多）
            modelBuilder.Entity<HolidayAdjustment>()
                .HasOne(h => h.Group)
                .WithMany(g => g.HolidayAdjustments)
                .HasForeignKey(h => h.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // User ↔ AttendanceRecord（一对多）
            modelBuilder.Entity<AttendanceRecord>()
                .HasOne(a => a.User)
                .WithMany(u => u.AttendanceRecords)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //// User ↔ UserStatus（一对多）
            //modelBuilder.Entity<UserStatus>()
            //    .HasOne(s => s.User)
            //    .WithMany(u => u.Statuses)
            //    .HasForeignKey(s => s.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // User ↔ Request（一对多）
            modelBuilder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ------------------------ 枚举类型配置 ------------------------
            // 将枚举存储为字符串（更易读）
            //modelBuilder.Entity<UserStatus>()
            //    .Property(s => s.StatusType)
            //    .HasConversion<string>()
            //    .HasMaxLength(20);

            modelBuilder.Entity<Request>()
                .Property(r => r.RequestType)
                .HasConversion<string>()
                .HasMaxLength(20);

            // ------------------------ 索引配置 ------------------------
            // 用户工号唯一索引
            modelBuilder.Entity<User>()
                .HasIndex(u => u.EmployeeNumber)
                .IsUnique();

            // 小组名称唯一索引
            modelBuilder.Entity<Group>()
                .HasIndex(g => g.GroupName)
                .IsUnique();

            //// 用户状态时间范围查询优化
            //modelBuilder.Entity<UserStatus>()
            //    .HasIndex(s => new { s.UserId, s.StartTime, s.EndTime });

            // 打卡记录按用户和时间查询优化
            modelBuilder.Entity<AttendanceRecord>()
                .HasIndex(a => new { a.UserId, a.CheckTime });
        }
    }
}
