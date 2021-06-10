#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：RecycleBinViewModel
// 创 建 者：杨程
// 创建时间：2021/6/9 9:47:03
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
using TaskNote.Core.SimpleMVVM;
using TaskNote.DataAccess;
using TaskNote.Model;
using TaskNote.Theme;

namespace TaskNote.ViewModel
{
    public class RecycleBinViewModel:ViewModelBase
    {
        public RecycleBinViewModel()
        {
            //构造函数

        }

        public override void InitData()
        {
            Title = "回收站";
            RecycleList = new ObservableCollection<RecycleModel>();
            GetData();
        }

        
        #region 属性
        public ObservableCollection<RecycleModel> RecycleList { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法
        private void GetData()
        {
            RecycleList.Clear();
            var current = SqlHelper.GetInfoLst<RecycleModel>(null).ToList();
            current.Sort((left, right) =>
            {
                if (left.CreateTime>right.CreateTime)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
            foreach (var item in current)
            {
                RecycleList.Add(item);
            }
        }
        
        /// <summary>
        /// 删除Task
        /// </summary>
        /// <param name="entity"></param>
        private void DeleteTask(RecycleModel entity)
        {
            //1、先获取源单的TaskModel
            TaskModel tm = SqlHelper.GetOne<TaskModel>(w => w.ID == entity.SourceID);
            //2、删除
            if (tm!=null)
            {
                //SqlHelper.Delete<TaskModel>(w => w.ID == tm.ID);
                SqlHelper.Delete(tm);
            }

        }
        #endregion

        #region 命令
        /// <summary>
        /// 删除选中项
        /// </summary>
        public RelayCommand DeleteSelectItemsCommand => new RelayCommand(() =>
          {
              var selectItems = RecycleList.Where(w => w.IsSelected == true).ToList();
              if (selectItems!=null&& selectItems.Count>0)
              {
                  
                  foreach (var item in selectItems)
                  {
                      switch (item.sourceType)
                      {
                          case SourceType.Task:
                              var taskmodel = SqlHelper.GetOne<TaskModel>(w => w.ID == item.SourceID);
                              SqlHelper.Delete(taskmodel);
                              break;
                          case SourceType.TaskDtl:
                              break;
                          case SourceType.TaskGroup:
                              break;
                          case SourceType.Note:
                              var notemodel = SqlHelper.GetOne<NoteModel>(w => w.ID == item.SourceID);
                              SqlHelper.Delete(notemodel);
                              break;
                          case SourceType.Attachment:
                              break;
                          default:
                              break;
                      }
                      SqlHelper.Delete(item);
                      //DeleteTask(item);
                      RecycleList.Remove(item);
                  }
              }
          });

        /// <summary>
        /// 清空回收站
        /// </summary>
        public RelayCommand ClearRecycleCommand => new RelayCommand(() =>
          {
              if (DialogWindow.ShowDialog("是否清空回收站！该操作将无法撤销！导致数据丢失！", "请确认"))
              {
                  foreach (var item in RecycleList.ToList())
                  {
                      switch (item.sourceType)
                      {
                          case SourceType.Task:
                              var currentTask = SqlHelper.GetOne<TaskModel>(w => w.ID == item.SourceID);
                              var taskdtls = SqlHelper.GetInfoLst<TaskDtlModel>(w => w.taskModelID == item.SourceID).ToList();
                              var taskgroups = SqlHelper.GetInfoLst<TaskGroup>(w => w.taskModelID == item.SourceID).ToList();
                              List<FileAttachmentModel> fileAttachmentModels = new List<FileAttachmentModel>();
                              if (taskdtls!=null&&taskdtls.Count>0)
                              {
                                  foreach (var taskdtl in taskdtls)
                                  {
                                      fileAttachmentModels.AddRange(SqlHelper.GetInfoLst<FileAttachmentModel>(w => w.taskModelID == taskdtl.ID).ToList());
                                      SqlHelper.Delete(taskdtl);
                                  }
                              }
                              if (taskgroups!=null&& taskgroups.Count>0)
                              {
                                  foreach (var taskgroup in taskgroups)
                                  {
                                      SqlHelper.Delete(taskgroup);
                                  }
                              }
                              if (fileAttachmentModels.Count>0)
                              {
                                  foreach (var file in fileAttachmentModels)
                                  {
                                      SqlHelper.Delete(file);
                                  }
                              }
                              //SqlHelper.RealDelete<TaskDtlModel>(w => w.taskModelID == item.SourceID);
                              //SqlHelper.RealDelete<TaskGroup>(w => w.taskModelID == item.SourceID);
                              
                              SqlHelper.Delete(currentTask);
                              
                              break;
                          case SourceType.TaskDtl:
                              var currentTaskDtl = SqlHelper.GetOne<TaskDtlModel>(w => w.ID == item.SourceID);
                              
                              List<FileAttachmentModel> fileAttachmentModels2 = new List<FileAttachmentModel>();
                              fileAttachmentModels2.AddRange(SqlHelper.GetInfoLst<FileAttachmentModel>(w => w.taskModelID == item.ID).ToList());
                              if (fileAttachmentModels2.Count > 0)
                              {
                                  foreach (var file in fileAttachmentModels2)
                                  {
                                      SqlHelper.Delete(file);
                                  }
                              }
                              SqlHelper.Delete(currentTaskDtl);
                              break;
                          case SourceType.TaskGroup:
                              var currentTaskGroup = SqlHelper.GetOne<TaskGroup>(w => w.ID == item.SourceID);
                              SqlHelper.Delete(currentTaskGroup);
                              break;
                          case SourceType.Note:
                              var currentNote = SqlHelper.GetOne<NoteModel>(w => w.ID == item.SourceID);
                              SqlHelper.Delete(currentNote);
                              break;
                          case SourceType.Attachment:
                              var currentAttachment = SqlHelper.GetOne<FileAttachmentModel>(w => w.ID == item.SourceID);
                              SqlHelper.Delete(currentAttachment);
                              break;

                      }
                      RecycleList.Remove(item);
                      SqlHelper.Delete(item);
                  }
                  
                  DialogWindow.Show("已成功清空回收站！", MessageType.Successful, WindowsManager.Windows["RecycleBinWindow"]);
              }
          });

        public RelayCommand<RecycleModel> UndoDeleteCommand => new RelayCommand<RecycleModel>((r) =>
          {
              if (r!=null)
              {
                  RecycleList.Remove(r);
                  switch (r.sourceType)
                  {
                      case SourceType.Task:
                          var currentTask = SqlHelper.GetOne<TaskModel>(w => w.ID == r.SourceID);
                          currentTask.IsDelete = false;
                          SqlHelper.Update(currentTask);
                          break;
                      case SourceType.TaskDtl:
                          var currentTaskDtl = SqlHelper.GetOne<TaskDtlModel>(w => w.ID == r.SourceID);
                          currentTaskDtl.IsDelete = false;
                          SqlHelper.Update(currentTaskDtl);
                          break;
                      case SourceType.TaskGroup:
                          var currentTaskGroup = SqlHelper.GetOne<TaskGroup>(w => w.ID == r.SourceID);
                          currentTaskGroup.IsDelete = false;
                          SqlHelper.Update(currentTaskGroup);
                          break;
                      case SourceType.Note:
                          var currentNote = SqlHelper.GetOne<NoteModel>(w => w.ID == r.SourceID);
                          currentNote.IsDelete = false;
                          SqlHelper.Update(currentNote);
                          break;
                      case SourceType.Attachment:
                          var currentAttachment = SqlHelper.GetOne<FileAttachmentModel>(w => w.ID == r.SourceID);
                          currentAttachment.IsDelete = false;
                          SqlHelper.Update(currentAttachment);
                          break;

                  }
                  DialogWindow.Show("已成功回退！", MessageType.Successful, WindowsManager.Windows["RecycleBinWindow"]);
                  SqlHelper.RealDelete<RecycleModel>(w => w.ID == r.ID);
              }
          });
        #endregion


    }
}
