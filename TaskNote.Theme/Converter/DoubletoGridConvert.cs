#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：DoubletoGridConvert
// 创 建 者：杨程
// 创建时间：2021/5/27 13:59:52
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TaskNote.Theme.Converter
{
    public class DoubletoGridConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var n = (double)value;
            return new GridLength(n);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GridLength length = (GridLength)value;
            if (length != null)
                return length.Value;
            return 0;
        }
    }
}
