#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：TaskViewModel
// 创 建 者：杨程
// 创建时间：2021/5/28 10:03:42
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using HandyControl.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TaskNote.Core;
using TaskNote.Core.SimpleMVVM;
using TaskNote.DataAccess;
using TaskNote.Model;
using TaskNote.Theme;

namespace TaskNote.ViewModel
{
    public class TaskViewModel:ViewModelBase
    {
        public TaskViewModel()
        {
            //构造函数
            Title = "Task";
        }

        #region 重写
        public override void InitData()
        {
            LoginUserInfo = GlobalDataManager.GetInstance().LoginUserInfo;
            TaskList = new ObservableCollection<TaskModel>();

            
            GetTaskData();
        }

        public override void MessengerRegister()
        {
            Messenger.Default.Register(this, "CreateNewTask", CreateNewTask);
            Messenger.Default.Register<TaskModel>(this, "OpenBottomPanel", OpenBottomPanel);
        }
        #endregion

        #region 属性
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

        private TaskDtlModel _SelectedTaskDtlModel;
        /// <summary>
        /// 被选择的任务明细
        /// </summary>
        public TaskDtlModel SelectedTaskDtlModel
        {
            get { return _SelectedTaskDtlModel; }
            set { _SelectedTaskDtlModel = value; DoNotify(); }
        }


        public User LoginUserInfo { get; set; }

        public ObservableCollection<TaskModel> TaskList { get; set; }

        /// <summary>
        /// 存储从数据库中获取到的数据
        /// </summary>
        //public ObservableCollection<TaskModel> AllTask { get; set; }

        #region 分页
        ///// <summary>
        /////     页码
        ///// </summary>
        //private int _pageIndex = 1;

        ///// <summary>
        /////     页码
        ///// </summary>
        //public int PageIndex
        //{
        //    get => _pageIndex;
        //    set
        //    {
        //        _pageIndex = value;
        //        DoNotify();
        //    }
        //}
        #endregion

        #endregion

        #region 公共方法

        #endregion

        #region 私有方法
        /// <summary>
        /// 初始化的时候，获取整体数据
        /// </summary>
        private void GetTaskData()
        {
            TaskList.Clear();
            var current = SqlHelper.GetInfoLst<TaskModel>(w => w.UserID == LoginUserInfo.ID&&w.IsDelete==false).ToList();
            current.Sort((left, right) =>
            {
                if (left.CreateTime > right.CreateTime)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });

            foreach (var item in current)
            {
                var currentDtl = SqlHelper.GetInfoLst<TaskDtlModel>(w => w.taskModelID == item.ID&&w.IsDelete==false).ToList();
                item.FinishedTaskDtl = currentDtl.Where(w => w.IsFinished == true).Count();
                item.NoFinishedTaskDtl = currentDtl.Where(w => w.IsFinished == false).Count();

                TaskList.Add(item);
            }
            
        }

        /// <summary>
        /// 获取选择的任务获取明细和附件
        /// </summary>
        /// <param name="task"></param>
        private void GetSelectTaskDtlAndAttachment(TaskModel task)
        {
            SelectTaskModel.Detail.Clear();
            SelectTaskModel.fileAttachmentModels.Clear();
            SelectTaskModel.TaskGroups.Clear();

            var current = SqlHelper.GetInfoLst<TaskDtlModel>(w => w.taskModelID == task.ID && w.IsDelete == false)?.ToList();
            if (current != null && current.Count > 0)
            {
                foreach (var item in current)
                {
                    SelectTaskModel.Detail.Add(item);
                }
            }

            var currentFileAttachments = SqlHelper.GetInfoLst<FileAttachmentModel>(w => w.taskModelID == task.ID&&w.IsDelete==false)?.ToList();
            if (currentFileAttachments != null && currentFileAttachments.Count > 0)
            {
                foreach (var item in currentFileAttachments)
                {
                    SelectTaskModel.fileAttachmentModels.Add(item);
                }
            }

            var currentTaskGroup = SqlHelper.GetInfoLst<TaskGroup>(w => w.taskModelID == task.ID&&w.IsDelete==false)?.ToList();
            if (currentTaskGroup != null && currentTaskGroup.Count > 0)
            {
                foreach (var item in currentTaskGroup)
                {
                    SelectTaskModel.TaskGroups.Add(item);
                }
            }
            
        }

