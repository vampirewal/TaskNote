#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：AddOrEditFolderViewModel
// 创 建 者：杨程
// 创建时间：2021/6/3 16:11:14
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
using System.Threading.Tasks;
using TaskNote.Core.SimpleMVVM;
using TaskNote.Model;
using TaskNote.ViewModel.Common;

namespace TaskNote.ViewModel
{
    public class AddOrEditFolderViewModel : ViewModelBase
    {
        

        public AddOrEditFolderViewModel()
        {
            //构造函数
        }

        public override object GetResult()
        {
            return folderModel;
        }

        public override void InitData()
        {
            
        }

        public override void PassData(object obj)
        {
            //folderModel = (FolderModel)obj ;
            PassValue value = ConverterHelper.ConvertObject<PassValue>(obj);
            if (value.Type=="新建")
            {
                Title = "新建文件夹";
                folderModel = new FolderModel()
                {
                    CanDelete = true,
                    FolderName = "新建文件夹",
                    IsDelete = false,
                    ParentId = value.folderModel.ID,
                    UserId = value.folderModel.UserId,
                    CreateTime = DateTime.Now
                };
            }
            else
            {
                Title = "修改文件夹";
                folderModel = value.folderModel;
            }
        }

        private FolderModel _folderModel;
        public FolderModel folderModel { get => _folderModel; set { _folderModel = value;DoNotify(); } }


        #region 属性

        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令
        public RelayCommand QueDingCommand => new RelayCommand(() =>
          {
              
          });
        #endregion

        
    }
    public class PassValue
    {
        public string Type { get; set; }
        public FolderModel folderModel { get; set; }
    }
}
