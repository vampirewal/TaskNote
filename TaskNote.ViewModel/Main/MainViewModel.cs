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
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using TaskNote.Core.SimpleMVVM;
using TaskNote.DataAccess;
using TaskNote.Model;
using TaskNote.Theme;
using TaskNote.ViewModel.Common;

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
            LoginUserInfo.IsLogin = false;
            SqlHelper.Update(LoginUserInfo);

            System.Environment.Exit(0);
            Application.Current.Shutdown();
        });

        public override void InitData()
        {
            LoginUserInfo = GlobalDataManager.GetInstance().LoginUserInfo;
            FocusTask = new ObservableCollection<TaskModel>();
            FocusNote = new ObservableCollection<NoteModel>();

            IsHideBtnRightMenu = false;

            //TaskView = LoadModulesServices.GetInstance().OpenModuleBindingVM("TaskView", new TaskViewModel());
            
            GetView();
            GetData();
        }

        public override void MessengerRegister()
        {
            Messenger.Default.Register<TaskModel>(this, "AddFocusTask", AddFocusTask);
            Messenger.Default.Register<TaskModel>(this, "RemoveFocusTask", RemoveFocusTask);

            Messenger.Default.Register<NoteModel>(this, "AddNoteList", AddNoteList);
            Messenger.Default.Register<NoteModel>(this, "RemoveNoteList", RemoveNoteList);

        }
        #endregion

        #region 属性
        public bool IsLeftPopupOpen { get; set; } = false;

        private bool _IsHideBtnRightMenu;
        public bool IsHideBtnRightMenu { get => _IsHideBtnRightMenu; set { _IsHideBtnRightMenu = value; DoNotify(); } }

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



        #region 窗体中TabControl显示
        private FrameworkElement _TaskView;

        public FrameworkElement TaskView
        {
            get { return _TaskView; }
            set { _TaskView = value; DoNotify(); }
        }

        private FrameworkElement _NoteView;

        public FrameworkElement NoteView
        {
            get { return _NoteView; }
            set { _NoteView = value; DoNotify(); }
        }

        private FrameworkElement _ToolsView;
        public FrameworkElement ToolsView
        {
            get { return _ToolsView; }
            set { _ToolsView = value; DoNotify(); }
        }
        #endregion

        /// <summary>
        /// 点击关注了的任务列表
        /// </summary>
        public ObservableCollection<TaskModel> FocusTask { get; set; }
        /// <summary>
        /// 点击关注了的笔记
        /// </summary>
        public ObservableCollection<NoteModel> FocusNote { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void GetData()
        {
            var focusTask = SqlHelper.GetInfoLst<TaskModel>(w => w.IsImportant == true).ToList();
            if (focusTask.Count > 0)
            {
                foreach (var item in focusTask)
                {
                    FocusTask.Add(item);
                }
            }

            var focusNote = SqlHelper.GetInfoLst<NoteModel>(w => w.IsFocus == true).ToList();
            if (focusNote.Count>0)
            {
                foreach (var item in focusNote)
                {
                    FocusNote.Add(item);
                }
            }
           
        }

        /// <summary>
        /// 反射获取窗体
        /// </summary>
        private void GetView()
        {
            TaskView = Messenger.Default.Send<FrameworkElement>("GetView", "TabView.TaskView");
            TaskView.DataContext = new TaskViewModel();

            NoteView= Messenger.Default.Send<FrameworkElement>("GetView", "TabView.NoteView");
            NoteView.DataContext = new NoteViewModel();

            ToolsView= Messenger.Default.Send<FrameworkElement>("GetView", "TabView.ToolsView");
            ToolsView.DataContext = new ToolsViewModel();
        }

        private void AddFocusTask(TaskModel taskModel)
        {
            if (!FocusTask.Where(w => w.ID == taskModel.ID).Any())
            {
                FocusTask.Add(taskModel);
            }
        }

        private void RemoveFocusTask(TaskModel taskModel)
        {
            var current = FocusTask.Where(w => w.ID == taskModel.ID).FirstOrDefault();
            if (current != null)
            {
                FocusTask.Remove(current);
            }
        }

        private void AddNoteList(NoteModel entity)
        {
            if (!FocusNote.Where(w=>w.ID==entity.ID).Any())
            {
                FocusNote.Add(entity);
            }
            
        }

        private void RemoveNoteList(NoteModel entity)
        {
            var current = FocusNote.Where(w => w.ID == entity.ID).FirstOrDefault();
            if (current!=null)
            {
                FocusNote.Remove(current);
            }
            
        }
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
                    Thread.Sleep(500);
                    IsHideBtnRightMenu = !IsHideBtnRightMenu;
                }
                else
                {
                    IsHideBtnRightMenu = !IsHideBtnRightMenu;
                }
                
            });
            
        });

        

        /// <summary>
        /// 创建新的任务命令
        /// </summary>
        public RelayCommand CreateTaskCommand => new RelayCommand(() =>
          {
              Messenger.Default.Send("CreateNewTask");
          });


        public RelayCommand OpenEditPasswordCommand => new RelayCommand(() =>
          {
              if((bool)WindowsManager.CreateDialogWindowByViewModelResult("EditPasswordView", new EditPasswordViewModel(),LoginUserInfo))
              {
                  LoginUserInfo.IsLogin = false;
                  SqlHelper.Update(LoginUserInfo);

                  System.Environment.Exit(0);
                  Application.Current.Shutdown();
              }
          });
        /// <summary>
        /// 打开回收站
        /// </summary>
        public RelayCommand OpenRecycleCommand => new RelayCommand(() =>
          {
              WindowsManager.CreateDialogWindowByViewModelResult("RecycleBinView", new RecycleBinViewModel());
          });

        public RelayCommand<TaskModel> LookTaskInfoCommand => new RelayCommand<TaskModel>((t) =>
          {
              if (t!=null)
              {
                  Messenger.Default.Send("OpenBottomPanel", t);
              }
              
          });
        #endregion

        #region 消息
        
        


        #endregion
    }
}
