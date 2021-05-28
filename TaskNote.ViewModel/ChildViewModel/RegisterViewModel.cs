#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：RegisterViewModel
// 创 建 者：杨程
// 创建时间：2021/5/26 18:11:16
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
    public class RegisterViewModel:ViewModelBase
    {
        public RegisterViewModel()
        {
            //构造函数
            Title = "TaskNote 注册";

            NewUser = new User()
            {
                IsLogin = false,
                IsRemember = false,
                IsDelete = false,
                
            };
        }

        public override void InitData()
        {
            
        }

        private bool isRegister = false;
        public override object GetResult()
        {
            return isRegister;
        }
        #region 属性
        public User NewUser { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令
        public RelayCommand RegisterUserCommand => new RelayCommand(() =>
          {
              if (string.IsNullOrEmpty( NewUser.UserName)||string.IsNullOrEmpty(NewUser.Password))
              {
                  DialogWindow.Show("用户名或密码必填！", MessageType.Error, WindowsManager.Windows["RegisterWindow"]);
                  return;
              }

              bool isok = false;
              Window w = View as Window;
              using (TaskNoteDataAccess work = new TaskNoteDataAccess())
              {
                  var OldUser = work.Users.FirstOrDefault(f => f.UserName == NewUser.UserName);
                  if (OldUser == null)
                  {
                      NewUser.CreateTime = DateTime.Now;
                      work.Users.Add(NewUser);
                      work.SaveChanges();
                      isok = true;
                      isRegister = true;
                  }
                  else
                  {
                      DialogWindow.Show("已存在相同的用户名！", MessageType.Error, WindowsManager.Windows["RegisterWindow"]);
                  }
              }
              if (isok)
              {
                  w.DialogResult = true;
                  WindowsManager.CloseWindow(w);
              }
          });
        #endregion
    }
}
