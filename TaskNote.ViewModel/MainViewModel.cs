#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：MainViewModel
// 创 建 者：杨程
// 创建时间：2021/5/26 11:05:01
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using TaskNote.Core.SimpleMVVM;
using TaskNote.DataAccess;
using TaskNote.Model;

namespace TaskNote.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            //构造函数
            Title = "TaskNote";
        }

        #region 重写

        public override RelayCommand CloseWindowCommand => new RelayCommand(() =>
        {
            //正常关闭程序时，需反写数据库内的数据，将用户的登陆状态改为未登录
            using (TaskNoteDataAccess taskNote = new TaskNoteDataAccess())
            {
                LoginUserInfo.IsLogin = false;
                taskNote.Users.Update(LoginUserInfo);
                taskNote.SaveChanges();
            }

            System.Environment.Exit(0);
            Application.Current.Shutdown();
        });

        public override void InitData()
        {
            LoginUserInfo = GlobalDataManager.GetInstance().LoginUserInfo;
            IsHideBtnRightMenu = false;
        }
        #endregion
        public bool IsLeftPopupOpen { get; set; } = false;

        private bool _IsHideBtnRightMenu;
        public bool IsHideBtnRightMenu { get => _IsHideBtnRightMenu; set { _IsHideBtnRightMenu = value;DoNotify(); } } 
        #region 属性

        #region 窗体显示属性

        public double MaxHeight
        {
            get
            {
                return SystemParameters.MaximizedPrimaryScreenHeight;
            }
        }

        public double MaxWidth
        {
            get
            {
                return SystemParameters.MaximizedPrimaryScreenWidth;
            }
        }

        #endregion 窗体显示属性

        public User LoginUserInfo { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令
        public RelayCommand<Popup> OpenLeftPopupMenuCommand => new RelayCommand<Popup>((p) =>
        {
            p.IsOpen = !IsLeftPopupOpen;
        });

        public RelayCommand ShowBtnLeftMenuCommand => new RelayCommand(() =>
        {
            Task.Run(() =>
            {
                if (!IsHideBtnRightMenu)
                {
                    Thread.Sleep(1000);
                    IsHideBtnRightMenu = !IsHideBtnRightMenu;
                }
                else
                {
                    IsHideBtnRightMenu = !IsHideBtnRightMenu;
                }
                
            });
            
        });
        #endregion
    }
}
