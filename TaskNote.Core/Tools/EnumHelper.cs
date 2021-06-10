#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EnumHelper
// 创 建 者：杨程
// 创建时间：2021/6/9 14:40:40
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaskNote.Core
{
    public static class EnumHelper
    {
        public static String GetDescription(this Enum value)
        {
            var type = value.GetType();//先获取这个枚举的类型
            var field = type.GetField(value.ToString());//通过这个类型获取到值
            var obj = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性
            return obj.Name ?? "";
        }
    }
}
