#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：LoginViewModel
// 创 建 者：杨程
// 创建时间：2021/5/25 16:50:20
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
using TaskNote.Core.SimpleMVVM;

namespace TaskNote.ViewModel
{
    public class LoginViewModel: ViewModelBase
    {
        public LoginViewModel()
        {
            //构造函数
            Title = "登陆";
        }

        #region 重写
        public override RelayCommand CloseWindowCommand => new RelayCommand(() =>
        {
            System.Environment.Exit(0);
            Application.Current.Shutdown();
        });
        #endregion

        #region 属性

        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令

        #endregion
    }
}
