#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：ObjectConvert
// 创 建 者：杨程
// 创建时间：2021/6/3 21:07:15
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

namespace TaskNote.Theme.Converter
{
    /// <summary>
    /// CommandParameter 多参数传递
    /// </summary>
    public class ObjectConvert : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public static object ConverterObject;

        public object Convert(object[] values, Type targetType,
          object parameter, System.Globalization.CultureInfo culture)
        {
            ConverterObject = values;
            string str = values.GetType().ToString();
            return values.ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
          object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
