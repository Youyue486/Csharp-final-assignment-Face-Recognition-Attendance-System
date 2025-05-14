using Csharp_final_assignment_Face_Recognition_Attendance_System.Core;
using Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Tools
{
    public class FlagBitMaskConverter : IValueConverter
    {
        // 转换：WorkDay（位掩码） → CheckBox.IsChecked（是否包含某一天）
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedDays = (WorkDay)value;    // 当前选中的工作日（如 周一|周二）
            var currentDay = (WorkDay)parameter;  // 当前 CheckBox 对应的工作日（如 周一）
            return (selectedDays & currentDay) == currentDay; // 判断是否包含该天
        }

        // 反向转换：CheckBox.IsChecked → 更新 WorkDay 位掩码
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;          // CheckBox 是否被选中
            var currentDay = (WorkDay)parameter;  // 当前 CheckBox 对应的工作日

            // 修复 CS8602 和 CS8605 错误
            var selectedWorkDaysProperty = targetType.GetProperty("SelectedWorkDays");
            if (selectedWorkDaysProperty == null)
            {
                throw new InvalidOperationException("SelectedWorkDays 属性未找到。");
            }

            var originalDaysValue = selectedWorkDaysProperty.GetValue(value);
            if (originalDaysValue == null)
            {
                throw new InvalidOperationException("SelectedWorkDays 属性值为 null。");
            }

            var originalDays = (WorkDay)originalDaysValue;

            if (isChecked)
                return originalDays | currentDay;  // 选中：添加该天到掩码
            else
                return originalDays & ~currentDay; // 取消选中：从掩码中移除该天
        }
    }
}
