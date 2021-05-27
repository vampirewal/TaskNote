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
            TaskList = new ObservableCollection<TaskModel>();
            IsHideBtnRightMenu = false;
        }
        #endregion


        #region 属性
        public bool IsLeftPopupOpen { get; set; } = false;

        private bool _IsHideBtnRightMenu;
        public bool IsHideBtnRightMenu { get => _IsHideBtnRightMenu; set { _IsHideBtnRightMenu = value; DoNotify(); } }

        public RowDefinition BottomHeight { get; set; }

        private TaskModel _SelectTaskModel;
        /// <summary>
        /// 选中的Task
        /// </summary>
        public TaskModel SelectTaskModel
        {
            get { return _SelectTaskModel; }
            set { _SelectTaskModel = value; DoNotify(); }
        }


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

        public ObservableCollection<TaskModel> TaskList { get; set; }

        
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法
        /// <summary>
        /// 获取选择的任务获取明细和附件
        /// </summary>
        /// <param name="task"></param>
        private void GetSelectTaskDtlAndAttachment(TaskModel task)
        {
            SelectTaskModel.Detail.Clear();
            SelectTaskModel.fileAttachmentModels.Clear();

            using (TaskNoteDataAccess tn = new TaskNoteDataAccess())
            {
                var current = tn.TaskDtl.Where(w => w.taskModelID == task.ID).ToList();

                var currentFileAttachments = tn.fileAttachments.Where(w => w.ParentGuidId == task.ID).ToList();

                foreach (var item in current)
                {
                    item.taskModel = SelectTaskModel;
                    SelectTaskModel.Detail.Add(item);
                }

                foreach (var item in currentFileAttachments)
                {
                    SelectTaskModel.fileAttachmentModels.Add(item);
                }
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
                    Thread.Sleep(1000);
                    IsHideBtnRightMenu = !IsHideBtnRightMenu;
                }
                else
                {
                    IsHideBtnRightMenu = !IsHideBtnRightMenu;
                }
                
            });
            
        });

        public RelayCommand<Border> test => new RelayCommand<Border>((b) =>
          {
              DoubleAnimation da = new DoubleAnimation()
              {
                  To = 0,
                  Duration = new Duration(TimeSpan.FromSeconds(1))
              };
              b.BeginAnimation(Border.WidthProperty, da);
          });

        public RelayCommand CreateTaskCommand => new RelayCommand(() =>
          {
              TaskModel tm = new TaskModel()
              {
                  TaskName="new",
                  StartTime=DateTime.Now,
                  EndTime=DateTime.Now
              };

              TaskList.Add(tm);
          });

        public RelayCommand<TaskModel> EditTaskCommand => new RelayCommand<TaskModel>((t) =>
          {

          });


        public bool IsCollapsed { get; set; } = true;
        public RelayCommand<TaskModel> LookTaskInfoCommand => new RelayCommand<TaskModel>((t) =>
          {
              if (t!=null)
              {
                  //给下方的赋值
                  SelectTaskModel = t;
                  GetSelectTaskDtlAndAttachment(t);

                  if (IsCollapsed)
                  {
                      //控制界面上的高度
                      GridLengthAnimation grid = new GridLengthAnimation()
                      {
                          From = new GridLength(0, GridUnitType.Pixel),
                          To = new GridLength(500, GridUnitType.Pixel),
                          Duration = new Duration(TimeSpan.FromSeconds(0.3))
                      };
                      Storyboard sb = new Storyboard();

                      BottomHeight.BeginAnimation(RowDefinition.HeightProperty, grid);
                      IsCollapsed = !IsCollapsed;
                  }

                  GC.Collect();
                  
              }
          });

        public RelayCommand CloseBottomInfoCommand => new RelayCommand(() =>
          {
              if (!IsCollapsed)
              {
                  GridLengthAnimation grid = new GridLengthAnimation()
                  {
                      From = new GridLength(500, GridUnitType.Pixel),
                      To = new GridLength(0, GridUnitType.Pixel),
                      Duration = new Duration(TimeSpan.FromSeconds(0.2))
                  };
                  BottomHeight.BeginAnimation(RowDefinition.HeightProperty, grid);
                  IsCollapsed = !IsCollapsed;

                  GC.Collect();
              }
          });


        public RelayCommand<RowDefinition> WindowLoadCommand => new RelayCommand<RowDefinition>((o) =>
          {
              BottomHeight = o;
          });

        public RelayCommand UpLoadAttachmentCommand => new RelayCommand(() =>
          {

          });
        #endregion
    }
}
