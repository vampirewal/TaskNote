#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：SettingTaskGroupViewModel
// 创 建 者：杨程
// 创建时间：2021/6/4 12:27:15
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
using TaskNote.Core;
using TaskNote.Core.SimpleMVVM;
using TaskNote.DataAccess;
using TaskNote.Model;
using TaskNote.Theme;

namespace TaskNote.ViewModel
{
    public class SettingTaskGroupViewModel : ViewModelBase
    {
        

        public SettingTaskGroupViewModel()
        {
            //构造函数
        }

        public override object GetResult()
        {
            return taskGroups;
        }

        public override void InitData()
        {
            Title = "设置任务组";
            taskGroups = new ObservableCollection<TaskGroup>();
            taskGroup = new TaskGroup()
            {
                CreateTime = DateTime.Now,
                GroupBackgroundColor = "#FF4040",
                GroupName = "",
                GroupSort = 99,
                IsCanDelete = true,
                IsDelete = false,
                taskModelID = SelectTaskModelId
            };


        }

        public override void PassData(object obj)
        {
            fliter _fliter= JsonHelper.ConvertObject<fliter>(obj);
            taskGroups = _fliter.taskGroups;
            SelectTaskModelId = _fliter.TaskModelId;
        }

        #region 属性
        public string SelectTaskModelId { get; set; }

        private TaskGroup _taskGroup;
        public TaskGroup taskGroup { get => _taskGroup; set { _taskGroup = value; DoNotify(); } }

        public ObservableCollection<TaskGroup> taskGroups { get; set; }

        private TaskGroup _SelectTaskGroup;

        public TaskGroup SelectTaskGroup
        {
            get { return _SelectTaskGroup; }
            set { _SelectTaskGroup = value; DoNotify(); }
        }

        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令
        public RelayCommand SelectColorCommand => new RelayCommand(() =>
          {
              string colorStr = WindowsManager.CreateDialogWindowByViewModelResult("SelectColorView", new SelectColorViewModel()).ToString();
              taskGroup.GroupBackgroundColor = colorStr;
          });

        public RelayCommand SaveNewTaskGroupCommand => new RelayCommand(() =>
          {
              taskGroup.GroupSort= taskGroups.Count;
              taskGroup.taskModelID = SelectTaskModelId;
              using (TaskNoteDataAccess task=new TaskNoteDataAccess())
              {
                  task.Add(taskGroup);
                  task.SaveChangesAsync();
              }
              taskGroups.Add(taskGroup);
              GC.Collect();
              WindowsManager.CloseWindow(View as Window);
          });

        public RelayCommand<TaskGroup> DeleteTaskGroupCommand => new RelayCommand<TaskGroup>((t) =>
          {
              if (t!=null)
              {
                  using (TaskNoteDataAccess task = new TaskNoteDataAccess())
                  {
                      task.Remove(t);
                      task.SaveChangesAsync();
                  }

                  DialogWindow.Show("删除成功！", MessageType.Successful, WindowsManager.Windows["SettingTaskGroupWindow"]);
              }
          });

        public RelayCommand<TaskGroup> EditColorCommand => new RelayCommand<TaskGroup>((t) =>
          {
              if (t!=null)
              {
                  string colorStr = WindowsManager.CreateDialogWindowByViewModelResult("SelectColorView", new SelectColorViewModel()).ToString();
                  t.GroupBackgroundColor = colorStr;
              }
          });
        #endregion

        public class fliter
        {
            public string TaskModelId { get; set; }

            public ObservableCollection<TaskGroup> taskGroups { get; set; }
        }
    }

    
}
