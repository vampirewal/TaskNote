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
            MessengerRegister();
            GetLoginName();

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

        #endregion

        #region 命令
        public RelayCommand LoginCommand => new RelayCommand(() =>
        {

            //var user= DC.Set<User>().Where(w => w.UserName == CurrentUser.UserName && w.Password == CurrentUser.Password).FirstOrDefault();

            using (TaskNoteDataAccess taskNote = new TaskNoteDataAccess())
            {
                var current = taskNote.Users.Where(w => w.UserName == CurrentUser.UserName && w.Password == CurrentUser.Password).FirstOrDefault();
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
                        taskNote.Users.Update(CurrentUser);
                        taskNote.SaveChanges();
                        GlobalDataManager.GetInstance().LoginUserInfo = CurrentUser;
                        isLogin = true;
                        WindowsManager.CloseWindow(WindowsManager.Windows["LoginWindow"]);
                    }
                    else
                    {
                        MessageBox.Show("当前用户已登陆，请勿重复登陆！");
                    }
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

        #region 消息
        /// <summary>
        /// 消息注册
        /// </summary>
        private void MessengerRegister()
        {
            Messenger.Default.Register(this, "GetLoginName", GetLoginName);
        }
        /// <summary>
        /// 获取登陆的用户名
        /// </summary>
        private void GetLoginName()
        {
            using (TaskNoteDataAccess work = new TaskNoteDataAccess())
            {
                HaveLoginUserName.Clear();
                var current = work.Users.Where(w => w.ID != null).ToList();
                for (int i = 0; i < current.Count; i++)
                {
                    HaveLoginUserName.Add(current[i]);
                }
            }
        }

        
        #endregion
    }
}
