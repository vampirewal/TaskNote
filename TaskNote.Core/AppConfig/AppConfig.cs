#region<<文 件 说 明>>
/*----------------------------------------------------------------
// 文件名称：AppConfig
// 创 建 者：杨程
// 创建时间：2021/3/4 星期四 15:59:25
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
	/// <summary>
	/// 对应config.json文件
	/// </summary>
	public class AppConfig
	{
		#region 配置文件中的配置属性

		/// <summary>
		/// 应用名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 是否记住我
		/// </summary>
		public bool IsRemberMe { get; set; }

		/// <summary>
		/// 是否自动登录
		/// </summary>
		public bool IsAutoLogin { get; set; }

		/// <summary>
		/// 上次登录账号
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// 上次登录密码
		/// </summary>
		public string Password { get; set; }

        /// <summary>
        /// 是否已启动
        /// </summary>
        public bool isStart { get; set; }

        public List<string> HasModules { get; set; }

        #endregion

        /// <summary>
        /// 运行配置，分为开发环境、生产环境
        /// </summary>
        //public ConfigDesc RunningConfig { get; set; }

        private static readonly object lockObj = new object();

		private static AppConfig _Instance = null;
		/// <summary>
		/// 配置单例
		/// </summary>
		public static AppConfig Instance
		{
			get
			{
				if (_Instance != null)
				{
					return _Instance;
				}
				lock (lockObj)
				{
					var configFile = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";
					if (File.Exists(configFile))
					{
						var configContent = File.ReadAllText(configFile);
						_Instance = JsonConvert.DeserializeObject<AppConfig>(configContent);
					}
					else
					{
						_Instance = new AppConfig
						{
							Name = "业务系统",
							UserName = "",
							Password = "",
							IsRemberMe = false,
							IsAutoLogin = false,
							isStart=false,
							HasModules=new List<string>() { "hahaha","aaaaaaa"}
						};
						var json = JsonConvert.SerializeObject(_Instance);
						var formatStr = JsonHelper.FormatJsonString(json);
						FileStream fs = File.Create(configFile);
						fs.Dispose();
						File.AppendAllText(configFile, formatStr);

						var configContent = File.ReadAllText(configFile);
						_Instance = JsonConvert.DeserializeObject<AppConfig>(configContent);
					}


					//var configDescFile = $"{AppDomain.CurrentDomain.BaseDirectory}config.{_Instance.ConfigType}.json";
					//var configDescContent = File.ReadAllText(configDescFile);
					//_Instance.RunningConfig = JsonConvert.DeserializeObject<ConfigDesc>(configDescContent);
				}
				return _Instance;
			}
		}


		/// <summary>
		/// 保存配置文件
		/// </summary>
		public void Save()
		{
			var jsonStr = JsonConvert.SerializeObject(_Instance);
			var formatStr = JsonHelper.FormatJsonString(jsonStr);
			var configFile = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";
			if (File.Exists(configFile))
			{
				File.Delete(configFile);
			}
			File.AppendAllText(configFile, formatStr);
		}

		private AppConfig() { }
	}

	/// <summary>
	/// 配置详细信息
	/// </summary>
	public class ConfigDesc
	{
		/// <summary>
		/// api地址
		/// </summary>
		public string API { get; set; }
	}
}