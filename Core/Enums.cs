using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Core
{
    #region 用户状态枚举
    /// <summary>
    /// 用户状态类型（支持叠加，但BusinessTrip和Vacation需独占）
    /// </summary>
    [Flags]
    public enum UserStatusType
    {
        [Description("在班")]
        在班=1,          // 正常在岗状态，可与Late/OverTime叠加

        [Description("下班")]
        下班=2,         // 独占状态（不可与其他状态共存）

        [Description("加班")]
        加班=4,        // 可与OnDuty/Overtime叠加

        [Description("出差")]
        出差=8,    // 独占状态（不可与其他状态共存）

        [Description("旷班")]
        旷班=16,          // 独占状态（不可与其他状态共存）

        [Description("迟到")]
        迟到=32,            // 可与OnDuty/Overtime叠加

        [Description("早退")]
        早退=64,      // 独占状态（不可与其他状态共存）

        [Description("请假")]
        请假=128        // 独占状态（不可与其他状态共存）
    }
    #endregion

    #region 打卡记录类型枚举
    /// <summary>
    /// 打卡记录类型
    /// </summary>
    /// <para name="CheckType">CheckIn: 上班打卡, CheckOut: 下班打卡</para>
    /// <returns></returns>
    public enum CheckType
    {
        [Description("上班打卡")]
        CheckIn,

        [Description("上班补卡")]
        CheckInSupplement,

        [Description("下班打卡")]
        CheckOut
    }
    #endregion

    #region 用户请求类型枚举
    /// <summary>
    /// 用户请求类型
    /// </summary>
    public enum RequestType
    {
        [Description("休假申请")]
        Vacation,

        [Description("出差申请")]
        BusinessTrip,

        //TODO: 其他用户请求类型
        //[Description("调班申请")]
        //ShiftChange
    }
    #endregion

    #region 用户请求审批状态枚举
    /// <summary>
    /// 请求审批状态
    /// </summary>
    public enum RequestStatus
    {
        [Description("待审批")]
        Pending,

        [Description("已批准")]
        Approved,

        [Description("已拒绝")]
        Rejected
    }
    #endregion

    #region 用户角色类型枚举
    /// <summary>
    /// 用户角色类型
    /// </summary>
    public enum UserRole
    {
        [Description("普通用户")]
        Normal,

        [Description("管理员")]
        Admin
    }
    #endregion

    #region 工作日枚举
    /// <summary>
    /// 工作日枚举（用于GroupSchedule的WorkDays按位存储）
    /// </summary>
    [Flags]
    public enum WorkDay
    {
        None = 0,
        Monday = 1 << 0,    // 1
        Tuesday = 1 << 1,   // 2
        Wednesday = 1 << 2, // 4
        Thursday = 1 << 3,  // 8
        Friday = 1 << 4,    // 16
        Saturday = 1 << 5,  // 32
        Sunday = 1 << 6     // 64
    }
    #endregion
}
