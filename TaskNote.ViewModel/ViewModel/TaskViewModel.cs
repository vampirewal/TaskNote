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

            using (TaskNoteDataAccess tn = new TaskNoteDataAccess())
            {
                var current = tn.TaskDtl.Where(w => w.taskModelID == task.ID)?.ToList();
                if (current==null)
                {
                    return;
                }
                var currentFileAttachments = tn.fileAttachments.Where(w => w.ParentGuidId == task.ID)?.ToList();
                if (currentFileAttachments == null)
                {
                    return;
                }
                foreach (var item in current)
                {
                    SelectTaskModel.Detail.Add(item);
                }

                foreach (var item in currentFileAttachments)
                {
                    SelectTaskModel.fileAttachmentModels.Add(item);
                }
            }
        }

        /// <summary>
        /// 创建新的任务版
        /// </summary>
        private void CreateNewTask()
        {
            TaskModel tm = new TaskModel()
            {
                TaskName = "new",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                
                UserID=LoginUserInfo.ID,
                CreateTime=DateTime.Now,
                IsDelete=false,
            };

            TaskList.Insert(0,tm);

            using (TaskNoteDataAccess tn = new TaskNoteDataAccess())
            {
                try
                {
                    tn.Add(tm);
                    tn.SaveChanges();
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
                                    work.SaveChanges();
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
                work.SaveChanges();
            }
            DialogWindow.Show("已成功将文件删除！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
        });
        #endregion

        public RelayCommand AddNewTaskDtlCommand => new RelayCommand(() =>
          {

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
