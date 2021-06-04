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

            MessengerRegister();
            GetTaskData();
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
            using (TaskNoteDataAccess taskNote = new TaskNoteDataAccess())
            {
                var current = taskNote.TaskS.Where(w => w.UserID == LoginUserInfo.ID).ToList();

                current.Sort((left, right) =>
                {
                    if (left.CreateTime > right.CreateTime)
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
                    var currentDtl = taskNote.TaskDtl.Where(w => w.taskModelID == item.ID).ToList();
                    item.FinishedTaskDtl = currentDtl.Where(w => w.IsFinished == true).Count();
                    item.NoFinishedTaskDtl= currentDtl.Where(w => w.IsFinished == false).Count();

                    TaskList.Add(item);
                }
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
            SelectTaskModel.TaskDtls.Clear();

            using (TaskNoteDataAccess tn = new TaskNoteDataAccess())
            {
                var current = tn.TaskDtl.Where(w => w.taskModelID == task.ID)?.ToList();
                if (current!=null&& current.Count>0)
                {
                    foreach (var item in current)
                    {
                        SelectTaskModel.Detail.Add(item);
                    }
                }
                var currentFileAttachments = tn.fileAttachments.Where(w => w.ParentGuidId == task.ID)?.ToList();
                if (currentFileAttachments != null&& currentFileAttachments.Count>0)
                {
                    foreach (var item in currentFileAttachments)
                    {
                        SelectTaskModel.fileAttachmentModels.Add(item);
                    }
                }
                var currentTaskGroup = tn.TaskGroups.Where(w => w.taskModelID == task.ID)?.ToList();
                if (currentTaskGroup != null&& currentTaskGroup.Count>0)
                {
                    foreach (var item in currentTaskGroup)
                    {
                        SelectTaskModel.TaskGroups.Add(item);
                    }
                }

                var currentTaskDtls = tn.TaskDtl.Where(w => w.taskModelID == task.ID)?.ToList();
                if (currentTaskDtls!=null&& currentTaskDtls.Count>0)
                {
                    foreach (var item in currentTaskDtls)
                    {
                        SelectTaskModel.TaskDtls.Add(item);
                    }
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
                    CreateTime=DateTime.Now
                },
                new TaskGroup()
                {
                    taskModelID=tm.ID,
                    GroupName="正在做",
                    GroupBackgroundColor="#CFB53B",
                    IsDelete=false,
                    IsCanDelete=false,
                    GroupSort=1,
                    CreateTime=DateTime.Now
                },
                new TaskGroup()
                {
                    taskModelID=tm.ID,
                    GroupName="已完成",
                    GroupBackgroundColor="#32CD32",
                    IsDelete=false,
                    IsCanDelete=false,
                    GroupSort=2,
                    CreateTime=DateTime.Now
                }
            };

            tm.TaskGroups = DefaultTaskGroup;
            using (TaskNoteDataAccess tn = new TaskNoteDataAccess())
            {
                try
                {
                    tn.Add(tm);
                    tn.AddRange(DefaultTaskGroup);
                    tn.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    DialogWindow.Show(e.ToString(), MessageType.Error, WindowsManager.Windows["MainWindow"]);
                    
                }
                
            }
        }
        #endregion

        #region 命令

        #region 详细信息面板开启与关闭
        public bool IsCollapsed { get; set; } = true;
        public RelayCommand<TaskModel> LookTaskInfoCommand => new RelayCommand<TaskModel>((t) =>
        {
            if (t != null)
            {
                //给下方的赋值
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
                                    ParentGuidId = SelectTaskModel.ID,
                                    Attachment = byData,
                                    AttachmentType = fileType,
                                    AttachmentByte = byData.Length / 1024

                                };
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    SelectTaskModel.fileAttachmentModels.Add(fam);
                                });

                                using (TaskNoteDataAccess work = new TaskNoteDataAccess())
                                {
                                    work.fileAttachments.Add(fam);
                                    work.SaveChangesAsync();
                                }
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
            using (TaskNoteDataAccess work = new TaskNoteDataAccess())
            {
                work.fileAttachments.Remove(f);
                work.SaveChangesAsync();
            }
            DialogWindow.Show("已成功将文件删除！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
        });
        #endregion

        public RelayCommand AddNewTaskDtlCommand => new RelayCommand(() =>
          {
              TaskDtlModel taskDtl = new TaskDtlModel()
              {
                  taskModelID = SelectTaskModel.ID,
                  TaskGroupID= SelectTaskModel.TaskGroups[0]?.ID,
                  IsDelete=false,
                  CreateTime=DateTime.Now,
                  TaskContext="新任务明细",
                  IsChecked=false
              };

              SelectTaskModel.TaskDtls.Add(taskDtl);
              using(TaskNoteDataAccess task =new TaskNoteDataAccess())
              {
                  task.Add(taskDtl);
                  task.SaveChangesAsync();
              }
          });

        /// <summary>
        /// 将任务放入回收站功能
        /// </summary>
        public RelayCommand<TaskModel> RecycleBinTaskCommand => new RelayCommand<TaskModel>((t) =>
          {
              if(DialogWindow.ShowDialog($"是否确定将<{t.TaskName}>任务放入回收站？", "请确认"))
              {
                  t.IsDelete = true;
                  using(TaskNoteDataAccess task=new TaskNoteDataAccess())
                  {
                      task.Update(t);
                      task.SaveChangesAsync();
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
                  using (TaskNoteDataAccess task = new TaskNoteDataAccess())
                  {
                      task.Update(SelectTaskModel);
                      task.UpdateRange(SelectTaskModel.TaskDtls);
                      task.UpdateRange(SelectTaskModel.TaskGroups);

                      task.SaveChangesAsync();
                  }
                  DialogWindow.Show("保存成功！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
              }
              catch 
              {
                  DialogWindow.Show("保存失败！",MessageType.Error,WindowsManager.Windows["MainWindow"]);
              }
              
          });

        public RelayCommand<TaskModel> IsImportantCommand => new RelayCommand<TaskModel>((s) =>
          {
              using(TaskNoteDataAccess task=new TaskNoteDataAccess())
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
                  task.Update(s);
                  task.SaveChangesAsync();
              }
              
          });

        public RelayCommand<TaskDtlModel> DeleteTaskDtlCommand => new RelayCommand<TaskDtlModel>((t) =>
          {
              if (t!=null)
              {
                  if(DialogWindow.ShowDialog("是否确定要删除该任务明细？一经删除无法恢复！", "请确认"))
                  {
                      SelectTaskModel.Detail.Remove(t);
                      using (TaskNoteDataAccess task=new TaskNoteDataAccess())
                      {
                          task.Remove(t);
                          task.SaveChangesAsync();
                          
                          DialogWindow.Show("删除成功！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
                      }
                  }
              }
          });


        public RelayCommand<string> SelectTaskGroupChangedCommand => new RelayCommand<string>((s) =>
          {
              if (!string.IsNullOrEmpty(s)&& SelectedTaskDtlModel!=null)
              {
                  SelectedTaskDtlModel.TaskGroupID = s;
                  using(TaskNoteDataAccess task=new TaskNoteDataAccess())
                  {
                      task.Update(SelectedTaskDtlModel);
                      task.SaveChangesAsync();
                  }
              }
          });

        public RelayCommand SettingTaskGroupCommand => new RelayCommand(() =>
          {
              if (SelectTaskModel!=null)
              {
                  SelectTaskModel.TaskGroups=JsonHelper.ConvertObject<ObservableCollection<TaskGroup>>( WindowsManager.CreateDialogWindowByViewModelResult("SettingTaskGroupView", new SettingTaskGroupViewModel(), new { TaskModelId=SelectTaskModel.ID,taskGroups= SelectTaskModel.TaskGroups }));
              }
          });
        #endregion

        #region 消息
        /// <summary>
        /// 消息注册
        /// </summary>
        private void MessengerRegister()
        {
            Messenger.Default.Register(this, "CreateNewTask", CreateNewTask);
        }


        #endregion
    }
}
