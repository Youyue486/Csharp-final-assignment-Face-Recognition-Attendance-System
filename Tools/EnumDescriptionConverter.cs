using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Tools
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // 检查 value 是否为 null  
            if (value == null)
            {
                // 返回空字符串或其他默认值，而不是抛出异常  
                return string.Empty;
            }

            // 检查 value 是否为枚举类型  
            if (!value.GetType().IsEnum)
            {
                // 记录日志并返回原始值的字符串表示  
                Debug.WriteLine($"警告: EnumDescriptionConverter 接收到非枚举类型: {value.GetType()}");
                return value.ToString() ?? string.Empty; // 修复 CS8603  
            }

            // 获取枚举类型  
            var enumType = value.GetType();

            // 获取枚举值的名称  
            string valueOfString = value.ToString() ?? string.Empty; // 修复 CS8603  

            // 获取枚举字段信息  
            var fieldInfo = enumType.GetField(valueOfString);

            // 检查字段是否存在  
            if (fieldInfo == null)
            {
                Debug.WriteLine($"警告: 找不到枚举成员: {enumType.Name}.{valueOfString}");
                return valueOfString;
            }

            // 获取 Description 属性  
            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // 返回 Description 或枚举名称  
            return attributes.Length > 0
                ? ((DescriptionAttribute)attributes[0]).Description
                : valueOfString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
