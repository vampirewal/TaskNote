#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：LoadModulesServices
// 创 建 者：杨程
// 创建时间：2021/5/28 10:22:34
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskNote.Core;
using TaskNote.Core.SimpleMVVM;

namespace TaskNote.ViewModel.Common
{
    public class LoadModulesServices:BaseSingleton<LoadModulesServices>
    {

        /// <summary>
        /// 开始读取模块
        /// </summary>
        public void StartLoadModules()
        {
            Messenger.Default.Register(GetInstance(), "LoadModules", LoadModules);
        }

        #region 私有方法
        [ImportMany]
        public Lazy<Core.IView, IMetaData>[] Plugins { get; set; }

        public Dictionary<string, FrameworkElement> ModulesDic = new Dictionary<string, FrameworkElement>();

        public List<FrameworkElementInfo> frameworks { get; set; } = new List<FrameworkElementInfo>();

        private CompositionContainer container = null;

        /// <summary>
        /// 加载模块
        /// </summary>
        public void LoadModules()
        {
            ModulesDic.Clear();
            frameworks.Clear();
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\Pluing");
            if (dir.Exists)
            {
                var catalog = new DirectoryCatalog(dir.FullName, "*.dll");
                container = new CompositionContainer(catalog);
                try
                {
                    container.ComposeParts(this);
                }
                catch (CompositionException compositionEx)
                {
                    Console.WriteLine(compositionEx.ToString());
                }

                foreach (var plugin in Plugins)
                {
                    
                    string pluginName = plugin.Metadata.ModuleName;
                    string pluginFullName = plugin.Metadata.ModuleFullName;
                    int pluginNum = plugin.Metadata.Priority;
                    var framework = (FrameworkElement)plugin.Value.Window;
                    framework.Name = pluginName;
                    ModulesDic.Add(pluginName, framework);
                    
                    //便于后续添加小工具
                    FrameworkElementInfo info = new FrameworkElementInfo()
                    {
                        Priority = plugin.Metadata.Priority,
                        ModuleFullName = plugin.Metadata.ModuleFullName,
                        ModuleName = plugin.Metadata.ModuleName,
                        Description = plugin.Metadata.Description,
                        Author = plugin.Metadata.Author,
                        Version = plugin.Metadata.Version,
                        WindowWidth=framework.Width,
                        WindowHeight=framework.Height,
                        frameworkElement = framework
                    };
                    frameworks.Add(info);
                }

            }
            else
            {
                Directory.CreateDirectory(dir.FullName);
            }
        }

        public FrameworkElement OpenModuleBindingVM(string ModuleName, ViewModelBase vm)
        {
            ModulesDic[ModuleName].DataContext = vm;
            return ModulesDic[ModuleName];
        }

        #endregion

        
    }
    /// <summary>
    /// Module信息类
    /// </summary>
    public class FrameworkElementInfo : IMetaData
    {
        public int Priority { get; set; }

        public string ModuleFullName { get; set; }

        public string ModuleName { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public FrameworkElement frameworkElement { get; set; }

        public double WindowWidth { get; set; }

        public double WindowHeight { get; set; }
    }
}
