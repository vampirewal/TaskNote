#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：FolderModel
// 创 建 者：杨程
// 创建时间：2021/6/3 13:24:29
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNote.Core.Models;

namespace TaskNote.Model
{
    [Table("Folders")]
    public class FolderModel : TopBase
    {
        public FolderModel()
        {
            //构造函数
            Childs = new ObservableCollection<FolderModel>();
        }

        #region 属性
        private string _FolderName;
        [Display(Name = "文件夹名称")]
        [Column("FolderName")]
        public string FolderName
        {
            get { return _FolderName; }
            set { _FolderName = value; DoNotify(); }
        }

        private string _ParentId;
        [Display(Name = "父文件夹ID")]
        [Column("ParentId")]
        public string ParentId
        {
            get { return _ParentId; }
            set { _ParentId = value; DoNotify(); }
        }

        private string _UserID;
        [Display(Name = "关联用户")]
        [Column("User")]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; DoNotify(); }
        }

        private User _User;
        [Display(Name = "关联用户")]
        [Column("User")]
        public User User
        {
            get { return _User; }
            set { _User = value; DoNotify(); }
        }




        private bool _CanDelete;
        [Display(Name = "能否删除")]
        [Column("CanDelete")]
        public bool CanDelete
        {
            get { return _CanDelete; }
            set { _CanDelete = value; DoNotify(); }
        }



        [NotMapped]
        public ObservableCollection<FolderModel> Childs { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令

        #endregion
    }
}
