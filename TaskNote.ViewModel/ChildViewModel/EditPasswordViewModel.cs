#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EditPasswordViewModel
// 创 建 者：杨程
// 创建时间：2021/6/7 16:44:52
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
using TaskNote.DataAccess;
using TaskNote.Model;
using TaskNote.Theme;

namespace TaskNote.ViewModel
{
    public class EditPasswordViewModel : ViewModelBase
    {
        private User loginUser1;

        public EditPasswordViewModel()
        {
            //构造函数
        }

        public override object GetResult()
        {
            return IsSuccess;
        }

        public override void InitData()
        {
            Title = "修改密码";
        }

        public override void PassData(object obj)
        {
            loginUser = (User)obj;
            OldPassword = loginUser.Password;
        }

        

        #region 属性
        private User _loginUser;
        public User loginUser { get => _loginUser; set { _loginUser = value; DoNotify(); } }

        public string OldPassword { get; set; }

        public bool IsSuccess { get; set; } = false;
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令
        public RelayCommand SaveEditCommand => new RelayCommand(() =>
          {
              if (loginUser.Password== OldPassword)
              {
                  DialogWindow.Show("密码无修改！", MessageType.Information, WindowsManager.Windows["MainWindow"]);
              }
              else
              {
                  loginUser.IsRemember = false;
                  IsSuccess = true;
                  SqlHelper.Update(loginUser);
                  DialogWindow.Show("密码修改成功！将会关闭程序请重新登陆！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
                  WindowsManager.CloseWindow(View as Window);
              }
          });
        #endregion
    }
}
