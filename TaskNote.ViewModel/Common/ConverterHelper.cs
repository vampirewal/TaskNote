#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：ConverterHelper
// 创 建 者：杨程
// 创建时间：2021/6/3 17:20:39
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskNote.ViewModel.Common
{
    /// <summary>
    /// 转化帮助类
    /// </summary>
    public class ConverterHelper
    {
        public ConverterHelper()
        {
            //构造函数
        }

        /// <summary>
        /// 将object对象转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="asObject">object对象</param>
        /// <returns></returns>
        public static T ConvertObject<T>(object asObject) where T : new()
        {
            //var serializer = new JavaScriptSerializer();
            string strJson = JsonConvert.SerializeObject(asObject);
            //将object对象转换为json字符
            //var json = serializer.Serialize(asObject);
            //将json字符转换为实体对象
            var t = JsonConvert.DeserializeObject<T>(strJson);
            return t;
        }
    }
}