        /// <summary>
        /// 创建新的任务版
        /// </summary>
        private void CreateNewTask()
        {
            //创建一个TaskModel类
            TaskModel tm = new TaskModel()
            {
                TaskName = "new",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                IsImportant=false,
                UserID=LoginUserInfo.ID,
                CreateTime=DateTime.Now,
                IsDelete=false,
            };
            //插入第1条的位置
            TaskList.Insert(0,tm);

            //创建一个默认的任务组
            ObservableCollection<TaskGroup> DefaultTaskGroup = new ObservableCollection<TaskGroup>()
            {
                new TaskGroup()
                {
                    taskModelID=tm.ID,
                    GroupName="准备去做",
                    GroupBackgroundColor="#FF4040",
                    IsDelete=false,
                    IsCanDelete=false,
                    GroupSort=0,
                    CreateTime=DateTime.Now,
                    IsFinishedTag=false
                },
                new TaskGroup()
                {
                    taskModelID=tm.ID,
                    GroupName="正在做",
                    GroupBackgroundColor="#CFB53B",
                    IsDelete=false,
                    IsCanDelete=false,
                    GroupSort=1,
                    CreateTime=DateTime.Now,
                    IsFinishedTag=false
                },
                new TaskGroup()
                {
                    taskModelID=tm.ID,
                    GroupName="已完成",
                    GroupBackgroundColor="#32CD32",
                    IsDelete=false,
                    IsCanDelete=false,
                    GroupSort=2,
                    CreateTime=DateTime.Now,
                    IsFinishedTag=true
                }
            };

            //tm.TaskGroups = DefaultTaskGroup;

            SqlHelper.AddEntity(tm);
            SqlHelper.AddEntityList(DefaultTaskGroup.ToList());

            
        }

