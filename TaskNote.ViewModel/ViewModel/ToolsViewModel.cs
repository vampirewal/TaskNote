#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：ToolsViewModel
// 创 建 者：杨程
// 创建时间：2021/6/4 16:33:43
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
using TaskNote.Core.SimpleMVVM;
using TaskNote.ViewModel.Common;

namespace TaskNote.ViewModel
{
    public class ToolsViewModel:ViewModelBase
    {
        public ToolsViewModel()
        {
            //构造函数
        }

        public override void InitData()
        {
            LoadModuels();
        }


        #region 属性

        public List<FrameworkElementInfo> elementInfos { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法
        /// <summary>
        /// 获取Moduels
        /// </summary>
        private void LoadModuels()
        {
            LoadModulesServices.GetInstance().LoadModules();
            elementInfos = LoadModulesServices.GetInstance().frameworks;
        }
        #endregion

        #region 命令
        public RelayCommand RefreshModules => new RelayCommand(() =>
          {
              //Messenger.Default.Send("LoadModules");
              LoadModuels();
          });

        public RelayCommand<FrameworkElementInfo> OpenModulesCommand => new RelayCommand<FrameworkElementInfo>((f) =>
          {
              if (f != null)
              {
                  //创建窗体打开

              }
          });
        #endregion
    }
}
