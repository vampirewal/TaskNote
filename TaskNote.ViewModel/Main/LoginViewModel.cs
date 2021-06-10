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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskNote.Core.SimpleMVVM;
using TaskNote.DataAccess;
using TaskNote.Model;

namespace TaskNote.ViewModel
{
    public class LoginViewModel : ViewModelBase
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

        private bool isLogin = false;
        

        public override object GetResult()
        {
            return isLogin;
        }

        public override void InitData()
        {
            HaveLoginUserName = new ObservableCollection<User>();
            CurrentUser = new User();
            
            GetLoginName();

        }

        public override void MessengerRegister()
        {
            Messenger.Default.Register(this, "GetLoginName", GetLoginName);
        }
        #endregion

        #region 属性
        private User currentUser;
        public User CurrentUser { get => currentUser; set { currentUser = value; DoNotify(); } }

        public ObservableCollection<User> HaveLoginUserName { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法
        /// <summary>
        /// 获取登陆的用户名
        /// </summary>
        private void GetLoginName()
        {
            HaveLoginUserName.Clear();
            var current = SqlHelper.GetInfoLst<User>(null).ToList();
            for (int i = 0; i < current.Count; i++)
            {
                HaveLoginUserName.Add(current[i]);
            }
        }
        #endregion

        #region 命令
        public RelayCommand LoginCommand => new RelayCommand(() =>
        {

            //var user= DC.Set<User>().Where(w => w.UserName == CurrentUser.UserName && w.Password == CurrentUser.Password).FirstOrDefault();
            var current = SqlHelper.GetOne<User>(w => w.UserName == CurrentUser.UserName && w.Password == CurrentUser.Password);
            var ViewDataUserModel = CurrentUser;
            if (current != null)
            {
                if (!current.IsLogin)
                {
                    CurrentUser = current;
                    if (ViewDataUserModel.IsRemember)
                    {
                        CurrentUser.IsRemember = true;
                    }
                    CurrentUser.IsLogin = true;
                    SqlHelper.Update(CurrentUser);
                    GlobalDataManager.GetInstance().LoginUserInfo = CurrentUser;
                    isLogin = true;
                    WindowsManager.CloseWindow(WindowsManager.Windows["LoginWindow"]);
                }
                else
                {
                    MessageBox.Show("当前用户已登陆，请勿重复登陆！");
                }
            }
        });

        public RelayCommand<string> SelectUserNameCommand => new RelayCommand<string>((u) =>
        {
            var current = HaveLoginUserName.FirstOrDefault(f => f.ID == u);
            if (current != null)
            {
                if (current.IsRemember)
                {
                    CurrentUser = current;
                }
                else
                {
                    CurrentUser.ClearInfo();
                    CurrentUser.UserName = current.UserName;
                }
            }
        });

        public RelayCommand RegisterCommand => new RelayCommand(() =>
        {
            //Messenger.Default.Send("CreateRegisterView");
            if (Convert.ToBoolean(WindowsManager.CreateDialogWindowByViewModelResult("RegisterView", new RegisterViewModel())))
            {
                GetLoginName();
            }

        });
        #endregion

        
    }
}