        /// <summary>
        /// 打开底部信息面板
        /// </summary>
        /// <param name="t"></param>
        private void OpenBottomPanel(TaskModel t)
        {
            SelectTaskModel = t;
            GetSelectTaskDtlAndAttachment(t);
            SelectedTaskDtlModel = null;
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
        #endregion

        #region 命令
        /// <summary>
        /// 刷新界面命令
        /// </summary>
        public RelayCommand RefreshCommand => new RelayCommand(() =>
          {
              GetTaskData();

          });

        #region 详细信息面板开启与关闭
        public bool IsCollapsed { get; set; } = true;
        public RelayCommand<TaskModel> LookTaskInfoCommand => new RelayCommand<TaskModel>((t) =>
        {
            if (t != null)
            {
                //给下方的赋值
                OpenBottomPanel(t);

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
        #endregion

        public RelayCommand<RowDefinition> WindowLoadCommand => new RelayCommand<RowDefinition>((o) =>
        {
            BottomHeight = o;
        });

        #region 附件相关命令
        /// <summary>
        /// 上传附件命令
        /// </summary>
        public RelayCommand UpLoadAttachmentCommand => new RelayCommand(() =>
        {
            if (SelectTaskModel == null)
            {
                //先判断一下当前选择的工作，是否为空

                return;
            }

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "请选择要上传的文件",
                Filter = "Excel文件(2003以上)|*.xlsx|Excel文件（97-2003）|*.xls|Word文件|*.docx|文本文件|*.txt|JPG图片|*.jpg|PNG图片|*.png",
                FileName = string.Empty,
                Multiselect = true
            };

            if (ofd.ShowDialog() == true)
            {
                Task.Run(() =>
                {

                    List<string> filefullName = ofd.FileNames.ToList();
                    foreach (var filePath in filefullName)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(filePath);
                        string fileType = Path.GetExtension(filePath);
                        try
                        {
                            //FileInfo fileInfo = new FileInfo(filePath);

                            using (FileStream fs = new FileStream(filePath, FileMode.Open))
                            {
                                BinaryReader brFile = new BinaryReader(fs);
                                Byte[] byData = brFile.ReadBytes((int)fs.Length);
                                FileAttachmentModel fam = new FileAttachmentModel()
                                {

                                    AttachmentName = fileName,
                                    taskModelID = SelectTaskModel.ID,
                                    Attachment = byData,
                                    AttachmentType = fileType,
                                    AttachmentByte = byData.Length / 1024

                                };
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    SelectTaskModel.fileAttachmentModels.Add(fam);
                                });

                                SqlHelper.AddEntity(fam);

                                
                            }
                        }
                        catch
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                DialogWindow.Show($"{fileName} 上传文件失败，请查看上传文件是否已经被打开！", MessageType.Error, WindowsManager.Windows["MainWindow"]);
                            });
                        }
                    }
                });
            }
        });

        /// <summary>
        /// 下载附件命令
        /// </summary>
        public RelayCommand<FileAttachmentModel> DownloadAttachment => new RelayCommand<FileAttachmentModel>((f) =>
        {
            byte[] currentFile = f.Attachment;
            //Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 获取桌面路径
            BinaryWriter bw = new BinaryWriter(File.Open($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{f.AttachmentName}{f.AttachmentType}", FileMode.OpenOrCreate));
            bw.Write(currentFile);
            bw.Close();
            DialogWindow.Show("已成功将文件保存到桌面！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
        });
        /// <summary>
        /// 删除附件命令
        /// </summary>
        public RelayCommand<FileAttachmentModel> DeleteAttachmentCommand => new RelayCommand<FileAttachmentModel>((f) =>
        {
            SelectTaskModel.fileAttachmentModels.Remove(f);
            SqlHelper.RealDelete<FileAttachmentModel>(w => w.ID == f.ID);
            //SqlHelper.FalseDelete(f, f.AttachmentName, SourceType.Attachment);


            DialogWindow.Show("已成功将附件删除！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
        });
        #endregion

        public RelayCommand AddNewTaskDtlCommand => new RelayCommand(() =>
          {
              if (SelectTaskModel.TaskGroups!=null&& SelectTaskModel.TaskGroups.Count>0)
              {
                  TaskDtlModel taskDtl = new TaskDtlModel()
                  {
                      taskModelID = SelectTaskModel.ID,
                      TaskGroupID = SelectTaskModel.TaskGroups[0]?.ID,
                      IsDelete = false,
                      CreateTime = DateTime.Now,
                      TaskContext = "新任务明细",
                      IsChecked = false
                  };

                  SelectTaskModel.Detail.Add(taskDtl);

                  SqlHelper.AddEntity(taskDtl);
              }
              else
              {
                  DialogWindow.Show("获取任务组出错！", MessageType.Error, WindowsManager.Windows["MainWindow"]);
              }
          });

        /// <summary>
        /// 将任务放入回收站功能
        /// </summary>
        public RelayCommand<TaskModel> RecycleBinTaskCommand => new RelayCommand<TaskModel>((t) =>
          {
              if (t!=null)
              {
                  if (DialogWindow.ShowDialog($"是否确定将<{t.TaskName}>任务放入回收站？", "请确认"))
                  {
                      t.IsDelete = true;
                      TaskList.Remove(t);
                      SqlHelper.FalseDelete(t,t.TaskName,SourceType.Task);
                  }
              }
              
          });

        /// <summary>
        /// 保存选择项的数据
        /// </summary>
        public RelayCommand SaveCommand => new RelayCommand(() =>
          {
              try
              {
                  SqlHelper.Update(SelectTaskModel);
                  SqlHelper.UpdateList(SelectTaskModel.Detail.ToList());
                  SqlHelper.UpdateList(SelectTaskModel.TaskGroups.ToList());
                  
                  DialogWindow.Show("保存成功！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
              }
              catch 
              {
                  DialogWindow.Show("保存失败！",MessageType.Error,WindowsManager.Windows["MainWindow"]);
              }
              
          });

        public RelayCommand<TaskModel> IsImportantCommand => new RelayCommand<TaskModel>((s) =>
          {
              if (s.IsImportant)
              {

                  Messenger.Default.Send("RemoveFocusTask", s);
              }
              else
              {
                  Messenger.Default.Send("AddFocusTask", s);
              }
              s.IsImportant = !s.IsImportant;

              SqlHelper.Update(s);
              
          });

        /// <summary>
        /// 删除任务明细
        /// </summary>
        public RelayCommand<TaskDtlModel> DeleteTaskDtlCommand => new RelayCommand<TaskDtlModel>((t) =>
          {
              if (t!=null)
              {
                  if(DialogWindow.ShowDialog("是否确定要删除该任务明细吗？", "请确认"))
                  {
                      SelectTaskModel.Detail.Remove(t);
                      //SqlHelper.FalseDelete(t,t.TaskContext,SourceType.TaskDtl);
                      SqlHelper.Delete(t);
                      DialogWindow.Show("删除成功！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
                  }
              }
          });


        public RelayCommand<string> SelectTaskGroupChangedCommand => new RelayCommand<string>((s) =>
          {
              if (!string.IsNullOrEmpty(s)&& SelectedTaskDtlModel!=null)
              {
                  SelectedTaskDtlModel.TaskGroupID = s;
                  

                  var current = SqlHelper.GetOne<TaskGroup>(w => w.ID == s);
                  if (current!=null&&current.IsFinishedTag==true)
                  {
                      SelectedTaskDtlModel.IsFinished = true;

                  }
                  else if(SelectedTaskDtlModel.IsFinished == true)
                  {
                      SelectedTaskDtlModel.IsFinished = false;
                  }
                  SqlHelper.Update(SelectedTaskDtlModel);
              }
          });

        public RelayCommand SettingTaskGroupCommand => new RelayCommand(() =>
          {
              if (SelectTaskModel!=null)
              {
                  SelectTaskModel.TaskGroups=JsonHelper.ConvertObject<ObservableCollection<TaskGroup>>(
                      WindowsManager.CreateDialogWindowByViewModelResult(
                          "SettingTaskGroupView",
                      new SettingTaskGroupViewModel(), 
                      new 
                      {
                          TaskModelId=SelectTaskModel.ID,taskGroups= SelectTaskModel.TaskGroups
                      }
                      ));
                  GetSelectTaskDtlAndAttachment(SelectTaskModel);
              }
          });


        ///// <summary>
        /////     页码改变命令
        ///// </summary>
        //public RelayCommand<FunctionEventArgs<int>> PageUpdatedCmd =>new Lazy<RelayCommand<FunctionEventArgs<int>>>(() =>
        //        new RelayCommand<FunctionEventArgs<int>>(PageUpdated)).Value;

        ///// <summary>
        /////     页码改变
        ///// </summary>
        //private void PageUpdated(FunctionEventArgs<int> info)
        //{
        //    //TaskList = _totalDataList.Skip((info.Info - 1) * 20).Take(20).ToList();
        //    var current = SqlHelper.GetInfoLst<TaskModel>(w => w.UserID == LoginUserInfo.ID && w.IsDelete == false).OrderByDescending(o=>o.CreateTime).Skip((info.Info - 1) * 20).Take(20).ToList();


        //}

        /// <summary>
        /// 搜索按钮命令
        /// </summary>
        public RelayCommand<string> SearchCommand => new RelayCommand<string>((s) =>
          {
              if (!string.IsNullOrEmpty(s))
              {
                  List<TaskModel> current = new List<TaskModel>();

                  foreach (var item in TaskList)
                  {
                      if (!string.IsNullOrEmpty( item.TaskName))
                      {
                          if (item.TaskName.Contains(s))
                          {
                              current.Add(item);
                              continue;
                          }
                      }

                      if (!string.IsNullOrEmpty(item.TaskDes))
                      {
                          if (item.TaskDes.Contains(s))
                          {
                              current.Add(item);
                              continue;
                          }
                      }


                  }

                  if (current.Count>0)
                  {
                      TaskList.Clear();
                      foreach (var item in current)
                      {
                          TaskList.Add(item);
                      }
                  }
                  else
                  {
                      DialogWindow.Show("未找到符合条件的TASK，请重新输入！", MessageType.Error, WindowsManager.Windows["MainWindow"]);
                  }
              }
              else
              {
                  DialogWindow.Show("请输入搜索条件后再点击！", MessageType.Error, WindowsManager.Windows["MainWindow"]);
              }
          });
        #endregion

        #region 消息




        #endregion
    }
}
