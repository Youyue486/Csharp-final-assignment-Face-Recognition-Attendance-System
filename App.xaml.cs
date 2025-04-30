using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Presentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        // 初始化数据库
        AppDbContext.Initialize(ServiceProvider);

        //显示主窗口
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

        // 注册窗口
        services.AddTransient<MainWindow>();
    }
}
