#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：NoteViewModel
// 创 建 者：杨程
// 创建时间：2021/6/3 14:12:20
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
    public class NoteViewModel:ViewModelBase
    {
        public NoteViewModel()
        {
            //构造函数
        }

        #region 重写
        public override void InitData()
        {
            LoginUserInfo = GlobalDataManager.GetInstance().LoginUserInfo;
            
            FolderList = new ObservableCollection<FolderModel>();
            NoteList = new ObservableCollection<NoteModel>();
            GetFolderData();
        }
        #endregion


        #region 属性
        public User LoginUserInfo { get; set; }
        public ObservableCollection<FolderModel> FolderList { get; set; }

        private FolderModel _SelectFolder;
        /// <summary>
        /// 选择的文件夹
        /// </summary>
        public FolderModel SelectFolder
        {
            get { return _SelectFolder; }
            set { _SelectFolder = value; DoNotify(); }
        }

        private NoteModel _SelectNote;

        public NoteModel SelectNote
        {
            get { return _SelectNote; }
            set { _SelectNote = value; DoNotify(); }
        }


        public ObservableCollection<NoteModel> NoteList { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法
        private void GetFolderData()
        {
            FolderList.Clear();

            var current = SqlHelper.GetInfoLst<FolderModel>(w => w.UserId == LoginUserInfo.ID).ToList();
            FolderModel firstFolder = current.Find(f => f.ParentId == "0");
            FolderList.Add(firstFolder);
            BuildTreeList(current, firstFolder);
            
        }

        /// <summary>
        /// 递归获取文件夹并绑定
        /// </summary>
        /// <param name="folderModels"></param>
        /// <param name="Father"></param>
        private void BuildTreeList(List<FolderModel> folderModels, FolderModel Father)
        {
            var current = folderModels.Where(w => w.ParentId == Father.ID).ToList();
            foreach (var item in current)
            {
                Father.Childs.Add(item);

                var currentChilds = folderModels.Where(w => w.ParentId == item.ID).ToList();
                if (currentChilds.Count > 0)
                {
                    BuildTreeList(folderModels, item);
                }
            }

        }
        #endregion

        #region 命令
        public RelayCommand<FolderModel> SelectFolderGetNoteCommand => new RelayCommand<FolderModel>((f) =>
          {
              if (f!=null)
              {
                  NoteList.Clear();
                  SelectNote = null;
                  SelectFolder = f;
                  SaveNoteCommand.RaiseCanExecuteChanged();
                  var current = SqlHelper.GetInfoLst<NoteModel>(w => w.FolderId == f.ID).ToList();
                  if (current.Count > 0)
                  {
                      foreach (var item in current)
                      {
                          NoteList.Add(item);
                      }
                  }
              }
          });


        public RelayCommand CreateNoteCommand => new RelayCommand(() =>
          {
              if (SelectFolder != null)
              {
                  NoteModel note = new NoteModel()
                  {
                      FolderId = SelectFolder.ID,
                      IsDelete = false,
                      NoteName = "新建笔记",
                      NoteContext = "（无）",
                      UserId = LoginUserInfo.ID,
                      CreateTime = DateTime.Now,
                      IsFocus=false
                  };

                  SqlHelper.AddEntity(note);

                  NoteList.Add(note);
              }
              else
              {
                  DialogWindow.Show("请先选择文件夹！", MessageType.Error, WindowsManager.Windows["MainWindow"]);
              }
              
              
          });

        public RelayCommand<FolderModel> AddNewFolderCommand => new RelayCommand<FolderModel>((f) =>
          {
              if (f!=null)
              {
                  
                  var current=(FolderModel) WindowsManager.CreateDialogWindowByViewModelResult("AddOrEditFolderView", new AddOrEditFolderViewModel(),
                      new { Type ="新建", folderModel =f});

                  SqlHelper.AddEntity(current);
                  
                  f.Childs.Add(current);
              }
          });

        public RelayCommand<FolderModel> EditFolderCommand => new RelayCommand<FolderModel>((f) =>
        {
            if (f != null)
            {

                var current = (FolderModel)WindowsManager.CreateDialogWindowByViewModelResult("AddOrEditFolderView", new AddOrEditFolderViewModel(),
                    new { Type = "修改", folderModel = f });

                SqlHelper.Update(current);
                
            }
        });

        public RelayCommand<FolderModel> DeleteFolderCommand => new RelayCommand<FolderModel>((f) =>
          {
              if (DialogWindow.ShowDialog("是否删除文件夹？\r\n该操作会将目录下的笔记转移至上级目录下", "请确认"))
              {
                  var currentNote = SqlHelper.GetInfoLst<NoteModel>(w => w.FolderId == f.ID).ToList();
                  if (currentNote.Count > 0)
                  {
                      foreach (var item in currentNote)
                      {
                          item.FolderId = f.ParentId;
                      }
                  }
                  SqlHelper.UpdateList(currentNote);
                  SqlHelper.RealDelete<FolderModel>(w=>w.ID==f.ID);
                  GetFolderData();
                  
              }
          });

        public RelayCommand<NoteModel> SelectNoteCommand => new RelayCommand<NoteModel>((s) =>
          {
              if (s!=null)
              {
                  SelectNote = s;
                  SaveNoteCommand.RaiseCanExecuteChanged();
              }
          });

        public RelayCommand<object> SaveNoteCommand => new RelayCommand<object>((o) =>
          {
              if (SelectNote!=null)
              {
                  SqlHelper.Update(SelectNote);
                  
                  DialogWindow.Show("保存成功！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
              }
          }, (o) =>
          {
              if (SelectNote == null)
              {
                  return false;
              }
              else
              {
                  return true;
              }
          });

        public RelayCommand<NoteModel> FocusNoteCommand => new RelayCommand<NoteModel>((n) =>
          {
              if (n!=null)
              {
                  if (!n.IsFocus)
                  {
                      n.IsFocus = !n.IsFocus;
                      //添加关注列表
                      Messenger.Default.Send("AddNoteList", n);
                  }
                  else
                  {
                      n.IsFocus = !n.IsFocus;
                      //移除关注列表
                      Messenger.Default.Send("RemoveNoteList", n);
                  }

                  SqlHelper.Update(n);
                  
              }
          });

        public RelayCommand<NoteModel> DeleteNoteCommand => new RelayCommand<NoteModel>((n) =>
          {
              if (n!=null)
              {
                  if (DialogWindow.ShowDialog("是否确定删除该笔记？", "请确认"))
                  {
                      NoteList.Remove(n);
                      if (n.IsFocus)
                      {
                          Messenger.Default.Send("RemoveNoteList", n);
                      }
                      SqlHelper.Delete<NoteModel>(w => w.ID == n.ID);
                      
                      DialogWindow.Show("删除成功！", MessageType.Successful, WindowsManager.Windows["MainWindow"]);
                  }
              }
          });
        #endregion
    }
}
