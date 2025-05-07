using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
using Csharp_final_assignment_Face_Recognition_Attendance_System.View;
using Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel;
using GalaSoft.MvvmLight.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using static Csharp_final_assignment_Face_Recognition_Attendance_System.Core.Models;

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
        // 使用 null 条件运算符和空合并运算符来处理可能的 null 值  
        IUserRepository? test_Repo = serviceProvider.GetService<IUserRepository>();

        IAttendanceRepository? test_Att_Repo = serviceProvider.GetService<IAttendanceRepository>();

        // 用户增删改查测试  
        if (test_Repo?.GetAllUsers() == null)
        {
            throw new Exception("用户表为空，请先添加用户。");
        }
        var users = test_Repo.GetAllUsers();
        foreach (var user in users)
        {
            Debug.WriteLine($"\nUser ID: {user.Id}, Name: {user.Name}\n");
        }

        // 查找用户  
        User? testUser = test_Repo.GetByName("测试用户");
        if (testUser != null && test_Att_Repo != null)
        {
            testUser.Age = 18;
            test_Att_Repo.AddAttendanceRecord(new AttendanceRecord
            {
                UserId = testUser.Id,
                User = testUser,
                CheckTime = DateTime.Now,
                CheckType = CheckType.CheckIn
            });
        }
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
        // 释放资源
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="services"></param>
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

        //注册所有View（需继承Window）
        services.AddTransient<MainWindow>();

        // 注册 INavigationService窗口导航服务
        services.AddSingleton<NavigationService, NavigationService>();

    }
}
