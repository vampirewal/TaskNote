#region<<文 件 说 明>>
/*----------------------------------------------------------------
// 文件名称：JsonHelper
// 创 建 者：杨程
// 创建时间：2021/3/4 星期四 16:04:44
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
using System.IO;
using System.Text;

namespace TaskNote.Core
{
	public class JsonHelper
	{
		/// <summary>
		/// json字符串格式化输出
		/// </summary>
		/// <param name="sourceJsonStr"></param>
		/// <returns></returns>
		public static string FormatJsonString(string sourceJsonStr)
		{
			//格式化json字符串
			JsonSerializer serializer = new JsonSerializer();
			TextReader tr = new StringReader(sourceJsonStr);
			JsonTextReader jtr = new JsonTextReader(tr);
			object obj = serializer.Deserialize(jtr);
			if (obj != null)
			{
				StringWriter textWriter = new StringWriter();
				JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
				{
					Formatting = Formatting.Indented,
					Indentation = 4,
					IndentChar = ' '
				};
				serializer.Serialize(jsonWriter, obj);
				return textWriter.ToString();
			}
			else
			{
				return sourceJsonStr;
			}
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