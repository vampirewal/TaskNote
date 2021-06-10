#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EnumValueConverter
// 创 建 者：杨程
// 创建时间：2021/6/9 14:39:42
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TaskNote.Core;

namespace TaskNote.Theme.Converter
{
    [ValueConversion(typeof(Enum), typeof(String))]
    public class EnumValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is Enum)
            {
                return EnumHelper.GetDescription((Enum)value);
            }
            return string.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
