using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
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
    public static IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        // 添加 DbContext，并指定 SQLite 文件路径
        var dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\AttendanceSystem.db");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        ServiceProvider = services.BuildServiceProvider();

        // 初始化数据库
        AppDbContext.Initialize(ServiceProvider);

        base.OnStartup(e);
    }
}