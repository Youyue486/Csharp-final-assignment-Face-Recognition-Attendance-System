using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
using Csharp_final_assignment_Face_Recognition_Attendance_System.View;
using Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;
using NavigationService = Csharp_final_assignment_Face_Recognition_Attendance_System.Business.NavigationService;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    //测试业务逻辑
    public static void test(IServiceProvider serviceProvider)
    {
        #region 初始化仓库
        // 使用 null 条件运算符和空合并运算符来处理可能的 null 值  
        IUserRepository? _userRepo = serviceProvider.GetService<IUserRepository>();
        IGroupRepository? _groupRepo = serviceProvider.GetService<IGroupRepository>();

        if (_userRepo == null)
        {
            throw new InvalidOperationException("IUserRepository service is not registered.");
        }

        if (_groupRepo == null)
        {
            throw new InvalidOperationException("IGroupRepository service is not registered.");
        }
        #endregion

        #region 初始化测试用户、用户组
        //初始化用户
        User Alice = new User
        {
            EmployeeNumber = 101,
            Name = "Alice",
            Password = [1, 2, 3, 4, 5, 6],
            userRole = UserRole.Normal
        };
        User Bob = new User
        {
            EmployeeNumber = 102,
            Name = "Bob",
            Password = [1, 2, 3, 4, 5, 6],
            userRole = UserRole.Normal
        };
        _userRepo.Add(Alice);
        _userRepo.Add(Bob);

        //初始化用户组
        Group Teacher = new Group
        {
            GroupName = "Teacher",
            groupDescription = "This is a Teacher group.",
            Schedule = new GroupSchedule
            {
                WorkStartTime = new TimeSpan(9, 0, 0),
                WorkEndTime = new TimeSpan(18, 0, 0),
                CheckInWindow = 15,
                WorkDays = (WorkDay.Monday | WorkDay.Wednesday | WorkDay.Friday),
            }
        };
        Group Student = new Group
        {
            GroupName = "Student",
            groupDescription = "This is a Student group.",
            Schedule = new GroupSchedule
            {
                WorkStartTime = new TimeSpan(9, 0, 0),
                WorkEndTime = new TimeSpan(18, 0, 0),
                CheckInWindow = 15,
                WorkDays = (WorkDay.Tuesday | WorkDay.Thursday | WorkDay.Saturday),
            }
        };
        _groupRepo.AddGroup(Teacher);
        _groupRepo.AddGroup(Student);

        //将用户添加到组
        Alice.Group = Teacher;
        Bob.Group = Student;
        _userRepo.Update(Alice);
        _userRepo.Update(Bob);
        #endregion

        

        #region 清除测试用户、用户组
        _groupRepo.DeleteGroupByName("Teacher");
        _groupRepo.DeleteGroupByName("Student");
        _userRepo.DeleteById(_userRepo.GetByName("Alice").Id);
        _userRepo.DeleteById(_userRepo.GetByName("Bob").Id);
        #endregion
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        // 初始化数据库  
        AppDbContext.Initialize(ServiceProvider);

        // 测试  
        test(ServiceProvider);

        // 显示主窗口  
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        // 释放资源idOperationException: Window 必须是树的根目录。不能将 Window 添加为 Visual 的子目录。
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    #region 配置服务
    private void ConfigureServices(IServiceCollection services)
    {
        // ------------------------ 数据访问层注册 ------------------------
        //配置数据库连接字符串
        var dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\AttendanceSystem.db");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));
        // 注册Repository
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();

        // ------------------------ 业务逻辑层注册 ------------------------
        // 注册业务服务
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IAttendanceService, AttendanceService>();
        services.AddSingleton<IFaceRecognitionService, FaceRecognitionService>();

        // ------------------------ 表示层注册 -----------------------------
        // 注册所有ViewModel（需继承ObservableObject）
        services.AddTransient<AdminViewModel>();
        services.AddTransient<UserDashboardViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<HomeViewModel>();

        //注册所有View（需继承Window）
        services.AddTransient<MainWindow>();

        // 注册 INavigationService窗口导航服务
        services.AddSingleton<INavigationService, NavigationService>();
    }
    #endregion
}
