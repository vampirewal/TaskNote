#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：ModulesViewModel
// 创 建 者：杨程
// 创建时间：2021/6/4 18:51:29
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
using System.Windows;
using TaskNote.Core;
using TaskNote.Core.SimpleMVVM;
using TaskNote.ViewModel.Common;

namespace TaskNote.ViewModel.ChildViewModel.Modules
{
    public class ModulesViewModel : ViewModelBase
    {
        

        public ModulesViewModel()
        {
            //构造函数
        }

        public override void PassData(object obj)
        {
            //FrameworkElementInfo frameworkElementInfo = JsonHelper.ConvertObject<FrameworkElementInfo>(obj);
            FrameworkElementInfo frameworkElementInfo = (FrameworkElementInfo)(obj);
            Title = frameworkElementInfo.ModuleName;
            ShowFrameworkElement = frameworkElementInfo.frameworkElement;
            WindowWidth = frameworkElementInfo.WindowWidth;
            WindowHeight = frameworkElementInfo.WindowHeight+31;
        }
        #region 属性
        private FrameworkElement showFrameworkElement;
        public FrameworkElement ShowFrameworkElement { get => showFrameworkElement; set { showFrameworkElement = value; DoNotify(); } }

        private double _WindowWidth;
        public double WindowWidth { get=>_WindowWidth; set { _WindowWidth = value;DoNotify(); } }

        private double _WindowHeight;
        public double WindowHeight { get=>_WindowHeight; set { _WindowHeight = value;DoNotify(); } }

        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令

        #endregion
    }
}
